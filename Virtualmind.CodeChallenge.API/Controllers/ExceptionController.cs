using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Virtualmind.CodeChallenge.Utilities.Helpers;

namespace Virtualmind.CodeChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ExceptionController : ControllerBase
    {
        private readonly ILogger<ExceptionController> _logger;

        public ExceptionController(ILogger<ExceptionController> logger)
        {
            _logger = logger;
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public ActionResult PostExceptionAsync()
        {
            IExceptionHandlerPathFeature exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            //_logger.LogWarning("THIS IS A CUSTOM MESSAGE FROM EXEPTION CONTROLLER");
            //_logger.LogError("THIS IS A CUSTOM MESSAGE FROM EXEPTION CONTROLLER");

            string message = LoggerHelper.GetMessageFromException(exceptionFeature.Error);

            _logger.LogError(exceptionFeature.Error, message);

            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}
