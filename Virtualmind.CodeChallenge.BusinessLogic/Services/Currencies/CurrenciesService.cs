using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Virtualmind.Codechallenge.Contracts.CurrenciesExternalServices;
using Virtualmind.CodeChallenge.Entities.Currencies;

namespace Virtualmind.CodeChallenge.BusinessLogic.Services.Currencies
{
    public class CurrenciesService : ICurrenciesService
    {
        public async Task<CurrencyQuote> GetCurrencyQuoteAsync(string ISOCode)
        {
            CurrencyApiSetting currencySetting = CurrenciesSettings.CurrenciesApiSettings
                .FirstOrDefault(c => c.ISOCode == ISOCode);

            JObject response = await GetQuoteAsync(currencySetting.Url);

            CurrencyQuote currencyQuote = new CurrencyQuote()
            {
                ISOCode = currencySetting.ISOCode,
                PurchaseRate = (decimal)response.SelectToken($"response{currencySetting.PurchaseRateField}"),
                SaleRate = (decimal)response.SelectToken($"response{currencySetting.SaleRateField}"),
                LastUpdate = DateTime.Now
            };

            currencyQuote.PurchaseRate *= currencySetting.QuoteRate;
            currencyQuote.SaleRate *= currencySetting.QuoteRate;

            return currencyQuote;
        }

        private async Task<JObject> GetQuoteAsync(string path)
        {
            using HttpClient httpClient = new HttpClient();
            using HttpResponseMessage response = await httpClient.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                return JObject.Parse($"{{'response':{apiResponse}}}");
            }
            else
                throw new Exception("An error occurred on the currency external service");
        }

        public bool IsCurrencyAbaible(string ISOCode)
        {
            return CurrenciesSettings.CurrenciesApiSettings.Any(c => c.ISOCode == ISOCode);
        }
    }
}
