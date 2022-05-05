using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketServe.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<OrderDetail>()
                .Property(o => o.Quantity)
                .HasDefaultValue(1);
           

            //builder.Entity<UserRestaurant>().HasKey(ur => new { ur.UserId, ur.RestaurantId });


            //foreach (var item in RoleType.GetRoles())
            //{
            //    builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = item, NormalizedName = item.ToUpper(), Id = Guid.NewGuid().ToString(), ConcurrencyStamp = Guid.NewGuid().ToString() });
            //}
        }
    }
}
