using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MS.Clientes.Application.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Clientes.Application.Common.Behaviours
{
    public sealed class HttpLoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : notnull, IRequest<TResponse>
    {
        private readonly ILogger<HttpLoggingBehaviour<TRequest, TResponse>> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpLoggingBehaviour(ILogger<HttpLoggingBehaviour<TRequest, TResponse>> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogApiRequestResponse(LogLevel.Information, "Request Headers: {@Headers}", _httpContextAccessor.HttpContext.Request.Headers.ToDictionary(h => h.Key, h => h.Value));
            _logger.LogApiRequestResponse(LogLevel.Information, "Request Body: {@Body}", request);
            return await next();
        }
    }
}
