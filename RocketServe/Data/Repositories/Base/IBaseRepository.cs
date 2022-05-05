using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Data.Repositories.Base
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">model type</typeparam>
    /// <typeparam name="U">model ID type</typeparam>
    public interface IBaseRepository<T,U>
    {
        Task<T> GetByIdAsync(U id);
        Task<List<T>> GetAllAsync();
        IQueryable<T> GetAll();

        /// <summary>
        /// if id = null Add 
        /// else update current item in DB
        /// </summary>
        /// <param name="item">item to be added or updated</param>
        /// <returns></returns>
        Task<T> SaveAsync(T item);

        Task RemoveByIdAsync(U id);
        Task RemoveAsync(T item);
        Task RemoveRangeAsync(IEnumerable<T> item);

        Task<T> InsertAsync(T items);
        Task UpdateAsync(T Item);
    }
}
