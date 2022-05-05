using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RocketServe.Data.Repositories.RestaurantRepository
{
    public class RestaurantRepository : Base.BaseRepository<Restaurant, string>,
        IRestaurantRepository
    {
        public RestaurantRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task<List<Restaurant>> GetUserRestaurants(User user)
        {
            var restaurants = await db.Restaurants.Include(r=>r.Users).ToListAsync();
            return restaurants.Where(r =>
            {
                if (r.Users != null)
                    return r.Users.Contains(user);
                else
                    return false;
            }).ToList();
        }

        /// <summary>
        /// includes only Users
        /// </summary>
        /// <returns></returns>
        public async override Task<List<Restaurant>> GetAllAsync()
        {
            return await db.Restaurants.Include(r => r.Users).ToListAsync();
        }


        /// <summary>
        /// includes nothing
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<Restaurant> GetByIdAsync(string id)
        {
            return await db.Restaurants.Include(r=>r.Tables)
                .FirstOrDefaultAsync(r=>r.Id == id);
        }
        
    }
}
