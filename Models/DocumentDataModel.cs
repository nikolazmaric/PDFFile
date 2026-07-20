

using System;

namespace DocumentData.Model;

public class documentData
{
    // public string DocumentType { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
   // public DateTime DocumentTime { get; set; } = DateTime.UtcNow;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerAddress { get; set; } = string.Empty;
    public string CustomerVAT { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public string Osnovica {  get; set; }=string.Empty;
    public string PDV { get; set; } = string.Empty;
    public string FullPrize { get; set; }=string.Empty;
    //public double reliability { get; set; }
    //public string warning { get; set; }=string.Empty;
}