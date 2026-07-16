using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace PDFFileReader.Models;

public class Stranica
{
    public int pageNumber {  get; set; }
    public int TextLength { get; set; }
    public string PageType {  get; set; } = string.Empty;
}

public class Dokument
{
    public string FileName { get; set; } = string.Empty;
    public double FileSize { get; set; }
    public string FileType { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    public int FilePages { get; set; }
    public string PDFType { get; set; } = string.Empty;
    public List<Stranica> Pages {get;set;}
}

public class TextExtract
{
    public int PageNumber { get; set; }
    public string Text { get; set; } = string.Empty;
}