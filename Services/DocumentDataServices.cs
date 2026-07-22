using Azure.AI.DocumentIntelligence;
using DocumentData.Model;
using DocumentType.Services;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Threading;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using Azure;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Text.Json;
using Azure.Core;
using System.ClientModel.Primitives;
using ResultValidation.Services;
using Microsoft.Extensions.Logging;
using Logger;

namespace DocumentData.Sevices;

public class DocumentDataServices: IDocumentDataServices
{
    private readonly DocumentIntelligenceClient _client;
    private readonly string _modelID;
    private readonly string _classificationModelID;
    private readonly DocType _docType;
    private readonly ILoggerService _logger;
    public DocumentDataServices(IConfiguration configuration, DocType docType, ILoggerService logger)
    {
        var endpoint = configuration["DocumentIntelligenceCustom:Endpoint"];
        var apiKey = configuration["DocumentIntelligenceCustom:ApiKey"];
        _modelID = configuration["DocumentIntelligenceCustom:ModelID"];
        _classificationModelID = configuration["DocumentIntelligenceCustom:ClassificationModelID"];
        _client = new DocumentIntelligenceClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
        _docType = docType;
        _logger= logger;
    }
    public async Task<documentData> DocumentDataExtraction(IFormFile dokument, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Pocetak obrade dokumenta");
            resultValidation prov = new resultValidation();
            var documentType = await GetDocumentType(
                dokument);
            using var memoryStream = new MemoryStream();

            await dokument.CopyToAsync(
                memoryStream,
                cancellationToken);

            memoryStream.Position = 0;

            var operation = await _client.AnalyzeDocumentAsync(
                WaitUntil.Completed,
                _modelID,
                BinaryData.FromStream(memoryStream),
                cancellationToken);


            var result = operation.Value;

            var document = result.Documents[0];
            var data = new documentData
            {
                DocumentType = documentType,
                DocumentNumber = GetField(document, "InvoiceId"),
                DocumentTime = GetField(document, "InvoiceDate"),
                CustomerName = GetField(document, "CustomerName"),
                CustomerAddress = GetField(document, "CustomerAddress"),
                CustomerVAT = GetField(document, "CustomerTaxId"),
                Currency = GetField(document, "Currency"),
                Osnovica = GetField(document, "SubTotal"),
                PDV = GetField(document, "TotalTax"),
                FullPrize = GetField(document, "InvoiceTotal"),
                Artikli = GetStavke(document)
            };
            if (!prov.Validation(data))
            {
                _logger.LogWarning("Dokument nije prosao validaciju");
                return null;
            }
            return data;
        }
        catch (Exception ex)
        {
            _logger.LogError("Greska prilikom obrade dokumenta");
            throw new Exception("Greska prilikom obrade PDF dokumenta:" + ex.Message);
        }
    }
    private string? GetField(
        AnalyzedDocument document,
        string fieldName)
    {
        if (document.Fields.TryGetValue(fieldName, out var field))
        {
            return field.Content;
        }

        return null;
    }
    private List<Stavke> GetStavke(AnalyzedDocument document)
    {
        List<Stavke> stavke = new();
        if (!document.Fields.TryGetValue("Items", out var itemsField))
            return stavke;
        foreach (var item in itemsField.ValueList)
        {
            var polja = item.ValueDictionary;

            Stavke stavka = new Stavke();

            if (polja.TryGetValue("Description", out var naziv))
                stavka.NazivStavke = naziv.Content;

            if (polja.TryGetValue("ProductCode", out var id))
                stavka.IdStavke = id.Content;

            if (polja.TryGetValue("Quantity", out var kolicina))
                stavka.Kolicina = kolicina.Content;

            if (polja.TryGetValue("UnitPrice", out var cijena))
                stavka.Cijena = cijena.Content;

            if (polja.TryGetValue("Amount", out var ukupno))
                stavka.Ukupno = ukupno.Content;

            stavke.Add(stavka);
        }
        return stavke;
    }
    private async Task<string> GetDocumentType(IFormFile dokument)
    {
        using var memoryStream = new MemoryStream();
        await dokument.CopyToAsync(memoryStream);
        var bytes = memoryStream.ToArray();
        var base64 = Convert.ToBase64String(bytes);

        var content = RequestContent.Create(new
        {
            base64Source = base64
        });

        var operation = await _client.ClassifyDocumentAsync(
            WaitUntil.Completed,
            _classificationModelID,
            content);

        using JsonDocument doc = JsonDocument.Parse(operation.GetRawResponse().Content.ToString());

        var documents = doc.RootElement
            .GetProperty("analyzeResult")
            .GetProperty("documents");

        if (documents.GetArrayLength() == 0)
        {
            throw new InvalidOperationException("No documents returned by classifier.");
        }

        return documents[0].GetProperty("docType").GetString()!;
    }
}