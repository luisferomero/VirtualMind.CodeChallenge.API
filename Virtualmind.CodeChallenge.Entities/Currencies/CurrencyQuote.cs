using System;
using System.Collections.Generic;
using System.Text;

namespace Virtualmind.CodeChallenge.Entities.Currencies
{
    public class CurrencyQuote
    {
        public string ISOCode { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal SaleRate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
