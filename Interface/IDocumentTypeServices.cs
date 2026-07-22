using DocumentData.Model;
using PDFFileReader.Models;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DocumentType.Services;

public interface IDocTypeServices
{
    Task<VrstaDokumenta> docType(IFormFile dokument, CancellationToken cancellationToken = default);
}