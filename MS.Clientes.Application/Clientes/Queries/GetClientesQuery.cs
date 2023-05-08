using MediatR;
using Microsoft.Extensions.Logging;
using MS.Clientes.Application.Common.Interfaces;
using MS.Clientes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Clientes.Application.Clientes.Queries
{
    public record GetClientesQuery : IRequest<IEnumerable<Cliente>>;

    public sealed class GetClientesQueryHandler : IRequestHandler<GetClientesQuery, IEnumerable<Cliente>>
    {
        private readonly IRepository<Cliente> _repository;
        private readonly ILogger<GetClientesQueryHandler> _logger;

        public GetClientesQueryHandler(IRepository<Cliente> repository, ILogger<GetClientesQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<Cliente>> Handle(GetClientesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Iniciando consulta de clientes");

                var clientes = await _repository.GetAllAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

                _logger.LogInformation("Finalizando consulta de clientes. Clientes encontrados: {@Clientes}", clientes);

                return clientes;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al consultar los clientes", ex);

                throw;
            }
        }
    }
}
