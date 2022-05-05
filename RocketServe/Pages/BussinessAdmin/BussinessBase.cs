using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using RocketServe.Data;
using RocketServe.Data.Repositories.CategoryRepository;
using RocketServe.Data.Repositories.ProductRepository;
using RocketServe.Data.Repositories.RestaurantRepository;
using RocketServe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe.Pages.BussinessAdmin
{
    public class BussinessBase : ComponentBase
    {
        [Parameter]
        public string RestaurantId { get; set; }

        [Inject]
        IRestaurantRepository restaurantRepo { get; set; }

        [Inject]
        ICategoryRepository categoryRepo { get; set; }

        [Inject]
        IProductRepository productRepo { get; set; }

        [Inject]
        NavigationManager navigationManager { get; set; }

        [Inject]
        AmazonS3FileService amazonS3FileService { get; set; }

        protected bool showCategoryPopup;
        protected bool showProductPopup;
        protected Restaurant restaurant;
        protected Category objCategory;
        protected Product objProduct;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            restaurant = await restaurantRepo.GetByIdAsync(RestaurantId);

            //TODO: ceck if restaurant contains loged in user

            restaurant.Categories = await categoryRepo.GetByRestaurantAsync(restaurant);
            if (restaurant.Categories == null)
                restaurant.Categories = new List<Category>();
        }

        protected void AddCategory()
        {
            objCategory = new Category();
            showCategoryPopup = true;
        }
        protected void EditCategory(Category category)
        {
            objCategory = category;
            showCategoryPopup = true;
        }
        protected void CloseCategoryPopup()
        {
            showCategoryPopup = false;
        }
        protected async Task SaveCategoryAsync()
        {
            objCategory.Restaurant = restaurant;
            if (objCategory.Products == null)
                objCategory.Products = new List<Product>();
            var category = await categoryRepo.SaveAsync(objCategory);


            CloseCategoryPopup();
        }
        protected async Task RemoveCategoryAsync(Category category)

        {
            await productRepo.RemoveRangeAsync(category.Products);
            await categoryRepo.RemoveAsync(category);
        }

        protected void AddProduct(Category category)
        {
            showProductPopup = true;
            objCategory = category;
            objProduct = new Product();
        }
        protected void EditProduct(Category category, Product product)
        {
            objCategory = category;
            objProduct = product;
            showProductPopup = true;
        }
        protected async Task RemoveProduct(Category category, Product product)
        {
            await productRepo.RemoveAsync(product);
        }
        protected async Task SaveProductAsync()
        {
            objProduct.Category = objCategory;
            var product = await productRepo.SaveAsync(objProduct);
            await categoryRepo.SaveAsync(objCategory);
            CloseProductPopup();
        }
        protected void CloseProductPopup()
        {
            showProductPopup = false;
        }

        protected void NavigateToTables()
        {
            navigationManager.NavigateTo($"Tables/{RestaurantId}");
        }
        protected async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            string result = await amazonS3FileService.UploadFileToAWSAsync(e.File);
            if (result.StartsWith("http"))
                objProduct.ImageURL = result;
            this.StateHasChanged();
        }
    }
}
