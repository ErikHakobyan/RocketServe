using Microsoft.AspNetCore.Components;
using RocketServe.Data;
using RocketServe.Data.Repositories.OrderRepository;
using RocketServe.Data.Repositories.RestaurantRepository;
using RocketServe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Pages.Employee
{

    public class EmployeeOrdersBase : ComponentBase, IDisposable
    {
        [Inject]
        IOrderRepository orderRepo { get; set; }
        [Inject]
        IRestaurantRepository restaurantRepo { get; set; }
        [Inject]
        UserService userService { get; set; }
        [Inject]
        RealTimeServices realTimeServices { get; set; }
        [Inject]
        NavigationManager navigationManager { get; set; }

        protected List<Order> orders;
        protected List<Order> newOrders;
        protected List<Order> preparingOrders;
        protected List<Order> servedOrders;
        protected User currentUser;
        protected Restaurant restaurant;
        protected OrderStage[] stages;
        protected async override Task OnInitializedAsync()
        {
            currentUser = await userService.GetCurrentUserAsync();
            restaurant = (await restaurantRepo.GetUserRestaurants(currentUser)).FirstOrDefault();
            if (restaurant != null)
            {
                orders = await orderRepo.GetByDateRestaurantAsync(DateTime.Now, restaurant);
                foreach (var item in orders)
                {
                    item.PropertyChanged += Order_PropertyChanged;
                }
                realTimeServices.OnOrderAdded += RealTimeServices_OnOrderAdded;
                await base.OnInitializedAsync();
                stages = Enum.GetValues<OrderStage>();
                if (orders != null && orders.Count != 0)
                {
                    newOrders = orders.Where(o => o.Stage == OrderStage.New).ToList();
                    preparingOrders = orders.Where(o => o.Stage == OrderStage.Preparing).ToList();
                    servedOrders = orders.Where(o => o.Stage == OrderStage.Served).ToList();
                }
            }
        }

        protected void RealTimeServices_OnOrderAdded(Order order)
        {
            if (order.Restaurant.Id == restaurant.Id)
            {
                orders.Add(order);
                newOrders.Add(order);
            }
            order.PropertyChanged += Order_PropertyChanged;
            InvokeAsync(() =>
            {

                StateHasChanged();
            });
        }

        private async Task Order_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Stage")
            {
                var order = (Order)sender;
                if (newOrders.Contains(order))
                    newOrders.Remove(order);
                else if (preparingOrders.Contains(order))
                    preparingOrders.Remove(order);
                else if (servedOrders.Contains(order))
                    servedOrders.Remove(order);

                if (order.Stage == OrderStage.New)
                {
                    newOrders.Add(order);
                }
                else if (order.Stage == OrderStage.Preparing)
                {
                    preparingOrders.Add(order);
                }
                else if (order.Stage == OrderStage.Served)
                {
                    servedOrders.Add(order);
                }

                await orderRepo.SaveAsync(order);
            }
        }

        public void ViewOrderDetails(Order order)
        {
            navigationManager.NavigateTo($"/EmployeeOrderDetail/{order.Id}");
        }

        public void Dispose()
        {
            realTimeServices.OnOrderAdded -= RealTimeServices_OnOrderAdded;
        }
    }
}
