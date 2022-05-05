using RocketServe.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Services
{
    public delegate void OrderAdded(Order order);
    public class RealTimeServices
    {
        public event OrderAdded OnOrderAdded;
        static RealTimeServices()
        {
           
        }
        public void InvokeOnOrderAdded(Order order)
        {
            OnOrderAdded?.Invoke(order);
        }
    }
}
