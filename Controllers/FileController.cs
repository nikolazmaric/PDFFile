using Microsoft.AspNetCore.Mvc;
using PDFFileReader.Models;
using PDFFileReader.Services;
using System;
using System.IO;
using Ocr.Services;
using FileType.Services;
using UglyToad.PdfPig.Fonts;
using System.ComponentModel;
using Processing.Options;
using Microsoft.Extensions.Options;

namespace PDFFileReader.Controllers;


[ApiController]
[Route("api/[controller]")]
public class pdfController : ControllerBase
{
    private readonly processingOptions _processingOptions;
    public pdfController(IOptions<processingOptions> process)
    {
        _processingOptions = process.Value;
    }
    [HttpPost("upload")]
    public ActionResult<Dokument> UploadPdf(IFormFile dokument)
    {
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
            PDFServisi servisi = new PDFServisi();
            var response = servisi.AnalyzePdf(dokument);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}