using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MS.Clientes.API.Controllers
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

            var databaseValue = report.Entries.FirstOrDefault(x => x.Key == "Database").Value.Status;

            return Ok(new
            {
                DatabaseStatus = databaseValue == HealthStatus.Healthy,
                GeneralStatus = databaseValue == HealthStatus.Healthy
            });
        }
    }
}
