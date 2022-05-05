using RocketServe.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Data.Repositories.RestaurantRepository
{
    public interface IRestaurantRepository : IBaseRepository<Restaurant, string>
    {
        Task<List<Restaurant>> GetUserRestaurants(User user);
    }
}
