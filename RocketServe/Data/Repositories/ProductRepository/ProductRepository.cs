using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Data.Repositories.ProductRepository
{
    public class ProductRepository : Base.BaseRepository<Product, string>,
        IProductRepository
    {
        public ProductRepository(ApplicationDbContext db) : base(db)
        {

        }
    }
}
