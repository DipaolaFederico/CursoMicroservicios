using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS.Transferencias.API.Controllers.Base;
using MS.Transferencias.Application.Common.Dtos;
using MS.Transferencias.Application.Transferencias.Commands;
using MS.Transferencias.Application.Transferencias.Queries;

namespace MS.Transferencias.API.Controllers
{
    public class TransferenciaController : ApiBaseController
    {
        /// <summary>
        /// Realiza una transferencia
        /// </summary>
        /// <param name="request">Transferencia a realizar</param>
        /// <returns>Estado final de la transferencia</returns>
        [HttpPost]
        public async Task<ActionResult<TransferenciaDto>> CreateTransferencia(CreateTransferenciaCommand request)
        {
            var response = await Mediator.Send(request);

            return Ok(response);
        }

        /// <summary>
        /// Obtiene ransferencias existentes para el cbu proporcionado
        /// </summary>
        /// <param name="cbu">Cbu a buscar</param>
        /// <returns>Transferencias para el cbu proporcionado</returns>
        [HttpGet("{cbu}")]
        public async Task<ActionResult<IEnumerable<TransferenciaDto>>> CreateTransferencia(string cbu)
        {
            var response = await Mediator.Send(new GetTransferenciaByCbuQuery(cbu));

            return Ok(response);
        }
    }
}
