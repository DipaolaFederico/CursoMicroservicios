using Microsoft.EntityFrameworkCore;
using MS.Clientes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MS.Clientes.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Cliente> Clientes { get; set; }
        DbSet<T> GetDbSet<T>() where T : class;
        Task<int> SaveChangesAsync();
    }
}
