using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Virtualmind.CodeChallenge.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionController : ControllerBase
    {
        //private IAuditService _auditService;

        //public ExceptionController(IAuditService auditService)
        //{
        //    _auditService = auditService;
        //}

        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PostExceptionAsync()
        {
            IExceptionHandlerPathFeature exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            string routeWhereExceptionOccurred = exceptionFeature.Path;

            //await _auditService.PostExceptionAsync(
            //    new ExceptionsHistory
            //    {
            //        CreationDate = DateTime.Now,
            //        CreationUserId = User.Claims.Count() == 0 ? (int?)null : int.Parse(User.Claims.Where(A => A.Type == "UserId").Single().Value),
            //        ExceptionTrace = exceptionFeature.Error.StackTrace,
            //        InnerException = exceptionFeature.Error.InnerException?.Message,
            //        Message = exceptionFeature.Error.Message,
            //        Path = exceptionFeature.Path
            //    });

            return StatusCode(StatusCodes.Status500InternalServerError, exceptionFeature.Error.Message);
        }
    }
}
