using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Clientes.Domain.Abstractions
{
    public abstract class BaseEvent<T> where T : BaseEntity
    {
        public BaseEvent(T obj)
        {
            Item = obj;
        }

        public string Type { get; set; }

        public T Item { get; }
    }
}
