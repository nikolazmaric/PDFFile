<<<<<<< HEAD
using System;
using System.Collections.Generic;
=======
>>>>>>> 2a63af4 (PDF)
using System.Security.Cryptography.X509Certificates;

namespace PDFFileReader.Models;

<<<<<<< HEAD
public class Stranica
{
    public int pageNumber {  get; set; }
    public int TextLength { get; set; }
    public string PageType {  get; set; } = string.Empty;
}

=======
>>>>>>> 2a63af4 (PDF)
public class Dokument
{
    public string FileName { get; set; } = string.Empty;
    public double FileSize { get; set; }
    public string FileType { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
<<<<<<< HEAD
    public int FilePages { get; set; }
    public string PDFType { get; set; } = string.Empty;
    public List<Stranica> Pages {get;set;}
}

public class TextExtract
{
    public int PageNumber { get; set; }
    public string Text { get; set; } = string.Empty;
=======
>>>>>>> 2a63af4 (PDF)
}