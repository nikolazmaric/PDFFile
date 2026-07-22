using DocumentData.Model;
using Microsoft.AspNetCore.Http;
using PDFFileReader.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TextNormalization.Services;

public interface ITextNormalizationServices
{
    string TextNorm(string text);
}