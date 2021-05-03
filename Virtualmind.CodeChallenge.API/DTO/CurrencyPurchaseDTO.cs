using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Virtualmind.CodeChallenge.API.DTO
{
    public class CurrencyPurchaseDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string ISOCode { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public DateTime DateTime { get; set; }
    }
}
