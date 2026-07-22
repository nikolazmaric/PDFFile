using Azure;
using Azure.AI.DocumentIntelligence;
using Azure.Core;
using DocumentData.Model;
using DocumentType.Services;
using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace ResultValidation.Services;

public class resultValidation: IResultValidationServices
{
    public bool Validation(documentData podaci)
    {
        podaci.Osnovica=DataNormalization(podaci.Osnovica);
        podaci.PDV=DataNormalization(podaci.PDV);
        podaci.FullPrize=DataNormalization(podaci.FullPrize);
        if (!float.TryParse(podaci.Osnovica, out var testO)||!float.TryParse(podaci.PDV, out var testPDV)||!float.TryParse(podaci.FullPrize, out var testFUll))
            return false;
        else if (float.TryParse(podaci.Osnovica, out var test1) && float.TryParse(podaci.PDV, out var test2) && float.TryParse(podaci.FullPrize, out var test3))
        {
            float osn, pdv, ukup;
            osn = float.Parse(podaci.Osnovica);
            pdv = float.Parse(podaci.PDV);
            ukup = float.Parse(podaci.FullPrize);
            if (osn + pdv != ukup)
                return false;
        }
        return true;
    }
    public string DataNormalization (string data)
    {
        if (string.IsNullOrEmpty(data))
            return data;
        string result;
        result=data.TrimStart('-');
        result=data.TrimStart('€');
        return result;
    }
}