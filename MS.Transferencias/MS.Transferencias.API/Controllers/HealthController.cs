using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MS.Transferencias.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _service;
        public HealthController(HealthCheckService service)
        {
            _service = service;
        }

        /// <summary>
        /// Método de health de la aplicacion
        /// </summary>
        /// <returns>Estado de los servicios y estado general de la app</returns>
        [HttpGet]
        public async Task<ActionResult> HealthCheck()
        {
            var report = await _service.CheckHealthAsync();

            var clienteHealthCheckValue = report.Entries.FirstOrDefault(x => x.Key == "ClientService").Value.Status;
            var databaseValue = report.Entries.FirstOrDefault(x => x.Key == "Database").Value.Status;

            return Ok(new
            {
                Services = new
                {
                    ClientServiceStatus = clienteHealthCheckValue == HealthStatus.Healthy,
                },
                DatabaseStatus = databaseValue == HealthStatus.Healthy,
                GeneralStatus = (clienteHealthCheckValue == HealthStatus.Healthy &&
                                 databaseValue == HealthStatus.Healthy)
            });
        }
    }
}
