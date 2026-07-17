using Azure.AI.DocumentIntelligence;
using DocumentType.Enum;
using DocumentType.Keywords;
using Microsoft.AspNetCore.Mvc;
using Ocr.Services;
using PDFFileReader.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using TextNormalization.Services;
using TextNormalization.Services;
using UglyToad.PdfPig;
using PDFFileReader.Models;

namespace DocumentType.Services;

public class DocType
{
    private readonly OcrServisi _ocrService;

    public DocType(OcrServisi ocrService)
    {
        _ocrService = ocrService;
    }
    public async Task<VrstaDokumenta> docType(IFormFile dokument, CancellationToken cancellationToken = default)
    {
        var stream = dokument.OpenReadStream();
        string text = await _ocrService.ExtractTextAsync(stream, cancellationToken);
        TextNormal temp = new TextNormal();
        text = temp.TextNorm(text);
        text = text.ToLower();
        Dictionary<documentType, int> scores = new();
        foreach (var document in DocumentKeywords.Keywords)
        {
            int score = 0;
            foreach (var keyword in document.Value)
            {
                if (text.Contains(keyword.ToLower()))
                    score++;
            }
            scores[document.Key] = score;
        }
        var result = scores.OrderByDescending(x => x.Value).First();
        var second = scores.OrderByDescending(x=> x.Value).Skip(1).First();
        if (result.Value == 0 || result.Value==second.Value)
        {
            var bag = new VrstaDokumenta
            {
                vrDoc = documentType.UnknownType,
                vjerovatnocaUspjeha = 0
            };
            return bag;
        }
        var response = new VrstaDokumenta
        {
            vrDoc = result.Key,
            vjerovatnocaUspjeha = (double)result.Value / (result.Value + second.Value)
        };
        return response;
    }
}