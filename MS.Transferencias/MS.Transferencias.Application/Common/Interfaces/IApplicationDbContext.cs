using Microsoft.EntityFrameworkCore;
using MS.Transferencias.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MS.Transferencias.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Transferencia> Transferencias { get; set; }
        DbSet<T> GetDbSet<T>() where T : class;
        Task<int> SaveChangesAsync();
    }
}
