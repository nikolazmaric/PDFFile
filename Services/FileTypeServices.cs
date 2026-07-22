using Microsoft.AspNetCore.Mvc;
using PDFFileReader.Models;
using System;
using System.Collections.Generic;
using System.IO;
using UglyToad.PdfPig;
using Azure.AI.DocumentIntelligence;

namespace FileType.Services;

public class VrstaFajla:IFileTypeServices
{
    public bool FileType(IFormFile dokument)
    {
        bool isPDF = true;
        var stream = dokument.OpenReadStream();
        var file = PdfDocument.Open(stream);
        for(int pages = 1; pages <= file.NumberOfPages; pages++)
        {
            var page = file.GetPage(pages);
            string text = page.Text;
            if(text.Length<30)
                isPDF= false;
        }
        return isPDF;
    }
}