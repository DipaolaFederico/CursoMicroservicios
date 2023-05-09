using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MS.Transferencias.Application.Common.Dtos;
using MS.Transferencias.Application.Common.Exceptions;
using MS.Transferencias.Application.Common.Interfaces;
using MS.Transferencias.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Transferencias.Application.Transferencias.Commands
{
    public sealed record CreateTransferenciaCommand(string CuilOriginante, string CuilDestinatario,
        string CbuOrigen, string CbuDestino, double Importe, string Concepto, string Descripcion) : IRequest<TransferenciaDto>;


    public sealed class CreateTransferenciaCommandHandler : IRequestHandler<CreateTransferenciaCommand, TransferenciaDto>
    {
        private readonly IRepository<Transferencia> _repository;
        private readonly ILogger<CreateTransferenciaCommandHandler> _logger;
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        public CreateTransferenciaCommandHandler(IRepository<Transferencia> repository, ILogger<CreateTransferenciaCommandHandler> logger, IClientService clientService, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _clientService = clientService;
            _mapper = mapper;
        }

        public async Task<TransferenciaDto> Handle(CreateTransferenciaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Iniciando proceso de creación de transferencia");

                var clients = await _clientService.GetClients().ConfigureAwait(false);

                if (clients == null || !clients.Any())
                    throw new BadRequestException("No se encontraron clientes para los cuils proporcionados");

                var originante = clients.FirstOrDefault(x => x.Cuil == request.CuilOriginante);

                if (originante == null)
                    throw new BadRequestException("No se encontraron clientes para el cuilOriginante proporcionado");

                var destinatario = clients.FirstOrDefault(x => x.Cuil == request.CuilDestinatario);

                if (destinatario == null)
                    throw new BadRequestException("No se encontraron clientes para el cuilDestinatario proporcionado");

                var transferencia = _mapper.Map<Transferencia>(request);

                await _repository.AddAsync(transferencia, cancellationToken).ConfigureAwait(false);

                _logger.LogInformation("Finalizando creación de transferencia");

                var transferenciaDto = _mapper.Map<TransferenciaDto>(transferencia);

                return transferenciaDto;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public sealed class CreateTransferenciaCommandValidator : AbstractValidator<CreateTransferenciaCommand>
    {
        public CreateTransferenciaCommandValidator()
        {
            RuleFor(x => x.CuilOriginante)
                .NotNull()
                .NotEmpty()
                .Must(x => x.Length == 11)
                .WithMessage("El cuil originante debe tener a menos 11 digitos");

            RuleFor(x => x.CuilDestinatario)
                .NotNull()
                .NotEmpty()
                .Must(x => x.Length == 11)
                .WithMessage("El cuil destinatario debe tener a menos 11 digitos");

            RuleFor(x => x.CbuOrigen)
                .NotNull()
                .NotEmpty()
                .Must(x => x.Length == 22)
                .WithMessage("El cbu origen debe tener a menos 22 digitos");

            RuleFor(x => x.CbuDestino)
                .NotNull()
                .NotEmpty()
                .Must(x => x.Length == 22)
                .WithMessage("El cbu destino debe tener a menos 22 digitos");

            RuleFor(x => x.Concepto)
                .NotNull()
                .NotEmpty()
                .WithMessage("El concepto no puede ser nulo o vacío");

            RuleFor(x => x.Importe)
                .GreaterThan(0)
                .WithMessage("El importe debe ser mayor a 0");
        }
    }
}
