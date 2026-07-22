using DocumentData.Model;
using Microsoft.AspNetCore.Http;
using PDFFileReader.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Ocr.Services;

public interface IOcrServices
{
    Task<string> ExtractTextAsync(Stream pdfStream, CancellationToken cancellationToken = default);
}