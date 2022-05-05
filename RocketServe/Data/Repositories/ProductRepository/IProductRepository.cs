using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Data.Repositories.ProductRepository
{
    interface IProductRepository: Base.IBaseRepository<Product, string>
    {
    }
}
