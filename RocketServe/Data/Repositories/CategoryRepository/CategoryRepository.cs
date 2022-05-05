using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RocketServe.Data.Repositories.CategoryRepository
{
    public class CategoryRepository : Base.BaseRepository<Category, string>,
        ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {

        }

        public async override Task<List<Category>> GetAllAsync()
        {
            return await db.Categories.Include(c=>c.Products).ToListAsync();
        }
        public async override Task<Category> GetByIdAsync(string id)
        {
            return await db.Categories.Include(c => c.Products).FirstOrDefaultAsync(c=>c.Id == id);
        }
        public async Task<List<Category>> GetByRestaurantAsync(Restaurant restaurant)
        {
            return await db.Categories.Include(c => c.Products)
                .Where(c => c.Restaurant == restaurant).ToListAsync();
        }
    }
}
