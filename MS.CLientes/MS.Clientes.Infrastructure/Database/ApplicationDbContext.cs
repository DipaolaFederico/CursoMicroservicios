using Microsoft.EntityFrameworkCore;
using MS.Clientes.Application.Common.Interfaces;
using MS.Clientes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Clientes.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Cliente> Clientes { get; set; }

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
