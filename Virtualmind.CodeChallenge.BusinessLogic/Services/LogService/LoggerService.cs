using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Virtualmind.CodeChallenge.Utilities.Logger;

namespace Virtualmind.CodeChallenge.BusinessLogic.Services.LogService
{
    public class LoggerService : ILoggerService
    {
        public void PostExceptionAsync(CustomLogDetail logDetail)
        {
            CustomLogger.WriteError(logDetail);
        }
    }
}
