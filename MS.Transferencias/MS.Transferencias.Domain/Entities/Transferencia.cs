using MS.Transferencias.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Transferencias.Domain.Entities
{
    public sealed class Transferencia : BaseEntity
    {
        public string CuilOriginante { get; set; }
        public string CuilDestinatario { get; set; }
        public string CbuOrigen { get; set; }
        public string CbuDestino { get; set; }
        public double Importe { get; set; }
        public string Concepto { get; set; }
        public string Descripcion { get; set; }
    }
}
