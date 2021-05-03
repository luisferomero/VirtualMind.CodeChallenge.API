using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Virtualmind.CodeChallenge.API.DTO;
using Virtualmind.CodeChallenge.BusinessLogic.Services.Currencies;
using Virtualmind.CodeChallenge.Entities.Currencies;
using Virtualmind.CodeChallenge.Utilities.Helpers;

namespace Virtualmind.CodeChallenge.API.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrenciesService _curreciesService;
        private readonly IMapper _mapper;
        public CurrencyController(ICurrenciesService curreciesService, IMapper mapper)
        {
            _curreciesService = curreciesService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the currency quote of the day.
        /// </summary>
        /// <param name="code">ISO code of the currency.</param>
        /// <returns>currency quote of the day.</returns>
        /// <response code="404">The currency you are trying to find is not in the currency list or is not available at the moment.</response>
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrencyQuote))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CurrencyQuote>> GetCurrencyQuote(string code)
        {
            code = code.ToUpper().Trim();

            if (!_curreciesService.IsCurrencyAbaible(code))
                return NotFound($@"The currency ""{code}"" is not available at this moment.");

            return Ok(await _curreciesService.GetCurrencyQuoteAsync(code));
        }

        /// <summary>
        /// Get a specific currency purchase record.
        /// </summary>
        /// <param name="id">Purchase Id.</param>
        /// <returns>Currency Purchase with the ID.</returns>
        [HttpGet("purchases/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrencyPurchaseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CurrencyPurchaseDTO>> GetPurchase(int id)
        {
            CurrencyPurchase purchase = await _curreciesService.GetPurchaseAsync(id);

            if (purchase == null)
                return NotFound();

            return Ok(_mapper.Map<CurrencyPurchaseDTO>(purchase));
        }

        /// <summary>
        /// Get the currency quote of the day.
        /// </summary>
        /// <param name="code">ISO code of the currency.</param>
        /// <param name="purchaseDTO"></param>
        /// <returns>currency quote of the day.</returns>
        /// <response code="404">The currency you are trying to find is not in the currency list or is not available at the moment.</response>
        [HttpPost("{code}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PurchaseCurrency(string code, CurrencyPurchaseDTO purchaseDTO)
        {
            code = code.ToUpper().Trim();

            if (!_curreciesService.IsCurrencyAbaible(code))
                return NotFound($@"The currency ""{code}"" is not available at this moment.");

            ResponseHelper<CurrencyPurchase> response = await _curreciesService.PurchaseCurrencyAsync(_mapper.Map<CurrencyPurchase>(purchaseDTO));

            if (!response.Success)
            {
                foreach (var error in response.Errors)
                    ModelState.AddModelError("Messages", error);

                return BadRequest(ModelState);
            }

            CurrencyPurchaseDTO DTO = _mapper.Map<CurrencyPurchaseDTO>(response.Entity);

            return CreatedAtAction(nameof(GetPurchase),new {id = DTO.Id }, DTO);
        }
    }
}
