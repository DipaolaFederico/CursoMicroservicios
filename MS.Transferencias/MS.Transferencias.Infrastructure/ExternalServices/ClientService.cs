using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Transferencias.Application.Common.Dtos;
using MS.Transferencias.Application.Common.Exceptions;
using MS.Transferencias.Application.Common.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Transferencias.Infrastructure.ExternalServices
{
    public sealed class ClientService : IClientService
    {
        private readonly IOptionsMonitor<ClientServiceOptions> _options;
        private readonly ILogger<ClientService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public ClientService(IOptionsMonitor<ClientServiceOptions> options, ILogger<ClientService> logger, IHttpClientFactory httpClientFactory)
        {
            _options = options;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<ClientDto>> GetClients()
        {
            _logger.LogInformation("Iniciando proceso de consulta de clientes");

            using(var httpClient = _httpClientFactory.CreateClient("ClientService"))
            {
                var getClientUrl = httpClient.BaseAddress + _options.CurrentValue.GetClientsAction;

                var response = await httpClient.GetAsync(getClientUrl).ConfigureAwait(false);

                _logger.LogInformation("Consulta realizada. Respuesta obtenida {@Response}", response);

                if (!response.IsSuccessStatusCode)
                    throw new InternalServerErrorException($"Error al tratar de consultar los clients {response.ReasonPhrase}");

                var json = await response.Content.ReadAsStringAsync();

                var clients = JsonConvert.DeserializeObject<IEnumerable<ClientDto>>(json);

                _logger.LogInformation("Consulta finalizada correctamente");

                return clients;
            }
        }
    }
}
