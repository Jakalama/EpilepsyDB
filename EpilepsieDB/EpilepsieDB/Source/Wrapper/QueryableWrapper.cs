using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace EpilepsieDB.Source.Wrapper
{
    public class QueryableWrapper<T> : IQueryableWrapper<T> where T : class
    {
        public IQueryable<T> Where(IQueryable<T> query, Expression<Func<T, bool>> predicate)
        {
            return query.Where(predicate);
        }

        public IOrderedQueryable<T> OrderBy<U>(IQueryable<T> query, Expression<Func<T, U>> predicate)
        {
            return query.OrderBy(predicate);
        }

        public IQueryable<T> Include(
            IQueryable<T> query,
            string navigationPropertyPath)
        {
            return query.Include(navigationPropertyPath);
        }

        public async Task<List<T>> ToListAsync(
            IQueryable<T> query)
        {
            return await query.ToListAsync();
        }

        public IQueryable<T> AsNoTracking(IQueryable<T> query)
        {
            return query.AsNoTracking();
        }

        public async Task<T> FirstOrDefaultAsync(IQueryable<T> query)
        {
            return await query.FirstOrDefaultAsync();
        }
    }
}
