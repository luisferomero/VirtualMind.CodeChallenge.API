using System;
using System.Collections.Generic;
using System.Text;
using Virtualmind.CodeChallenge.Utilities.Logger;

namespace Virtualmind.CodeChallenge.BusinessLogic.Services.LogService
{
    public interface ILoggerService
    {
        void PostExceptionAsync(CustomLogDetail logDetail);
    }
}
