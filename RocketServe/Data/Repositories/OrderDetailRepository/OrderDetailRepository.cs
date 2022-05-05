using Microsoft.EntityFrameworkCore;
using RocketServe.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Data.Repositories.OrderDetailRepository
{
    public class OrderDetailRepository : BaseRepository<OrderDetail, string>,
        IOrderDetailRepository
    {
        public OrderDetailRepository(ApplicationDbContext db) : base(db)
        {

        }

        public async Task<List<OrderDetail>> GetByOrderAsync(string orderId)
        {
            return await db.OrderDetails.Where(od => od.OrderId == orderId)
                .Include(od=>od.Product).ToListAsync();
        }
        public IQueryable<OrderDetail> GetByOrder(string orderId)
        {
            return db.OrderDetails.Include(od => od.Product)
                .Where(od => od.OrderId == orderId);
        }
    }
}
