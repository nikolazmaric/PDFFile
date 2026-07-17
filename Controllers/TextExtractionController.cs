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

namespace TextExtraction.Controllers;


[ApiController]
[Route("api/[controller]")]
public class textExtractionController : ControllerBase
{
    int MaxSize = 7 * 1024 * 1024;
    private readonly OcrServisi _ocrService;
    public textExtractionController(OcrServisi ocrService)
    {
        _ocrService = ocrService;
    }
    [HttpPost("extract-text")]
    public async Task<ActionResult<TextExtract>> ExtractPdf(IFormFile dokument, CancellationToken cancellationToken = default)
    {
        var param = dokument.OpenReadStream();
        var file = PdfDocument.Open(param);
        if (dokument == null || dokument.Length == 0)
            return BadRequest("Datoteka nije pronadjena.");
        string ext = Path.GetExtension(dokument.FileName);
        if (!string.Equals(ext, ".pdf", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Datoteka nije PDF.");
        if (dokument.Length > (7 * 1024 * 1024))
            return BadRequest("Datoteka je prevelika.");
        VrstaFajla provjera = new VrstaFajla();
        bool isPDF = provjera.FileType(dokument);
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

            string tekst = await _ocrService.ExtractTextAsync(stream, cancellationToken);
            TextExtract response = new TextExtract()
            {
                PageNumber = file.NumberOfPages,
                Text = tekst,
                OcrUsage = "OCR used."
            };
            return Ok(response);
        }
    }
}