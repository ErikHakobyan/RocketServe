using Microsoft.AspNetCore.Identity;
using RocketServe.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Data
{
    public class Restaurant : IEntity<string>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Address { get; set; }

        public string ImageURL { get; set; }

        public List<User> Users { get; set; }
        public List<Category> Categories { get; set; }
        public List<Table> Tables { get; set; }
        public List<Order> Orders { get; set; }
    }
}
