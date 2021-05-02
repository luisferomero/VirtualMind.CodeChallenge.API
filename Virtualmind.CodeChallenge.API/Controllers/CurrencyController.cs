using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Virtualmind.CodeChallenge.BusinessLogic.Services.Currencies;
using Virtualmind.CodeChallenge.Entities.Currencies;

namespace Virtualmind.CodeChallenge.API.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        public ICurrenciesService _curreciesService { get; set; }
        public CurrencyController(ICurrenciesService curreciesService)
        {
            _curreciesService = curreciesService;
        }

        /// <summary>
        /// Get the currency quote of the day.
        /// </summary>
        /// <param name="code">ISO code of the currency</param>
        /// <returns>currency quote of the day</returns>
        /// <response code="404">Not Found - The currency you are trying to find is not in the currency list or is not available at the moment.</response>
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrencyQuote))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> GetCurrencyQuote(string code)
        {
            if (!_curreciesService.IsCurrencyAbaible(code.ToUpper()))
                return NotFound($@"The currency ""{code}"" is not available at this moment.");

            return Ok(await _curreciesService.GetCurrencyQuoteAsync(code.ToUpper()));
        }
    }
}
