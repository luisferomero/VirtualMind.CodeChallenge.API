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
using Virtualmind.CodeChallenge.Repository.UnitOfWork;
using Virtualmind.CodeChallenge.Utilities.Helpers;

namespace Virtualmind.CodeChallenge.BusinessLogic.Services.Currencies
{
    public class CurrenciesService : ICurrenciesService
    {
        private readonly IUnitOfWorkService UnitOfWork;

        public CurrenciesService(CurrenciesDbContext dbContext)
        {
            UnitOfWork = new UnitOfWorkService(dbContext);
        }
        public async Task<CurrencyQuote> GetCurrencyQuoteAsync(string ISOCode)
        {
            CurrencyApiSetting currencySetting = CurrenciesSettings.CurrenciesApiSettings
                .FirstOrDefault(c => c.ISOCode == ISOCode);

            JObject response = await GetCurrencyExchangeRateAsync(currencySetting.Url);

            CurrencyQuote currencyQuote = new CurrencyQuote()
            {
                ISOCode = currencySetting.ISOCode,
                PurchaseRate = (decimal) response.SelectToken($"response{currencySetting.PurchaseRateField}") * currencySetting.QuoteRate,
                SaleRate = (decimal) response.SelectToken($"response{currencySetting.SaleRateField}") * currencySetting.QuoteRate,
                LastUpdate = DateTime.Now
            };
            return currencyQuote;
        }

        private async Task<JObject> GetCurrencyExchangeRateAsync(string url)
        {
            using HttpClient httpClient = new HttpClient();
            using HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JObject.Parse($"{{'response':{jsonResponse}}}");
            }
            else
                throw new Exception($"An error occurred on the currency external service ({response.StatusCode})");
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

            decimal monthlyAmmout = UnitOfWork.GenericRepository<CurrencyPurchase>()
                .GetQueryable(null)
                .Where(x => x.UserId == purchase.UserId && x.DateTime.Month == DateTime.Now.Month)
                .Sum(x => x.Amount);

            CurrencyQuote currencyQuote = await GetCurrencyQuoteAsync(purchase.ISOCode);
            purchase.Amount /= currencyQuote.PurchaseRate;

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
