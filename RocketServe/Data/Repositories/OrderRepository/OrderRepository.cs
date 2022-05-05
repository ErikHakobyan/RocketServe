using Microsoft.EntityFrameworkCore;
using RocketServe.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Data.Repositories.OrderRepository
{
    
    public class OrderRepository : BaseRepository<Order, string>,
        IOrderRepository
    {
       
        public OrderRepository(ApplicationDbContext db) : base(db)
        {

        }

        public async Task<List<Order>> GetByDateRestaurantAsync(DateTime date,Restaurant restaurnat)
        {
            return (await db.Orders.Include(o=>o.Table).ToListAsync())
                .Where(o=> o.RestaurantId == restaurnat.Id).ToList();
        }

        public IQueryable<Order> GetByRestaurant(Restaurant restaurnat)
        {
            return db.Orders.Include(o => o.Table)
                .Where(o => o.RestaurantId == restaurnat.Id);
        }

        public async Task<List<Order>> GetByRestaurantAsync(Restaurant restaurnat)
        {
            return (await db.Orders.Include(o => o.Table).ToListAsync())
                .Where(o => o.RestaurantId == restaurnat.Id).ToList();
        }
    }
}
