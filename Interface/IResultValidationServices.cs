using DocumentData.Model;
using Microsoft.AspNetCore.Http;
using PDFFileReader.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ResultValidation.Services;

public interface IResultValidationServices
{
    bool Validation(documentData podaci);
}