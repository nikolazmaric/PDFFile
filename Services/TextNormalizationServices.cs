using Microsoft.AspNetCore.Mvc;
using PDFFileReader.Models;
using System;
using System.Collections.Generic;
using System.IO;
using UglyToad.PdfPig;
using Azure.AI.DocumentIntelligence;
using System.Text;
using System.Text.RegularExpressions;


namespace TextNormalization.Services;

public class TextNormal
{
    public string TextNorm(string text)
    {
        text = Regex.Replace(text, @"\s+", " ");
        text = Regex.Replace(text, @"(\r?\n){2,}", Environment.NewLine);
        text = Regex.Replace(text, @"[^\p{L}\p{N}\s\.,;:!?()\-/]", "");
        text = Regex.Replace(text, @"-\r?\n", "");
        text = text.Replace("\r\n", " ");
        text = text.Replace("\n", " ");
        text = text.Trim();
        return text;
    }
}