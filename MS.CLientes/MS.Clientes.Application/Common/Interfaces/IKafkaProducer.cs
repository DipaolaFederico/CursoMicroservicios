using MS.Clientes.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Clientes.Application.Common.Interfaces
{
    public interface IKafkaProducer
    {
        Task PublishAsync<T>(BaseEvent<T> message, CancellationToken cancellationToken = default) where T : BaseEntity;
    }
}
