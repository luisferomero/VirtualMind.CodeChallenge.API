using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Virtualmind.CodeChallenge.BusinessLogic.Services.Logs;
using Virtualmind.CodeChallenge.Utilities.Logger;

namespace Virtualmind.CodeChallenge.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionController : ControllerBase
    {
        private ILoggerService _loggerService;

        public ExceptionController(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public ActionResult PostExceptionAsync()
        {
            IExceptionHandlerPathFeature exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            CustomLogDetail logDetail = new CustomLogDetail
            {
                Path = exceptionFeature.Path,
                Exception = exceptionFeature.Error,
                ExceptionTrace = exceptionFeature.Error.StackTrace,
                Product = "Currency Exchange"
            };

            _loggerService.PostExceptionAsync(logDetail);

            return StatusCode(StatusCodes.Status500InternalServerError, exceptionFeature.Error.Message);
        }
    }
}
