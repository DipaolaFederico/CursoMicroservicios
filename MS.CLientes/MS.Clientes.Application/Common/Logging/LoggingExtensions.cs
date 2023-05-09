using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Clientes.Application.Common.Logging
{
    public static class LoggingExtensions
    {
        public static void LogApiRequestResponse(this ILogger logger, LogLevel logLevel, string message, params object[] args)
        {
            using (LogContext.PushProperty("ApiRequestResponse", true))
            {
                logger.Log(logLevel, message, args);
            }
        }
    }
}
