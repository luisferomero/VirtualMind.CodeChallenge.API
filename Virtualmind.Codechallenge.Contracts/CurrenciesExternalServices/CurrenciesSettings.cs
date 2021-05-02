using System;
using System.Collections.Generic;
using System.Text;

namespace Virtualmind.Codechallenge.Contracts.CurrenciesExternalServices
{
    public static class CurrenciesSettings
    {
        public static readonly List<CurrencyApiSetting> CurrenciesApiSettings = new List<CurrencyApiSetting>()
        {
            new CurrencyApiSetting()
            {
                ISOCode = "USD",
                Url = "https://www.bancoprovincia.com.ar/Principal/Dolar",
                PurchaseRateField = "[0]",
                SaleRateField = "[1]",
                LastUpdateField = "[2]"
            },
            new CurrencyApiSetting()
            {
                ISOCode ="BRL",
                Url = "https://www.bancoprovincia.com.ar/Principal/Dolar",
                PurchaseRateField = "[0]",
                SaleRateField = "[1]",
                LastUpdateField = "[2]",
                QuoteRate = decimal.Divide(1,4)
            },
        };
    }

    public class CurrencyApiSetting
    {
        public string ISOCode { get; set; }
        public string Url { get; set; }
        public string PurchaseRateField { get; set; }
        public string SaleRateField { get; set; }
        public string LastUpdateField { get; set; }
        public decimal QuoteRate { get; set; } = 1;
    }
}
