using EpilepsieDB.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using EpilepsieDB.Repositories;

namespace EpilepsieDB.Services
{
    public abstract class AService<T> : IService<T> where T : BaseModel
    {
        protected readonly IRepository<T> _repository;

        public AService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<T> Get(int id)
        {
            return await _repository.Get(id);
        }

        public async Task<IEnumerable<T>> Get(
        Expression<Func<T, bool>> filter = null,
            string includeProperties = "")
        {
            return await _repository.Get(filter, includeProperties);
        }

        public async Task<List<T>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task Create(T model)
        {
            await _repository.Add(model);
        }

        public async Task<bool> Update(T model)
        {
            if (await _repository.Exists(model.ID))
            {
                await _repository.Update(model);
                return true;
            }

            return false;
        }

        public async Task<bool> Delete(int id)
        {
            T model = await _repository.Get(id);
            if (model != null && model != default(T))
            {
                await _repository.Delete(model);
                return true;
            }

            return false;
        }
    }
}
