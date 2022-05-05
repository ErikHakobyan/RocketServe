using Microsoft.AspNetCore.Components;
using RocketServe.Data;
using RocketServe.Data.Repositories.CategoryRepository;
using RocketServe.Data.Repositories.RestaurantRepository;
using RocketServe.Data.Repositories.TableRepository;
using RocketServe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Pages.Public
{
    public class MenuBase : ComponentBase
    {
        [Parameter]
        public string TableId { get; set; }

        [Inject]
        ITableRepository tableRepo { get; set; }

        [Inject]
        IRestaurantRepository restaurantRepo { get; set; }

        [Inject]
        ICategoryRepository categoryRepo { get; set; }

        [Inject]
        BasketService basket { get; set; }

        protected Restaurant restaurant;
        protected Table table;
        protected OrderDetail objOrderDetail;
        protected bool showProductPopup;
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            table = await tableRepo.GetByIdAsync(TableId);
            if (table != null)
            {
                restaurant = await restaurantRepo.GetByIdAsync(table.RestaurantId);
                restaurant.Categories = await categoryRepo.GetByRestaurantAsync(restaurant);
                basket.TableId = TableId;
                basket.RestaurantId = restaurant.Id;
            }
        }
        protected void ViewProduct(Product product)
        {
            objOrderDetail = new OrderDetail();
            objOrderDetail.Product = product;
            objOrderDetail.Quantity = 1;
            objOrderDetail.UnitPrice = product.Price;
            objOrderDetail.TotalPrice = objOrderDetail.UnitPrice * objOrderDetail.Quantity;

            showProductPopup = true;
            

        }
        protected void SubtractCount()
        {
            if (objOrderDetail.Quantity == 1)
                basket.RemoveOrderDetail(objOrderDetail);
            else
                objOrderDetail.Quantity--;
        }
        protected void AddCount()
        {
            objOrderDetail.Quantity++;
        }
        protected void AddToBasket()
        {
            basket.AddOrderDetail(objOrderDetail);
            CloseProductPopup();
        }
        protected void CloseProductPopup()
        {
            showProductPopup = false;
        }
    }
}
