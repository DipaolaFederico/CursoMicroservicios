using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Transferencias.Infrastructure.Logging
{
    public class SerilogOptions
    {
        public string LogLevel { get; init; } = string.Empty;
        public string FilePath { get; init; } = string.Empty;
        public string FilePathExternal { get; init; } = string.Empty;
        public string FilePathApiRequestResponse { get; init; } = string.Empty;
        public string OutputTemplate { get; init; } = string.Empty;

        public Serilog.Events.LogEventLevel GetLogLevel()
        {
            switch (LogLevel)
            {
                case "Debug":
                    return Serilog.Events.LogEventLevel.Debug;
                case "Information":
                    return Serilog.Events.LogEventLevel.Information;
                case "Verbose":
                    return Serilog.Events.LogEventLevel.Verbose;
                case "Warning":
                    return Serilog.Events.LogEventLevel.Warning;
                case "Fatal":
                    return Serilog.Events.LogEventLevel.Fatal;
                case "Error":
                    return Serilog.Events.LogEventLevel.Error;
                default:
                    return Serilog.Events.LogEventLevel.Information;
            }
        }
    }
}
