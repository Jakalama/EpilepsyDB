using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using EpilepsieDB.Models;

namespace EpilepsieDB.Services
{
    public interface IService<T> where T : BaseModel
    {
        Task<T> Get(int id);
        Task<IEnumerable<T>> Get(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = "");

        Task<List<T>> GetAll();

        Task Create(T model);

        Task<bool> Update(T model);
        Task<bool> Delete(int id);
    }
}
