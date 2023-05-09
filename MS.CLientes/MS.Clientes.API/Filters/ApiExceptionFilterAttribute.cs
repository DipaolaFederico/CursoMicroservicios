using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MS.Clientes.Application.Common.Exceptions;
using MS.Clientes.Application.Common.Logging;

namespace MS.Clientes.API.Filters
{
    public sealed class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
        private readonly ILogger<ApiExceptionFilterAttribute> _logger;
        public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
        {
            _logger = logger;

            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
                { typeof(BadRequestException), HandleBadRequestException },
                { typeof(InternalServerErrorException), HandleInternalServerErrorException }
            };
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            LogResponse(context);

            base.OnException(context);
        }

        private void LogResponse(ExceptionContext context)
        {

            _logger.LogApiRequestResponse(LogLevel.Information,
                "Api Response:" + Environment.NewLine +
                "Status Code: {StatusCode}" + Environment.NewLine +
                "Headers: {@Headers}",
                context.HttpContext.Response.StatusCode,
                context.HttpContext.Response.Headers.ToDictionary(h => h.Key, h => h.Value)
                );

            if (context.Result != null)
                _logger.LogApiRequestResponse(LogLevel.Information, "Body: {@body}", context.Result);
        }

        private void HandleException(ExceptionContext context)
        {

            Type type = context.Exception.GetType();

            _logger.LogError(context.Exception, "Se detectó una excepción {Name}", type.Name);

            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }

            if (!context.ModelState.IsValid)
            {
                HandleInvalidModelStateException(context);
                return;
            }

            HandleInternalServerErrorException(context);
        }

        private void HandleValidationException(ExceptionContext context)
        {
            var exception = (ValidationException)context.Exception;

            var details = new ValidationProblemDetails(exception.Errors);

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleInvalidModelStateException(ExceptionContext context)
        {
            var details = new ValidationProblemDetails(context.ModelState);

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var exception = (NotFoundException)context.Exception;

            var details = new ProblemDetails()
            {
                Title = "No se encontró el recurso solicitado",
                Detail = exception.Message
            };

            context.Result = new NotFoundObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleBadRequestException(ExceptionContext context)
        {
            var exception = (BadRequestException)context.Exception;

            var details = new ProblemDetails()
            {
                Title = "Error en la solicitud",
                Detail = exception.Message
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleUnauthorizedAccessException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };

            context.ExceptionHandled = true;
        }

        private void HandleInternalServerErrorException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}
