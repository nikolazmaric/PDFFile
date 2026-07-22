using PDFFileReader.Services;
using Ocr.Services;
using Microsoft.AspNetCore.StaticFiles;
using FileType.Services;
using TextNormalization.Services;
using DocumentType.Services;
using System.Text.Json.Serialization;
using DocumentData.Sevices;
using Logger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<PDFServisi>();
builder.Services.AddScoped<OcrServisi>();
builder.Services.AddScoped<VrstaFajla>();
builder.Services.AddScoped<TextNormal>();
builder.Services.AddScoped<DocType>();
builder.Services.AddScoped<DocumentDataServices>();
builder.Services.AddScoped<ILoggerService, LoggerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();