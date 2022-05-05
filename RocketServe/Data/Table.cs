using RocketServe.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Data
{
    public class Table : IEntity<string>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public bool IsWorking { get; set; }
        public string RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public List<Order> Orders { get; set; }
    }
}
