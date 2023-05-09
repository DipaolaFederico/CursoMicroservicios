using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MS.Transferencias.Application.Common.Dtos;
using MS.Transferencias.Application.Common.Exceptions;
using MS.Transferencias.Application.Common.Interfaces;
using MS.Transferencias.Application.Common.Specifications;
using MS.Transferencias.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Transferencias.Application.Transferencias.Queries
{
    public sealed record GetTransferenciaByCbuQuery(string CBU) : IRequest<IEnumerable<TransferenciaDto>>;

    public sealed class GetTransferenciaByCbuQueryHandler : IRequestHandler<GetTransferenciaByCbuQuery, IEnumerable<TransferenciaDto>>
    {
        private readonly IRepository<Transferencia> _repository;
        private readonly ILogger<GetTransferenciaByCbuQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetTransferenciaByCbuQueryHandler(IRepository<Transferencia> repository, ILogger<GetTransferenciaByCbuQueryHandler> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TransferenciaDto>> Handle(GetTransferenciaByCbuQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Iniciando consulta de transferencias para el cbu {Cbu}", request.CBU);

                var transferencias = await _repository.GetAllAsync(new TransferenciaByCbuSpecification(request.CBU), cancellationToken).ConfigureAwait(false);

                if (transferencias == null)
                    throw new NotFoundException($"Transferencias no encontradas para el cbu {request.CBU}");

                var transferenciasDto = _mapper.Map<IEnumerable<TransferenciaDto>>(transferencias);

                _logger.LogInformation("Finalizando consulta de transferencias para el cbu {CBU}", request.CBU);

                return transferenciasDto;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public sealed class GetTransferenciaByCbuQueryValidator : AbstractValidator<GetTransferenciaByCbuQuery>
    {
        public GetTransferenciaByCbuQueryValidator()
        {
            RuleFor(x => x.CBU)
                .NotNull()
                .NotEmpty()
                .WithMessage("El Cbu debe tener 22 digitos");
        }
    }
}
