using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Virtualmind.Codechallenge.Contracts.CurrenciesExternalServices;
using Virtualmind.CodeChallenge.Entities.Currencies;
using Virtualmind.CodeChallenge.Entities.Responses;

namespace Virtualmind.CodeChallenge.BusinessLogic.Services.Currencies
{
    public interface ICurrenciesService
    {
        Task<CurrencyQuote> GetCurrencyQuoteAsync(string ISOCode);
        Task<ResponseHelper<CurrencyPurchase>> PurchaseCurrencyAsync(CurrencyPurchase purchase);
        Task<CurrencyPurchase> GetPurchaseAsync(int id);
        bool IsCurrencyAbaible(string ISOCode);
    }
}
