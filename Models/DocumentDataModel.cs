
using DocumentType.Enum;
using System;
using System.Collections.Generic;

namespace DocumentData.Model;

public class documentData
{
    public string DocumentType { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string DocumentTime { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerAddress { get; set; } = string.Empty;
    public string CustomerVAT { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public string Osnovica {  get; set; }=string.Empty;
    public string PDV { get; set; } = string.Empty;
    public string FullPrize { get; set; }=string.Empty;
    public List<Stavke> Artikli { get; set; }
}

public class Stavke
{
    public string NazivStavke {  get; set; } = string.Empty;
    public string IdStavke {  set; get; } = string.Empty;
    public string Kolicina {  get; set; } = string.Empty;
    public string Cijena {  get; set; } = string.Empty;
    public string Ukupno {  get; set; } = string.Empty;
}