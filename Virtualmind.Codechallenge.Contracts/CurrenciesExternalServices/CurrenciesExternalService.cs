using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Virtualmind.Codechallenge.Contracts.CurrenciesExternalServices
{
    public class CurrenciesExternalService : ICurrenciesExternalService
    {
        public async Task<JObject> GetCurrencyExchangeRateAsync(string url)
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
    }
}
