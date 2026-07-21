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


namespace DocumentData.Sevices;

public class DocumentDataServices
{
    private readonly DocumentIntelligenceClient _client;
    private readonly string _modelID;
    private readonly string _classificationModelID;
    private readonly DocType _docType;
    public DocumentDataServices(IConfiguration configuration, DocType docType)
    {
        Console.WriteLine("DOCUMENT DATA SERVICE CREATED");
        var endpoint = configuration["DocumentIntelligenceCustom:Endpoint"];
        var apiKey = configuration["DocumentIntelligenceCustom:ApiKey"];
        _modelID = configuration["DocumentIntelligenceCustom:ModelID"];
        _classificationModelID = configuration["DocumentIntelligenceCustom:ClassificationModelID"];
        _client = new DocumentIntelligenceClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
        _docType = docType;
        Console.WriteLine("Extraction model: " + _modelID);
        Console.WriteLine("Classification model: " + _classificationModelID);
    }
    public async Task<documentData> DocumentDataExtraction(IFormFile dokument, CancellationToken cancellationToken = default)
    {
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
        return new documentData
        {
            DocumentType = documentType,
            DocumentNumber = GetField(document, "InvoiceId"),
            DocumentTime=GetField(document, "InvoiceDate"),
            CustomerName = GetField(document, "CustomerName"),
            CustomerAddress = GetField(document, "CustomerAddress"),
            CustomerVAT = GetField(document, "CustomerTaxId"),
            Currency = GetField(document, "Currency"),
            Osnovica = GetField(document, "SubTotal"),
            PDV = GetField(document, "TotalTax"),
            FullPrize = GetField(document, "InvoiceTotal"),
            Artikli = GetStavke(document)
        };
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

        memoryStream.Position = 0;


        var operation = await _client.ClassifyDocumentAsync(
            WaitUntil.Completed,
            _classificationModelID,
            BinaryData.FromStream(memoryStream));


        var json = operation.Value.ToString();


        using JsonDocument doc = JsonDocument.Parse(json);


        var document = doc.RootElement
            .GetProperty("documents")[0];


        return document
            .GetProperty("docType")
            .GetString();
    }
}