using EpilepsieDB.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;

namespace EpilepsieDB.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> Get(int id);

        Task<T> GetNoTracking(int id);

        Task<IEnumerable<T>> Get(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = "");

        IQueryable<T> GetQueryable(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = "");

        Task<List<T>> GetAll();
        Task<bool> Exists(int id);

        Task Add(T model);
        Task Update(T model);
        Task Delete(T model);
    }
}
