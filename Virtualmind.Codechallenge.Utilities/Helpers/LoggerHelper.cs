using System;

namespace Virtualmind.CodeChallenge.Utilities.Helpers
{
    public static class LoggerHelper
    {
        public static string GetMessageFromException(Exception ex)
        {
            if (ex.InnerException != null)
                return GetMessageFromException(ex.InnerException);

            return ex.Message;
        }
    }
}
