using DocumentData.Model;
using PDFFileReader.Models;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FileType.Services;

public interface IFileTypeServices
{
    bool FileType(IFormFile dokument);
}