using Azure.AI.DocumentIntelligence;
using DocumentData.Model;
using DocumentData.Sevices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace DocumentData.Controller;


[ApiController]
[Route("api/[controller]")]
public class DocumentDataController: ControllerBase
{
    int MaxSize = 7 * 1024 * 1024;
    private readonly DocumentDataServices _documentDataService;


    public DocumentDataController(
        DocumentDataServices documentDataService)
    {
        _documentDataService = documentDataService;
    }

    [HttpPost("extract")]
    public async Task<ActionResult<documentData>> Extract(IFormFile dokument, CancellationToken cancellationToken= default)
    {
        if (dokument == null || dokument.Length == 0)
            return BadRequest("Datoteka nije pronadjena.");
        string ext = Path.GetExtension(dokument.FileName);
        if (!string.Equals(ext, ".pdf", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Datoteka nije PDF.");
        if (dokument.Length > (7 * 1024 * 1024))
            return BadRequest("Datoteka je prevelika.");
        var result = await _documentDataService.DocumentDataExtraction(dokument, cancellationToken);

        return Ok(result);
    }
}