using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Transferencias.Application.Common.Dtos
{
    public sealed record ClientDto
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cuil { get; set; }
    }

    public sealed record TransferenciaDto
    {
        public long Id { get; set; }

        public string Resultado { get; set; } = "FINALIZADA";

        public double Importe { get; set; }

        public string CbuOrigen { get; set; }
        public string CbuDestino { get; set; }
    }
}
