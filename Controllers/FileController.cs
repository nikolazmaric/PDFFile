<<<<<<< HEAD
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
    private readonly OcrServisi _ocrService;
    public pdfController(OcrServisi ocrService)
    {
        _ocrService = ocrService;
    }

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
    [HttpPost("extract-text")]
    public async Task< ActionResult<TextExtract>> ExtractPdf(IFormFile dokument, CancellationToken cancellationToken=default)
    {
        if (dokument == null || dokument.Length == 0)
            return BadRequest("Datoteka nije pronadjena.");
        string ext = Path.GetExtension(dokument.FileName);
        if (!string.Equals(ext, ".pdf", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Datoteka nije PDF.");
        if (dokument.Length > (7 * 1024 * 1024))
            return BadRequest("Datoteka je prevelika.");
        VrstaFajla provjera = new VrstaFajla();
        bool isPDF=provjera.FileType(dokument);
        if (isPDF)
        {
            try
            {
                PDFServisi servisi = new PDFServisi();
                var response = servisi.GetExtract(dokument);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        else
        {
            using var stream = dokument.OpenReadStream();

            string response = await _ocrService.ExtractTextAsync(stream, cancellationToken);
            return Ok(response);
        }
=======
using PDFFileReader.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace PDFFileReader.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PodaciController : ControllerBase
{
    [HttpPost("upload")]
    public ActionResult<Dokument> UploadPdf(IFormFile dokument)
    {
        var ext = Path.GetExtension(dokument.FileName);
        if (dokument == null || dokument.Length == 0)
            return BadRequest("Datoteka nije pronadjena.");

        if (dokument.ContentType != "application/pdf")
          return BadRequest("Datoteka nije PDF.");
        if (!string.Equals(ext,".pdf",StringComparison.OrdinalIgnoreCase))
            return BadRequest("Datoteka nije PDF.");
        var response = new Dokument
        {
            FileName = dokument.FileName,
            FileSize = dokument.Length,
            FileType = ext,
            ReceivedAt = DateTime.UtcNow,
        };

        return Ok(response);
>>>>>>> 2a63af4 (PDF)
    }
}