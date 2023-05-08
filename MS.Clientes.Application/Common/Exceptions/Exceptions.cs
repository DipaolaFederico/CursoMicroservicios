using FluentValidation.Results;
using MS.Clientes.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Clientes.Application.Common.Exceptions
{
    public sealed class BadRequestException : CustomException
    {
        public BadRequestException(string message) : base(message) { }
    }

    public sealed class NotFoundException : CustomException
    {
        public NotFoundException(string message) : base(message) { }
    }

    public sealed class InternalServerErrorException : CustomException
    {
        public InternalServerErrorException(string message) : base(message) { }

        public InternalServerErrorException(string message, Exception ex) : base(message, ex) { }
    }

    public sealed class ValidationException : CustomException
    {
        public ValidationException() : base("Han ocurrido uno o más errores de validación")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }
        public IDictionary<string, string[]> Errors { get; }
    }
}
