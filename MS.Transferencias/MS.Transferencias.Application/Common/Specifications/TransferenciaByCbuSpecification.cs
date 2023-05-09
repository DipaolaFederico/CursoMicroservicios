using MS.Transferencias.Application.Common.Specifications.Base;
using MS.Transferencias.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Transferencias.Application.Common.Specifications
{
    public sealed class TransferenciaByCbuSpecification : BaseSpecification<Transferencia>
    {
        public TransferenciaByCbuSpecification(string cbu) : base(x => x.CbuOrigen == cbu || x.CbuDestino == cbu) { }
    }
}
