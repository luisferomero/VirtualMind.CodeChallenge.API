using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Virtualmind.CodeChallenge.Entities.Currencies;

namespace Virtualmind.CodeChallenge.DataAccess.Contexts
{
    public class CurrenciesDbContext : DbContext
    {
        public CurrenciesDbContext(DbContextOptions<CurrenciesDbContext> options) : base(options)
        {

        }

        public virtual DbSet<CurrencyPurchase> CurrenciesPurchases { get; set; }
    }
}
