using Azure.AI.DocumentIntelligence;
using DocumentData.Model;
using DocumentData.Sevices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using Processing.Options;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace DocumentData.Controller;


[ApiController]
[Route("api/[controller]")]
public class DocumentDataController: ControllerBase
{
    private readonly DocumentDataServices _documentDataService;
    private readonly processingOptions _processingOptions;

    public DocumentDataController(
        DocumentDataServices documentDataService,
        IOptions<processingOptions> process)
    {
        _documentDataService = documentDataService;
        _processingOptions = process.Value;
    }

    [HttpPost("extract")]
    public async Task<ActionResult<documentData>> Extract(IFormFile dokument, CancellationToken cancellationToken= default)
    {
        Stopwatch stopwatch= Stopwatch.StartNew();
        if (dokument == null || dokument.Length == 0)
            return BadRequest("Datoteka nije pronadjena.");
        string ext = Path.GetExtension(dokument.FileName);
        if (!string.Equals(ext, ".pdf", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Datoteka nije PDF.");
        long maxSize = _processingOptions.MaxFileSizeMb * 1024L * 1024L;
        if (dokument.Length > maxSize)
            return BadRequest("Datoteka je prevelika.");
        try
        {
            var result = await _documentDataService.DocumentDataExtraction(dokument, cancellationToken);
            stopwatch.Stop();
            Console.WriteLine($"Vrijeme obrade dokumenta: {stopwatch.Elapsed.TotalSeconds:F2} sekundi");

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}