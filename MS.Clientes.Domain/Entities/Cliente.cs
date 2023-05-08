using MS.Clientes.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Clientes.Domain.Entities
{
    public class Cliente : BaseEntity
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cuil { get; set; }
        public string TipoDocumento { get; set; }
        public int NroDocumento { get; set; }
        public bool EsEmpleadoBNA { get; set; }
        public string PaisOrigen { get; set; }
    }
}

