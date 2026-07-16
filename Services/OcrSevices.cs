using Azure.AI.DocumentIntelligence;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Azure;
using System.Threading;

namespace Ocr.Services;

public class OcrServisi
{
    private readonly DocumentIntelligenceClient _client;
    private readonly string _endpoint;
    private readonly string _apiKey;
    public OcrServisi(IConfiguration configuration)
    {
        _endpoint = configuration["DocumentIntelligence:Endpoint"];
        _apiKey = configuration["DocumentIntelligence:ApiKey"];
        _client = new DocumentIntelligenceClient(new Uri(_endpoint), new AzureKeyCredential(_apiKey));
    }
    public async Task<string> ExtractTextAsync(Stream pdfStream, CancellationToken cancellationToken = default)
    {
        var operation = await _client.AnalyzeDocumentAsync(
            WaitUntil.Completed,
            "prebuilt-read",
            BinaryData.FromStream(pdfStream), cancellationToken);

        var result = operation.Value;

        string text = "";

        foreach (var page in result.Pages)
        {
            foreach (var line in page.Lines)
            {
                text += line.Content + "\n";
            }
        }

        return text;
    }
}