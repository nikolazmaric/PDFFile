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


namespace DocumentData.Sevices;

public class DocumentDataServices
{
    private readonly DocumentIntelligenceClient _client;
    private readonly string _modelID;
    public DocumentDataServices(IConfiguration configuration)
    {
        var endpoint = configuration["DocumentIntelligenceCustom:Endpoint"];
        var apiKey = configuration["DocumentIntelligenceCustom:ApiKey"];
        _modelID = configuration["DocumentIntelligenceCustom:ModelID"];
        _client = new DocumentIntelligenceClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
    }
    public async Task<documentData> DocumentDataExtraction(IFormFile dokument, CancellationToken cancellationToken = default)
    {
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

        var document = result.Documents[0]; ;
        return new documentData
        {
            DocumentNumber = GetField(document, "InvoiceId"),
            CustomerName = GetField(document, "CustomerName"),
            CustomerAddress = GetField(document, "CustomerAddress"),
            CustomerVAT = GetField(document, "CustomerTaxId"),
            Currency = GetField(document, "Currency"),
            Osnovica = GetField(document, "SubTotal"),
            PDV = GetField(document, "TotalTax"),
            FullPrize = GetField(document, "InvoiceTotal"),

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
}