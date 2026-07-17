using DocumentType.Enum;
using System.Collections.Generic;

namespace DocumentType.Keywords;

public static class DocumentKeywords
{
    public static readonly Dictionary<documentType, List<string>> Keywords = new()
    {
        {
            documentType.Invoice, new List<string>
            {
                "faktura",
                "racun",
                "račun",
                "invoice"
            }
        },
        {
            documentType.Offer, new List<string>
            {
                "ponuda",
                "offer",
                "proposal"
            }
        },
        {
            documentType.DeliveryNote, new List<string>
            {
                "otpremnica",
                "delivery",
                "slip"
            }
        },
        {
            documentType.Contract, new List<string>
            {
                "ugovor",
                "dogovor",
                "contract"
            }
        },
        {
            documentType.Order, new List<string>
            {
                "narudzbina",
                "porudzbina",
                "order"
            }
        },
    };
}