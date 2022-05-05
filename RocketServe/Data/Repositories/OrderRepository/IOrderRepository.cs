using RocketServe.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Data.Repositories.OrderRepository
{
    public interface IOrderRepository : IBaseRepository<Order, string>
    {
        Task<List<Order>> GetByDateRestaurantAsync(DateTime date,Restaurant restaurnat);
        Task<List<Order>> GetByRestaurantAsync(Restaurant restaurnat);
        IQueryable<Order> GetByRestaurant(Restaurant restaurnat);
    }
}
