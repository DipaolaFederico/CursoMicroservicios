using AutoMapper;
using MS.Clientes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MS.Clientes.Application.Common.Mapping
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<User, UserValidationResponse>().ReverseMap(); --> Common mapping example

            //CreateMap<Consent, GetConsentResponse>()
            //     .ForMember(x => x.Id, o => o.MapFrom(x => x.Id))
            //     .ForMember(x => x.Cuil, o => o.MapFrom(x => x.User.Cuil)); --> Custom mapping example

        }
    }
}
