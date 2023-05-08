using Microsoft.AspNetCore.Mvc;
using MS.Clientes.API.Controllers.Base;
using MS.Clientes.Application.Clientes.Commads;
using MS.Clientes.Application.Clientes.Queries;
using MS.Clientes.Domain.Entities;

namespace MS.Clientes.API.Controllers
{
    public class ClienteController : ApiBaseController
    {
        /// <summary>
        /// Obiene todos los clientes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Cliente>> GetClientes()
        {
            var clientes = await Mediator.Send(new GetClientesQuery());

            return Ok(clientes);
        }

        /// <summary>
        /// Obtiene un cliente por Id
        /// </summary>
        /// <param name="id">Id del cliente</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetClienteById(long id)
        {
            var cliente = await Mediator.Send(new GetClienteByIdQuery(id));

            return Ok(cliente);
        }

        /// <summary>
        /// Modifica a un cliente por Id
        /// </summary>
        /// <param name="request">Cliente a modificar</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<Cliente>> UpdateCliente([FromBody] UpdateClienteCommand request)
        {
            var cliente = await Mediator.Send(request);

            return Ok(cliente);
        }

        /// <summary>
        /// Crea un nuevo cliente
        /// </summary>
        /// <param name="request">Los atributos necesarios para crear un cliente</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Cliente>> CreateCliente([FromBody]CreateClienteCommand request)
        {
            var clientes = await Mediator.Send(request);

            return Ok(clientes);
        }
    }
}
