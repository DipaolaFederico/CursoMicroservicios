using Microsoft.Extensions.Logging;
using MS.Transferencias.Infrastructure.Logging;
using Serilog;
using Serilog.Configuration;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Transferencias.Infrastructure.Logging
{
    public static class LoggingExtensions
    {
        public static LoggerConfiguration WithMasking(this LoggerEnrichmentConfiguration loggerConfiguration)
        {
            return loggerConfiguration
               .With(new MaskingEnricher());
        }

        public static void LogExternal(this Microsoft.Extensions.Logging.ILogger logger, LogLevel logLevel, string message, params object[] args)
        {
            using (LogContext.PushProperty("External", true))
            {
                logger.Log(logLevel, message, args);
            }
        }

        public static void LogExternalBody(this Microsoft.Extensions.Logging.ILogger logger, LogLevel logLevel, string message, string stringBody)
        {
            using (LogContext.PushProperty("External", true))
            {
                MaskMessage(ref stringBody);
                logger.Log(logLevel, message, stringBody);
            }
        }

        private static void MaskMessage(ref string stringBody)
        {
            ReplaceText(ref stringBody, "Authorization:", ":", "\r\n");
            ReplaceText(ref stringBody, "X-IBM-Client-Secret:", ":", "\r\n");
            ReplaceText(ref stringBody, "environment_key=", "=", "\r\n");
            ReplaceText(ref stringBody, "password=", "=", "&");
            ReplaceText(ref stringBody, "\"number\":\"", ":\"", "\",");
            ReplaceText(ref stringBody, "\"access_token\":\"", ":\"", "\",");
            ReplaceText(ref stringBody, "\"refresh_token\":\"", ":\"", "\",");
        }

        private static void ReplaceText(ref string fullText, string searchPattern, string delimiterStart, string delimiterEnd)
        {
            if (fullText != null)
            {
                if (fullText.IndexOf(searchPattern) > 0)
                {
                    string pre = fullText.Substring(0, fullText.IndexOf(searchPattern));
                    string password = fullText.Substring(fullText.IndexOf(searchPattern));
                    string replaceText = password.Substring(0, password.IndexOf(delimiterStart) + 1);
                    replaceText += "*****";

                    string post = string.Empty;
                    if (password.IndexOf(delimiterEnd) > 0)
                    {
                        post = password.Substring(password.IndexOf(delimiterEnd));
                    }

                    fullText = $"{pre}{replaceText}{post}";
                }
            }
        }
    }
}
