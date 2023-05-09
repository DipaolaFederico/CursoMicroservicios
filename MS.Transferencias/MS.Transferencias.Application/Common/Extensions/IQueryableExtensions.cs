using Microsoft.EntityFrameworkCore;
using MS.Transferencias.Application.Common.Interfaces;
using MS.Transferencias.Application.Common.Pagination;

namespace MS.Transferencias.Application.Common.Extensions
{
    public static class IQueryableExtensions
    {
        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var query = source;

            var totalCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);

            var totalPages = Math.Ceiling(totalCount / (double)specification.Take);

            var pageIndex = totalPages >= specification.Skip ? specification.Skip : 1;

            query = query.Skip((int)(pageIndex - 1) * specification.Take)
                    .Take(specification.Take);

            var items = totalCount > 0 ? await query.ToListAsync(cancellationToken).ConfigureAwait(false) : null;

            var paginatedList = new PaginatedList<T>(totalCount, specification.Take, (int)pageIndex, items);

            return paginatedList;
        }
    }
}
