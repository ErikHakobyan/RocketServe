using Microsoft.AspNetCore.Components;
using RocketServe.Data;
using RocketServe.Data.Repositories.OrderDetailRepository;
using RocketServe.Data.Repositories.OrderRepository;
using RocketServe.Data.Repositories.RestaurantRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChartJs.Blazor.PieChart;
using ChartJs.Blazor.Common;
using Microsoft.EntityFrameworkCore;
using ChartJs.Blazor.Util;

namespace RocketServe.Pages.BussinessAdmin
{
    public class OrderListBase : ComponentBase
    {
        [Parameter]
        public string RestaurantId { get; set; }

        [Inject]
        protected IRestaurantRepository restaurantRepo { get; set; }

        [Inject]
        protected IOrderRepository orderRepo { get; set; }

        [Inject]
        protected IOrderDetailRepository orderDetailRepo { get; set; }

        [Inject]
        protected NavigationManager navigationManager { get; set; }

        protected Restaurant currentRestaurant;
        protected IQueryable<Order> orders;
        protected PieConfig pieConfig;
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            currentRestaurant = await restaurantRepo.GetByIdAsync(RestaurantId);

            if (currentRestaurant == null)
            { //TODO : what if restaurant is not found
            }
            else
            {
                orders = orderRepo.GetByRestaurant(currentRestaurant).OrderByDescending(o=>o.Date);
                await ConfigureTablePieChartConfigAsync();
            }
        }

        protected async Task ConfigureTablePieChartConfigAsync()
        {
            pieConfig = new PieConfig();
            pieConfig.Options = new PieOptions()
            {
                Responsive = true,
                Title = new OptionsTitle
                {
                    Display = true,
                    Text = "Commonly used Tables"
                }
            };

            Dictionary<string,int> tables = new Dictionary<string, int>();

            foreach (var table in await orders.Select(o => o.Table.Name).ToListAsync())
            {
                if (tables.TryGetValue(table, out int count))
                {
                    tables[table] = ++count;
                }
                else
                {
                    tables.Add(table,1);
                    pieConfig.Data.Labels.Add(table);
                }
            }

            var dataset = new PieDataset<int>();
            var colors = Enumerable.Range(0, tables.Count)
                .Select(c => ColorUtil.RandomColorString()).ToArray();

            dataset.BackgroundColor = new IndexableOption<string>(colors);

            foreach (var item in tables.Select(t=>t.Value))
            {
                dataset.Add(item);
            }
            pieConfig.Data.Datasets.Add(dataset);

        }

        protected void ViewOrderDetails(Order order)
        {
            navigationManager.NavigateTo($"/OrderDetails/{order.Id}");
        }
    }
}
