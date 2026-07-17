using Microsoft.AspNetCore.Mvc;
using PDFFileReader.Models;
using PDFFileReader.Services;
using System;
using System.IO;
using Ocr.Services;
using FileType.Services;
using UglyToad.PdfPig.Fonts;
using System.ComponentModel;

namespace PDFFileReader.Controllers;


[ApiController]
[Route("api/[controller]")]
public class pdfController : ControllerBase
{
    int MaxSize = 7 * 1024 * 1024;
    [HttpPost("upload")]
    public ActionResult<Dokument> UploadPdf(IFormFile dokument)
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