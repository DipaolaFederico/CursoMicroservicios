using Microsoft.EntityFrameworkCore;
using MS.Transferencias.Application.Common.Interfaces;
using MS.Transferencias.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Transferencias.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Transferencia> Transferencias { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await ((DbContext)this).SaveChangesAsync();
        }

        public DbSet<T> GetDbSet<T>() where T : class
        {
            return Set<T>();
        }
    }
}
