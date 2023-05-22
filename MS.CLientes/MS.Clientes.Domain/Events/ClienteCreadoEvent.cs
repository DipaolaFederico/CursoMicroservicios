using MS.Clientes.Domain.Abstractions;
using MS.Clientes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Clientes.Domain.Events
{
    public class ClienteCreadoEvent : BaseEvent<Cliente>
    {
        public ClienteCreadoEvent(Cliente obj) : base(obj)
        {
            Type = nameof(ClienteCreadoEvent);
        }
    }
}
