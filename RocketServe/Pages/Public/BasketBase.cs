using Microsoft.AspNetCore.Components;
using RocketServe.Data;
using RocketServe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Pages.Public
{
    public class BasketBase : ComponentBase
    {
        [Inject]
        protected BasketService basketService { get; set; }

        public Order Order { get; set; }
        protected async override Task OnInitializedAsync()
        {
             
            Order = basketService.Order;
            await base.OnInitializedAsync();
        }

        protected void SubtractCount(OrderDetail orderDetail)
        {
            if (orderDetail.Quantity == 1)
                basketService.RemoveOrderDetail(orderDetail);
            else
                orderDetail.Quantity--;
        }
        protected void AddCount(OrderDetail orderDetail)
        {
            orderDetail.Quantity++;
        }
        protected async Task SaveOrder()
        {
            await basketService.SaveOrder();
            this.Order = basketService.Order;
        }
    }
}
