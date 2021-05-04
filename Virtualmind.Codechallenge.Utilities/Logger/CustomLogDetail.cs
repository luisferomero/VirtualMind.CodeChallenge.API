using System;
using System.Collections.Generic;
using System.Text;

namespace Virtualmind.CodeChallenge.Utilities.Logger
{
    public class CustomLogDetail
    {
        public CustomLogDetail()
        {
            Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; private set; }
        public string Message { get; set; }

        // WHERE
        public string Product { get; set; }
        public string Path { get; set; }
        public string ExceptionTrace { get; set; }

        // EVERYTHING ELSE
        public Exception Exception { get; set; }  // the exception for error logging

    }
}
