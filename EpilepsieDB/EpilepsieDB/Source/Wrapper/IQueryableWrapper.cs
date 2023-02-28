using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EpilepsieDB.Source.Wrapper
{
    public interface IQueryableWrapper<T> where T : class
    {
        IQueryable<T> Where(IQueryable<T> query, Expression<Func<T, bool>> predicate);

        IOrderedQueryable<T> OrderBy<U>(IQueryable<T> query, Expression<Func<T, U>> predicate);

        IQueryable<T> Include(
            IQueryable<T> query,
            string navigationPropertyPath);

        Task<List<T>> ToListAsync(
            IQueryable<T> query);

        IQueryable<T> AsNoTracking(IQueryable<T> query);

        Task<T> FirstOrDefaultAsync(IQueryable<T> query);
    }
}
