using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<CurrencyQuotation> GetCurrencyQuotationAsync(string ISOCode)
        {
            CurrencyApiSetting currencySetting = CurrenciesSettings.CurrenciesApiSettings
                .FirstOrDefault(c => c.ISOCode == ISOCode);

            JObject response = await _currenciesExternalService.GetCurrencyExchangeRateAsync(currencySetting.Url);

            CurrencyQuotation currencyQuote = new CurrencyQuotation()
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

        public async Task<ResponseHelper<CurrencyPurchase>> BuyCurrencyAsync(CurrencyPurchase purchase)
        {
            CurrencyQuotation currencyQuotation = await GetCurrencyQuotationAsync(purchase.ISOCode);

            purchase.Amount /= currencyQuotation.PurchaseRate;

            CurrencyApiSetting currencySetting = CurrenciesSettings.CurrenciesApiSettings
                .FirstOrDefault(c => c.ISOCode == purchase.ISOCode);

            decimal monthlyAmmout = GetUserMonthlyAmmount(purchase);

            List<string> errors = ValidatePurchase(purchase, currencySetting.Limit, monthlyAmmout);

            if (errors.Count > 0)
                return new ResponseHelper<CurrencyPurchase>(errors, statusCode: 400);
            else
            {
                purchase.DateTime = DateTime.Now;
                UnitOfWork.GenericRepository<CurrencyPurchase>().Add(purchase);
                await UnitOfWork.CompleteAsync();
                return new ResponseHelper<CurrencyPurchase>(purchase);
            }
        }

        private decimal GetUserMonthlyAmmount(CurrencyPurchase purchase)
        {
            return UnitOfWork.GenericRepository<CurrencyPurchase>()
                                .GetQueryable(null)
                                .Where(x => x.UserId == purchase.UserId && x.DateTime.Month == DateTime.Now.Month)
                                .Sum(x => x.Amount);
        }

        private List<string> ValidatePurchase(CurrencyPurchase purchase, decimal? limit, decimal monthlyAmmout)
        {
            List<string> errors = new List<string>();

            if (limit != null && (purchase.Amount > limit || purchase.Amount + monthlyAmmout > limit))
                errors.Add("This transaction exceeds de limint amomunt for the currency.");

            if (limit != null && monthlyAmmout > limit)
                errors.Add($"The monthly amount for currency {purchase.ISOCode} has been exceeded.");

            return errors;
        }
    }
}
