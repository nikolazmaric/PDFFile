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
                "proposal",
                "packing"
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
        {

            documentType.CreditMemo, new List<string>
            {
                "dobropis",
                "credit",
                "note",
                "odobrenje",
                "memo",
                "adjustment",
                "ispravka",
                "popravek",
                "popravka",
                "extended"
            }
        }
    };
}