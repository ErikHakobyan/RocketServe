using Microsoft.AspNetCore.Identity;
using RocketServe.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Data
{
    public class User : IdentityUser, IEntity<string>
    {
        public IList<Restaurant> Restaurants { get; set; }
    }
}
