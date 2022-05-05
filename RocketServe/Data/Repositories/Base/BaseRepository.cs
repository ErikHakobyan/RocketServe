using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace RocketServe.Data.Repositories.Base
{
    public abstract class BaseRepository<T, U> : IBaseRepository<T, U> where T : class, IEntity<U>
    {
        protected ApplicationDbContext db;
        public BaseRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async virtual Task<List<T>> GetAllAsync()
        {
            return await db.Set<T>().ToListAsync();
        }
        public virtual IQueryable<T> GetAll()
        {
            return db.Set<T>().AsQueryable();
        }
        public async virtual Task<T> GetByIdAsync(U id)
        {
            return await db.Set<T>().FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public async virtual Task<T> InsertAsync(T item)
        {
            db.Set<T>().Add(item);
            await db.SaveChangesAsync();
            return item;
        }

        public async virtual Task RemoveAsync(T item)
        {
            db.Set<T>().Remove(item);
            await db.SaveChangesAsync();
        }

        public async virtual Task RemoveByIdAsync(U id)
        {
            db.Set<T>().Remove(await GetByIdAsync(id));
            await db.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> items)
        {
            db.Set<T>().RemoveRange(items);
            await db.SaveChangesAsync();
        }

        public async virtual Task<T> SaveAsync(T item)
        {
            if (!IsNullOrDefault(item.Id))
            {
                T dbItem = await GetByIdAsync(item.Id);
                if (dbItem != null)
                {
                    await UpdateAsync(item);
                }
                return item;
            }
            else
            {
                var insertedItem = await InsertAsync(item);
                return insertedItem;
            }
            
        }

        public async virtual Task UpdateAsync(T Item)
        {
            db.Set<T>().Update(Item);
            await db.SaveChangesAsync();
        }

        protected bool IsNullOrDefault(U inObj)
        {
            if (inObj == null) return true;
            return EqualityComparer<U>.Default.Equals(inObj, default);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Id type</typeparam>
    public interface IEntity<T>
    {
        public T Id { get; set; }
    }
}
