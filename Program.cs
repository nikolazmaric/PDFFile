<<<<<<< HEAD
using PDFFileReader.Services;
using Ocr.Services;
using Microsoft.AspNetCore.StaticFiles;
using FileType.Services;

=======
>>>>>>> 2a63af4 (PDF)
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
<<<<<<< HEAD
builder.Services.AddScoped<PDFServisi>();
builder.Services.AddScoped<OcrServisi>();
builder.Services.AddScoped<VrstaFajla>();
=======
>>>>>>> 2a63af4 (PDF)

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();