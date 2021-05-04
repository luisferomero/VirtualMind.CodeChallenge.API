using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Virtualmind.Codechallenge.Contracts.CurrenciesExternalServices;
using Virtualmind.CodeChallenge.DataAccess.Contexts;
using Virtualmind.CodeChallenge.Entities.Currencies;
using Virtualmind.CodeChallenge.Entities.Responses;
using Virtualmind.CodeChallenge.Repository.UnitOfWork;

namespace Virtualmind.CodeChallenge.BusinessLogic.Services.Currencies
{
    public class CurrenciesService : ICurrenciesService
    {
        private readonly IUnitOfWorkService UnitOfWork;
        private readonly ICurrenciesExternalService _currenciesExternalService;

        public CurrenciesService(CurrenciesDbContext dbContext, ICurrenciesExternalService currenciesExternalService)
        {
            UnitOfWork = new UnitOfWorkService(dbContext);
            _currenciesExternalService = currenciesExternalService;
        }
        public async Task<CurrencyQuote> GetCurrencyQuoteAsync(string ISOCode)
        {
            CurrencyApiSetting currencySetting = CurrenciesSettings.CurrenciesApiSettings
                .FirstOrDefault(c => c.ISOCode == ISOCode);

            JObject response = await _currenciesExternalService.GetCurrencyExchangeRateAsync(currencySetting.Url);

            CurrencyQuote currencyQuote = new CurrencyQuote()
            {
                ISOCode = currencySetting.ISOCode,
                PurchaseRate = (decimal) response.SelectToken($"response{currencySetting.PurchaseRateField}") * currencySetting.QuoteRate,
                SaleRate = (decimal) response.SelectToken($"response{currencySetting.SaleRateField}") * currencySetting.QuoteRate,
                LastUpdate = DateTime.Now
            };
            return currencyQuote;
        }

        public bool IsCurrencyAbaible(string ISOCode)
        {
            return CurrenciesSettings.CurrenciesApiSettings.Any(c => c.ISOCode == ISOCode);
        }

        public async Task<CurrencyPurchase> GetPurchaseAsync(int id)
        {
            return await UnitOfWork.GenericRepository<CurrencyPurchase>().GetByAsync(x => x.Id == id, null);
        }

        public async Task<ResponseHelper<CurrencyPurchase>> PurchaseCurrencyAsync(CurrencyPurchase purchase)
        {
            CurrencyApiSetting currencySetting = CurrenciesSettings.CurrenciesApiSettings
                .FirstOrDefault(c => c.ISOCode == purchase.ISOCode);

            CurrencyQuote currencyQuote = await GetCurrencyQuoteAsync(purchase.ISOCode);

            purchase.Amount /= currencyQuote.PurchaseRate;

            decimal monthlyAmmout = UnitOfWork.GenericRepository<CurrencyPurchase>()
                .GetQueryable(null)
                .Where(x => x.UserId == purchase.UserId && x.DateTime.Month == DateTime.Now.Month)
                .Sum(x => x.Amount);

            if (currencySetting.Limit != null && purchase.Amount > currencySetting.Limit)
            {
                List<string> errors = new List<string>
                {
                    $"This transaction exceeds de limint amomunt for the currency."
                };
                return new ResponseHelper<CurrencyPurchase>(errors, statusCode: 400);
            }
            if (currencySetting.Limit != null && monthlyAmmout > currencySetting.Limit)
            {
                List<string> errors = new List<string>
                {
                    $"The monthly amount for currency {purchase.ISOCode} has been exceeded."
                };
                return new ResponseHelper<CurrencyPurchase>(errors, statusCode:400);
            }
            else
            {
                purchase.DateTime = DateTime.Now;
                UnitOfWork.GenericRepository<CurrencyPurchase>().Add(purchase);
                await UnitOfWork.CompleteAsync();
                return new ResponseHelper<CurrencyPurchase>(purchase);
            }
        }
    }
}
