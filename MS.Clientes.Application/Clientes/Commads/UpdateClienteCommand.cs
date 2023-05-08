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

namespace MS.Clientes.Application.Clientes.Commads
{
    public record UpdateClienteCommand(long Id, string Nombre, string Apellido, string Cuil,
        string TipoDocumento, int NroDocumento, bool EsEmpleadoBNA, string PaisOrigen) : IRequest<Cliente>;

    public sealed class UpdateClienteCommandHandler : IRequestHandler<UpdateClienteCommand, Cliente>
    {
        private readonly IRepository<Cliente> _repository;
        private readonly ILogger<UpdateClienteCommandHandler> _logger;

        public UpdateClienteCommandHandler(IRepository<Cliente> repository, ILogger<UpdateClienteCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Cliente> Handle(UpdateClienteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Iniciando modificación del cliente id {Id}", request.Id);

                var cliente = await _repository.GetAsync(new ClientByIdSpecification(request.Id), cancellationToken).ConfigureAwait(false);

                if (cliente == null)
                    throw new BadRequestException("No existe un cliente para el id proporcionado");

                cliente.Nombre = request.Nombre;
                cliente.Apellido = request.Apellido;
                cliente.TipoDocumento = request.TipoDocumento;
                cliente.NroDocumento = request.NroDocumento;
                cliente.PaisOrigen = request.PaisOrigen;
                cliente.Cuil = request.Cuil;
                cliente.EsEmpleadoBNA = request.EsEmpleadoBNA;

                await _repository.UpdateAsync(cliente, cancellationToken).ConfigureAwait(false);

                _logger.LogInformation("Finalizando correctamente la modificación del cliente {@Cliente}", cliente);

                return cliente;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public sealed class UpdateClienteCommandValidator : AbstractValidator<UpdateClienteCommand>
    {
        public UpdateClienteCommandValidator()
        {
            RuleFor(x => x.Id)
               .GreaterThan(0)
               .WithMessage("El id del cliente debe ser mayor a 0");

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
