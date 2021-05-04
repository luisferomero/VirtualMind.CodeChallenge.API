using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Virtualmind.Codechallenge.Contracts.CurrenciesExternalServices
{
    public interface ICurrenciesExternalService
    {
        Task<JObject> GetCurrencyExchangeRateAsync(string url);
    }
}
