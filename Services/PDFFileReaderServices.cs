using Microsoft.AspNetCore.Mvc;
using PDFFileReader.Models;
using System;
using System.Collections.Generic;
using System.IO;
using UglyToad.PdfPig;
using Azure.AI.DocumentIntelligence;

namespace PDFFileReader.Services;

public class PDFServisi
{
    public Dokument AnalyzePdf(IFormFile dokument)
    {
        try
        {
            List<Stranica> pages = new List<Stranica>();
            string vrsta = string.Empty;
            var stream = dokument.OpenReadStream();
            var file = PdfDocument.Open(stream);
            string ext = Path.GetExtension(dokument.FileName);
            for (int PageNumber = 1; PageNumber <= file.NumberOfPages; PageNumber++)
            {
                var page = file.GetPage(PageNumber);
                string text = page.Text;
                if (text.Length < 4)
                {
                    pages.Add(new Stranica
                    {
                        pageNumber = PageNumber,
                        TextLength = text.Length,
                        PageType = "Skenirana"
                    });
                }
                else if(text.Length < 30)
                {
                    pages.Add(new Stranica
                    {
                        pageNumber = PageNumber,
                        TextLength = text.Length,
                        PageType = "Mix"
                    });
                }
                else
                {
                    pages.Add(new Stranica
                    {
                        pageNumber = PageNumber,
                        TextLength = text.Length,
                        PageType = "Tekstualna"
                    });
                }
            }
                vrsta = "PDF je tekstualni.";
            var response = new Dokument
            {
                FileName = dokument.FileName,
                FileSize = dokument.Length,
                FileType = ext,
                ReceivedAt = DateTime.UtcNow,
                FilePages = file.NumberOfPages,
                PDFType = vrsta,
                Pages= pages
            };
            return response;
        }
        catch (Exception ex)
        {
            throw new Exception("Greska prilikom obrade PDF dokumenta:" + ex.Message);
        }
    }
    public TextExtract GetExtract(IFormFile dokument)
    {
        var stream = dokument.OpenReadStream();
        var file = PdfDocument.Open(stream);
        int pages = file.NumberOfPages;
        string tekst = string.Empty;
        for(int brojStrane = 1; brojStrane <= pages; brojStrane++)
        {
            var page = file.GetPage(brojStrane);
            string tekstStrane = page.Text;
            tekst += tekstStrane;
        }
        var response = new TextExtract
        {
            PageNumber = file.NumberOfPages,
            Text = tekst
        };
        return response;
    }
}