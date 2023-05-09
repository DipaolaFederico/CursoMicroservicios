using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MS.Transferencias.Application.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Transferencias.Application.Common.PostProcessors
{
    public sealed class HttpLoggingPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
    {
        private readonly ILogger<HttpLoggingPostProcessor<TRequest, TResponse>> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HttpLoggingPostProcessor(ILogger<HttpLoggingPostProcessor<TRequest, TResponse>> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            _logger.LogApiRequestResponse(LogLevel.Information, "Response Headers: {@Headers}", _httpContextAccessor.HttpContext.Response.Headers.ToDictionary(h => h.Key, h => h.Value));
            _logger.LogApiRequestResponse(LogLevel.Information, "Response Body: {@Body}", response);

            return Task.CompletedTask;
        }
    }
}
