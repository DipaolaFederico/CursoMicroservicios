using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Transferencias.Infrastructure.ExternalServices
{
    public sealed class ClientServiceOptions
    {
        public string BaseUrl { get; init; } = string.Empty;
        public string GetClientsAction { get; init; } = string.Empty;
        public string HealthAction { get; init; } = string.Empty;
    }
}
