using Microsoft.EntityFrameworkCore;
using MS.Transferencias.Application.Common.Interfaces;
using MS.Transferencias.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Transferencias.Application.Common.Specifications
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> source, ISpecification<T> specification)
        {
            if (source == null)
                throw new ArgumentNullException("No data set was provided");

            var query = source;

            if (specification != null)
            {
                query = specification.Criteria != null ? query.Where(specification.Criteria) : query;

                query = specification.Includes.Aggregate(query,
                                        (current, include) => current.Include(include));

                if (specification.OrderBy != null)
                    query = query.OrderBy(specification.OrderBy);
                else if (specification.OrderByDescending != null)
                    query = query.OrderByDescending(specification.OrderByDescending);

                query = specification.GroupBy != null ? query.GroupBy(specification.GroupBy).SelectMany(x => x) : query;

                if (specification.IsPagingEnabled)
                    query = query.Skip((specification.Skip - 1) * specification.Take)
                                 .Take(specification.Take);
            }

            return query;
        }
    }
}
