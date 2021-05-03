using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Virtualmind.CodeChallenge.API.DTO;
using Virtualmind.CodeChallenge.Entities.Currencies;

namespace Virtualmind.CodeChallenge.API.Mappers
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<CurrencyPurchase, CurrencyPurchaseDTO>().ReverseMap();
        }
    }
}
