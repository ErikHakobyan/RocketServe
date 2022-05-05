using RocketServe.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Data.Repositories.TableRepository
{
    interface ITableRepository : IBaseRepository<Table, string>
    {
    }
}
