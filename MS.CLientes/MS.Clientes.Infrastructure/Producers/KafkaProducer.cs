using KafkaFlow.Producers;
using MS.Clientes.Application.Common.Interfaces;
using MS.Clientes.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Clientes.Infrastructure.Producers
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducerAccessor _producerAccessor;

        public KafkaProducer(IProducerAccessor producerAccessor)
        {
            _producerAccessor = producerAccessor;
        }

        public async Task PublishAsync<T>(BaseEvent<T> message, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            var producer = _producerAccessor.GetProducer("publish-client");

            await producer.ProduceAsync("Clientes", Guid.NewGuid().ToString(), message);
        }
    }
}
