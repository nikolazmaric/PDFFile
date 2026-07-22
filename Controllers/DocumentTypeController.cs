using FileType.Services;
using Microsoft.AspNetCore.Mvc;
using Ocr.Services;
using PDFFileReader.Models;
using PDFFileReader.Services;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using DocumentType.Services;
using DocumentType.Keywords;
using DocumentType.Enum;

namespace DocumentType.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentTypeController : ControllerBase
{
    int MaxSize = 7 * 1024 * 1024;
    private readonly DocType _docType;
    public DocumentTypeController(DocType docType)
    {
        _docType = docType;
    }
    [HttpPost("document-type")]
    public async Task<ActionResult<string>> UploadPdf(IFormFile dokument, CancellationToken cancellationToken = default)
    {
        if (dokument == null || dokument.Length == 0)
            return BadRequest("Datoteka nije pronadjena.");
        string ext = Path.GetExtension(dokument.FileName);
        if (!string.Equals(ext, ".pdf", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Datoteka nije PDF.");
        if (dokument.Length > MaxSize)
            return BadRequest("Datoteka je prevelika.");
        try
        {
            VrstaDokumenta result = await _docType.docType(
                dokument,
                cancellationToken);


            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}