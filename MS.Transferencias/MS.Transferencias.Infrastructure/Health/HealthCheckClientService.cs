using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Transferencias.Infrastructure.ExternalServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Transferencias.Infrastructure.Health
{
    public class HealthCheckClientService : IHealthCheck
    {
        private readonly ILogger<HealthCheckClientService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptionsMonitor<ClientServiceOptions> _options;
        public HealthCheckClientService(ILogger<HealthCheckClientService> logger, IHttpClientFactory httpClientFactory, IOptionsMonitor<ClientServiceOptions> options)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _options = options;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Iniciando consulta de salud del servicio Clientes");

                using (var httpClient = _httpClientFactory.CreateClient("ClientService"))
                {
                    var healthUrl = httpClient.BaseAddress + _options.CurrentValue.HealthAction;

                    var response = await httpClient.GetAsync(healthUrl).ConfigureAwait(false);

                    _logger.LogInformation("Respuesta recibida del servicio de Clientes {@Response}", response);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        var healthStatus = JsonConvert.DeserializeObject<ClientServiceHealthCheckResponse>(json);

                        if(healthStatus.GeneralStatus)
                        {
                            _logger.LogInformation("Finalizando consulta de salud de servicio Clientes satisfactoriamente");
                            return HealthCheckResult.Healthy();
                        }
                    }

                    _logger.LogInformation("Finalizando consulta de salud de servicio Clientes con errores");

                    return HealthCheckResult.Unhealthy();
                }
            }
            catch (Exception)
            {
                _logger.LogInformation("Finalizando consulta de salud de servicio Clientes con errores");

                return HealthCheckResult.Unhealthy();
            }
        }
    }

    internal sealed record ClientServiceHealthCheckResponse
    {
        public bool DatabaseStatus { get; set; }
        public bool GeneralStatus { get; set; }
    }
}
