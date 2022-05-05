using Microsoft.AspNetCore.Components;
using RocketServe.Data;
using RocketServe.Data.Repositories.OrderDetailRepository;
using RocketServe.Data.Repositories.OrderRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Pages.Employee
{
    public class EmployeeOrderDetailBase : ComponentBase
    {
        [Parameter]
        public string OrderId { get; set; }

        [Inject]
        IOrderRepository orderRepo { get; set; }

        [Inject]
        IOrderDetailRepository orderDetailRepo { get; set; }

        protected Order order;

        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            order = await orderRepo.GetByIdAsync(OrderId);
            if (order != null)
                order.OrderDetails = await orderDetailRepo.GetByOrderAsync(OrderId);
        }
    }
}
