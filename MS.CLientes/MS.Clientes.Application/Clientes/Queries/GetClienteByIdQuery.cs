using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MS.Clientes.Application.Common.Exceptions;
using MS.Clientes.Application.Common.Interfaces;
using MS.Clientes.Application.Common.Specifications;
using MS.Clientes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Clientes.Application.Clientes.Queries
{
    public record class GetClienteByIdQuery(long id) : IRequest<Cliente>;

    public sealed class GetClienteByIdQueryHandler : IRequestHandler<GetClienteByIdQuery, Cliente>
    {
        private readonly IRepository<Cliente> _repository;
        private readonly ILogger<GetClienteByIdQueryHandler> _logger;

        public GetClienteByIdQueryHandler(IRepository<Cliente> repository, ILogger<GetClienteByIdQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Cliente> Handle(GetClienteByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Iniciando consulta de cliente por id {id}", request.id);

                var cliente = await _repository.GetAsync(new ClientByIdSpecification(request.id), cancellationToken).ConfigureAwait(false);

                if (cliente == null)
                    throw new NotFoundException($"No se encontro un cliente con id {request.id}");

                _logger.LogInformation("Finalizando consulta de cliente. Clientes encontrado: {@Cliente}", cliente);

                return cliente;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public sealed class GetClienteByIdQueryValidator : AbstractValidator<GetClienteByIdQuery>
    {
        public GetClienteByIdQueryValidator()
        {
            RuleFor(x => x.id)
                .GreaterThan(0)
                .WithMessage("El id del cliente debe ser mayor a 0");
        }
    }
}
