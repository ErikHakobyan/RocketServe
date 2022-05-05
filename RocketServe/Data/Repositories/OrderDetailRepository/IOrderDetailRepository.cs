using RocketServe.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Data.Repositories.OrderDetailRepository
{
    public interface IOrderDetailRepository : IBaseRepository<OrderDetail, string>
    {
        Task<List<OrderDetail>> GetByOrderAsync(string orderId);
        IQueryable<OrderDetail> GetByOrder(string orderId);
    }
}
