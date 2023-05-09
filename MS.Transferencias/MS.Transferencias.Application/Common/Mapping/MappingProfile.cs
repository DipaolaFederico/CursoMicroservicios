using AutoMapper;
using MS.Transferencias.Application.Common.Dtos;
using MS.Transferencias.Application.Transferencias.Commands;
using MS.Transferencias.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MS.Transferencias.Application.Common.Mapping
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateTransferenciaCommand, Transferencia>();
            CreateMap<Transferencia, TransferenciaDto>();
        }
    }
}
