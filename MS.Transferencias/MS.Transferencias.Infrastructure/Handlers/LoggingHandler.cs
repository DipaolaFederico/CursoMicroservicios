using Microsoft.Extensions.Logging;
using MS.Transferencias.Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Transferencias.Infrastructure.Handlers
{
    internal class LoggingHandler : DelegatingHandler
    {
        private readonly ILogger<LoggingHandler> _logger;

        public LoggingHandler(ILogger<LoggingHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestContent = string.Empty;
            if (request.Content != null)
                requestContent = await request.Content.ReadAsStringAsync().ConfigureAwait(false);

            _logger.LogExternal(
                LogLevel.Information,
                "Llamado externo http REST." + Environment.NewLine +
                "Metodo: {Metodo}. " + Environment.NewLine +
                "Url: {Url} " + Environment.NewLine +
                "Headers: {@Headers}",
                request.Method,
                request.RequestUri,
                request.Headers.ToDictionary(h => h.Key, h => h.Value)
                );
            if (!string.IsNullOrEmpty(requestContent))
                _logger.LogExternalBody(LogLevel.Information, "Body: {body}", requestContent);

            var response = await base.SendAsync(request, cancellationToken);

            var responseContent = string.Empty;
            if (response.Content != null)
                responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            _logger.LogExternal(LogLevel.Information, "Respuesta a llamado externo http REST." + Environment.NewLine +
                "Metodo: {Metodo}. " + Environment.NewLine +
                "Url: {Url} " + Environment.NewLine +
                "Status Code: {StatusCode}" + Environment.NewLine +
                "Headers: {@Headers}" + Environment.NewLine,
                request.Method,
                request.RequestUri,
                response.StatusCode,
                response.Headers.ToDictionary(h => h.Key, h => h.Value)
                );
            if (!string.IsNullOrEmpty(responseContent))
                _logger.LogExternalBody(LogLevel.Information, "Body: {body}", responseContent);

            return response;
        }
    }
}
