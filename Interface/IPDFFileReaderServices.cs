using DocumentData.Model;
using Microsoft.AspNetCore.Http;
using PDFFileReader.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PDFFileReader.Services;

public interface IPDFFileReaderServices
{
    Dokument AnalyzePdf(IFormFile dokument);
}