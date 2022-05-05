using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Data.Repositories.CategoryRepository
{
    interface ICategoryRepository : Base.IBaseRepository<Category,string>
    {
        Task<List<Category>> GetByRestaurantAsync(Restaurant restaurant);
    }
}
