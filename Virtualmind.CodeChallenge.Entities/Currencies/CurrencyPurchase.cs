using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Virtualmind.CodeChallenge.Entities.Currencies
{
    public class CurrencyPurchase
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string ISOCode { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
