using Microsoft.EntityFrameworkCore;
using MS.Transferencias.Application.Common.Extensions;
using MS.Transferencias.Application.Common.Interfaces;
using MS.Transferencias.Application.Common.Pagination;
using MS.Transferencias.Application.Common.Specifications;
using MS.Transferencias.Domain.Abstractions;


namespace MS.Transferencias.Infrastructure.Repositories
{
    public sealed class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IApplicationDbContext _dbContext;
        private DbSet<T> _dbSet;
        public Repository(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.GetDbSet<T>();
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            try
            {
                if (entity is IAuditableEntity)
                    UpdateAuditableEntity((IAuditableEntity)entity, isCreational: true);

                await _dbSet.AddAsync(entity).ConfigureAwait(false);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                var entityToRemove = await _dbSet.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

                if (entityToRemove == null)
                    throw new Exception("The entity to remove does not exist");

                _dbSet.Remove(entityToRemove);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification = null, CancellationToken cancellationToken = default)
        {
            return await SpecificationEvaluator<T>.GetQuery(_dbSet, specification).ToListAsync(cancellationToken);
        }

        public Task<T> GetAsync(ISpecification<T> specification = null, CancellationToken cancellationToken = default)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbSet, specification).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<PaginatedList<T>> GetPaginatedAsync(ISpecification<T> specification = null, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!specification.IsPagingEnabled)
                    throw new Exception("No paging was specified");

                var query = SpecificationEvaluator<T>.GetQuery(_dbSet, specification);

                var paginatedList = await query.ToPaginatedListAsync(specification, cancellationToken);

                return paginatedList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            try
            {
                if (entity is IAuditableEntity)
                    UpdateAuditableEntity((IAuditableEntity)entity);

                _dbSet.Update(entity);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UpdateAuditableEntity(IAuditableEntity entity, bool isCreational = false)
        {
            if (isCreational)
                entity.CreateDate = DateTime.Now;
            else
                entity.UpdateDate = DateTime.Now;
        }
    }
}
