using RocketServe.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RocketServe.Data
{
    public delegate Task PropertyChangedEventHandlerAsync(object sender, PropertyChangedEventArgs e);
    public class Order : IEntity<string>
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [MaxLength(100)]
        public string ExtraInstructions { get; set; }

        public float TotalPrice { get; set; }

        public DateTime Date { get; set; }

        private OrderStage stage;
        public OrderStage Stage
        {
            get => stage;
            set
            {
                stage = value;
                OnPropertyChanged();
            }
        }

        public string TableId { get; set; }
        public Table Table { get; set; }

        public string RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public event PropertyChangedEventHandlerAsync PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public enum OrderStage
    {
        New,
        Preparing,
        Served
    }
}
