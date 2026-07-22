using DocumentData.Model;
using PDFFileReader.Models;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DocumentData.Sevices;

public interface IDocumentDataServices
{
    Task<documentData> DocumentDataExtraction(IFormFile dokument, CancellationToken cancellationToken = default);
}