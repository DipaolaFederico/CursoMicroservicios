using MS.Transferencias.Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Transferencias.Application.Common.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetClients();
    }
}
