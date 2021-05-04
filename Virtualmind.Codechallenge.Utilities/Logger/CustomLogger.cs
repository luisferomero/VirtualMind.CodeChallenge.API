using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Text;

namespace Virtualmind.CodeChallenge.Utilities.Logger
{
    public static class CustomLogger
    {
        private static readonly ILogger _errorLogger;

        static CustomLogger()
        {
            //string connStr = ConfigurationManager.ConnectionStrings["CurrencyExchange"].ToString();
            string connStr = "Server =.; Database = CurrencyExchange; Trusted_Connection = True;";

            var sinkOpts = new MSSqlServerSinkOptions
            {
                TableName = "ErrorsLogs",
                AutoCreateSqlTable = true,
                BatchPostingLimit = 1
            };

            _errorLogger = new LoggerConfiguration()
                .WriteTo.MSSqlServer(
                    connectionString: connStr,
                    sinkOptions: sinkOpts)
                .CreateLogger();
        }

        public static void WriteError(CustomLogDetail logDetail)
        {
            if (logDetail.Exception != null)
                logDetail.Message = GetMessageFromException(logDetail.Exception);

            _errorLogger.Write(LogEventLevel.Information, "{@ErrorLogDetail}", logDetail);
        }
        private static string GetMessageFromException(Exception ex)
        {
            if (ex.InnerException != null)
                return GetMessageFromException(ex.InnerException);

            return ex.Message;
        }
    }
}
