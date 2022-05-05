using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using RocketServe.Data;
using RocketServe.Data.Repositories.OrderDetailRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Pages.BussinessAdmin
{
    public class OrderDetailsBase : ComponentBase
    {
        [Parameter]
        public string OrderId { get; set; }

        [Inject]
        IOrderDetailRepository orderDetailRepo { get; set; }

        protected IQueryable<OrderDetail> orderDetails;
        protected async override Task OnInitializedAsync()
        {
            orderDetails = orderDetailRepo.GetByOrder(OrderId);
            await base.OnInitializedAsync();

        }
    }
}
