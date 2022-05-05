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
    public class OrderDetail : IEntity<string>, INotifyPropertyChanged
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [MaxLength(100)]
        public string ExtraInstructions { get; set; }

        [Required]
        public float UnitPrice { get; set; }

        public float TotalPrice { get; set; }

        private int quantity;
        public int Quantity
        {
            get => quantity;
            set
            {
                int oldValue = quantity;
                quantity = value;
                NotifyPropertyChanged(nameof(Quantity), oldValue,value);
            }
        }


        public string ProductId { get; set; }
        public Product Product { get; set; }


        public string OrderId { get; set; }
        public Order Order { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName, object oldvalue, object newvalue)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedExtendedEventArgs(propertyName, oldvalue, newvalue));
        }
    }
}
