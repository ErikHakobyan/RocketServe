using Microsoft.AspNetCore.Components;
using RocketServe.Data;
using RocketServe.Data.Repositories.OrderRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Services
{
    public class BasketService
    {
        [Inject]
        protected IOrderRepository orderRepo { get; set; }
        [Inject]
        protected RealTimeServices realTimeServices { get; set; }
        public Order Order { get; private set; }

        public string TableId { get; set; }
        public string RestaurantId { get; set; }
        public BasketService(IOrderRepository orderRepo,
            RealTimeServices realTimeServices)
        {
            Order = new Order();
            Order.OrderDetails = new List<OrderDetail>();
            this.orderRepo = orderRepo;
            this.realTimeServices = realTimeServices;
        }

        public void AddOrderDetail(OrderDetail orderDetail)
        {
            if (Order.OrderDetails.Select(p => p.Product).Contains(orderDetail.Product))
                return;


            if (orderDetail.Quantity == 0)
                orderDetail.Quantity = 1;
            orderDetail.UnitPrice = orderDetail.Product.Price;
            orderDetail.TotalPrice = orderDetail.Quantity * orderDetail.UnitPrice;
            this.Order.TotalPrice += orderDetail.TotalPrice;

            orderDetail.PropertyChanged += OrderDetail_PropertyChanged;
            Order.OrderDetails.Add(orderDetail);
        }

        private void OrderDetail_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var args = e as PropertyChangedExtendedEventArgs;
            OrderDetail orderDetail = (OrderDetail)sender;
            if(args.PropertyName == "Quantity")
            {
                Order.TotalPrice -= orderDetail.TotalPrice;

                orderDetail.TotalPrice = orderDetail.UnitPrice * (int)args.NewValue;
                Order.TotalPrice += orderDetail.TotalPrice;
            }
        }

        public void RemoveOrderDetail(OrderDetail orderDetail)
        {
            if (!Order.OrderDetails.Contains(orderDetail))
                return;

            Order.TotalPrice -= orderDetail.TotalPrice;
            Order.OrderDetails.Remove(orderDetail);
            orderDetail.PropertyChanged -= OrderDetail_PropertyChanged;
        }

        public async Task SaveOrder()
        {
            Order.TableId = TableId;
            Order.RestaurantId = RestaurantId;
            Order.Date = DateTime.Now;
            Order.Stage = OrderStage.New;

            await orderRepo.InsertAsync(Order);
            await Task.Factory.StartNew(()=>realTimeServices.InvokeOnOrderAdded(Order));

            this.Order = new Order();
            this.Order.OrderDetails = new List<OrderDetail>();

        }
    }
}
