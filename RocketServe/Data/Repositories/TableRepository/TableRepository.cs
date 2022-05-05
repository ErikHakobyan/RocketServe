using RocketServe.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Data.Repositories.TableRepository
{
    public class TableRepository : BaseRepository<Table, string>,
        ITableRepository
    {
        public TableRepository(ApplicationDbContext db) : base(db)
        {

        }
    }
}
