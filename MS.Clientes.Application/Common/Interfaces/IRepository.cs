using MS.Clientes.Application.Common.Pagination;
using MS.Clientes.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Clientes.Application.Common.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        public Task<T> GetAsync(ISpecification<T> specification = null, CancellationToken cancellationToken = default);

        public Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification = null, CancellationToken cancellationToken = default);

        public Task<PaginatedList<T>> GetPaginatedAsync(ISpecification<T> specification = null, CancellationToken cancellationToken = default);

        public Task AddAsync(T entity, CancellationToken cancellationToken = default);

        public Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        public Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}
