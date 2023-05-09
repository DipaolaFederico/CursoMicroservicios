using MS.Clientes.Application.Common.Specifications.Base;
using MS.Clientes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Clientes.Application.Common.Specifications
{
    public sealed class ClientByIdSpecification : BaseSpecification<Cliente>
    {
        public ClientByIdSpecification(long id) : base(x => x.Id == id) { }
    }
}
