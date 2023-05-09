using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MS.Clientes.Application.Clientes.Queries;
using MS.Clientes.Application.Common.Interfaces;
using MS.Clientes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Clientes.Application.Clientes.Commads
{
    public record CreateClienteCommand(string Nombre, string Apellido, string Cuil, 
        string TipoDocumento, int NroDocumento, bool EsEmpleadoBNA, string PaisOrigen) : IRequest<long>;

    public sealed class CreateClienteCommandHandler : IRequestHandler<CreateClienteCommand, long>
    {
        private readonly IRepository<Cliente> _repository;
        private readonly ILogger<CreateClienteCommandHandler> _logger;

        public CreateClienteCommandHandler(IRepository<Cliente> repository, ILogger<CreateClienteCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<long> Handle(CreateClienteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Iniciando creación de cliente");

                var cliente = new Cliente
                {
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    Cuil = request.Cuil,
                    TipoDocumento = request.TipoDocumento,
                    EsEmpleadoBNA = request.EsEmpleadoBNA,
                    NroDocumento = request.NroDocumento,
                    PaisOrigen = request.PaisOrigen
                };

                await _repository.AddAsync(cliente, cancellationToken).ConfigureAwait(false);

                _logger.LogInformation("Finalizando creación de cliente {@Cliente}", cliente);

                return cliente.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al tratar de crear al cliente", ex);
                throw;
            }
        }
    }

    public class CreateCLienteCommandValidator : AbstractValidator<CreateClienteCommand>
    {
        public CreateCLienteCommandValidator()
        {
            RuleFor(x => x.Nombre)
                .NotNull()
                .NotEmpty()
                .WithMessage("El nombre no puede ser nulo o vacío");

            RuleFor(x => x.Apellido)
                .NotNull()
                .NotEmpty()
                .WithMessage("El apellido no puede ser nulo o vacío");

            RuleFor(x => x.TipoDocumento)
                .NotNull()
                .NotEmpty()
                .WithMessage("El TipoDocumento no puede ser nulo o vacío");

            RuleFor(x => x.PaisOrigen)
                .NotNull()
                .NotEmpty()
                .WithMessage("El Pais de Origen no puede ser nulo o vacío");

            RuleFor(x => x.Cuil)
                .NotNull()
                .NotEmpty()
                .Must(x => x.Length == 11)
                .WithMessage("El Cuil no puede ser nulo o vacío y debe tener una longitud de 11 digitos");

            RuleFor(x => x.NroDocumento)
                .Must(x => x.ToString().Length == 8)
                .WithMessage("El Numero de Documento debe tener una longitud de 8 digitos");
        }
    }
}
