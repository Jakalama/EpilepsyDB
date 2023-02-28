using EpilepsieDB.Data;
using EpilepsieDB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using EpilepsieDB.Source.Wrapper;

namespace EpilepsieDB.Repositories
{
    public abstract class ARepository<T> : IRepository<T> where T : BaseModel
    {
        protected readonly IAppDbContext _context;
        protected readonly DbSet<T> dbSet;
        private readonly IQueryableWrapper<T> wrapper;

        protected ARepository(IAppDbContext context)
        {
            _context = context;
            dbSet = context.Set<T>();
            wrapper = new QueryableWrapper<T>();
        }

        public virtual async Task<T> Get(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<T> GetNoTracking(int id)
        {
            return await wrapper.FirstOrDefaultAsync(
                wrapper.AsNoTracking(
                    wrapper.Where(dbSet, m => m.ID == id)));
        }

        public virtual async Task<IEnumerable<T>> Get(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = "")
        {

            return await wrapper.ToListAsync(GetQueryable(filter, includeProperties));
        }

        public virtual IQueryable<T> GetQueryable(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = "")
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = wrapper.Where(query, filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = wrapper.Include(query, includeProperty);
            }

            return query;
        }

        public virtual async Task<List<T>> GetAll()
        {
            return await wrapper
                .ToListAsync(dbSet);
        }

        public virtual async Task<bool> Exists(int id)
        {
            return await dbSet.AnyAsync(p => p.ID == id);
        }

        public virtual async Task Add(T model)
        {
            dbSet.Add(model);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Update(T model)
        {
            T original = await dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == model.ID, default);

            // only apply changes which are set in the new model
            // prevent null values on "Required" fields
            T updated = (T) CheckUpdateModel(original, model);

            dbSet.Update(updated);

            await _context.SaveChangesAsync();
        }

        public virtual async Task Delete(T model)
        {
            dbSet.Remove(model);
            await _context.SaveChangesAsync();
        }

        private object CheckUpdateModel(object original, object update)
        {
            foreach (var property in update.GetType().GetProperties())
            {
                // only apply changes which are set in the new model
                // ToDo: not working for default values for int, etc
                if (property.GetValue(update, null) == null)
                {
                    property.SetValue(update, original.GetType().GetProperty(property.Name).GetValue(original, null));
                }
            }
            return update;
        }
    }
}
