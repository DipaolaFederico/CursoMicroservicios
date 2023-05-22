using MS.Clientes.Domain.Abstractions;
using MS.Clientes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Clientes.Domain.Events
{
    public class ClienteModificadoEvent : BaseEvent<Cliente>
    {
        public ClienteModificadoEvent(Cliente previousState, Cliente obj) : base(obj)
        {
            PreviousState = previousState;
            Type = nameof(ClienteModificadoEvent);
        }

        public Cliente PreviousState { get;}
    }
}
