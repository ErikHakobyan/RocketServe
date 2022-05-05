using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RocketServe.Data;
using RocketServe.Data.Repositories.RestaurantRepository;
using RocketServe.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using RocketServe.Services;

namespace RocketServe.Pages.BussinessAdmin
{

    public class BussinessListBase : ComponentBase
    {
        [Inject]
        IRestaurantRepository restaurantRepo { get; set; }

        [Inject]
        AuthenticationStateProvider authenticationStateProvider { get; set; }

        [Inject]
        UserManager<User> userManager { get; set; }

        [Inject]
        NavigationManager navigationManager { get; set; }

        [Inject]
        AmazonS3FileService amazonS3FileService { get; set; }

        protected bool noBussinesses = false;
        protected bool showPopup = false;
        protected bool showUserPopup = false;
        protected bool userFound = false;
        protected string userSearchResult;
        protected string foundUserRole;
        protected string userEmail;
        protected User currentUser;
        protected User userToAddToRestaurant;
        protected Restaurant objRestaurant;
        protected List<Restaurant> restaurants = new List<Restaurant>();
        protected List<string> roles = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var stateProvider = await authenticationStateProvider.GetAuthenticationStateAsync();
            currentUser = await userManager.FindByIdAsync(stateProvider.User.GetId().ToString());

            await GetRestaurantsAsync();
            roles = new List<string>
            {
                RoleType.BussinessAdmin,
                RoleType.BussnessManager,
                RoleType.Employee
            };
        }

        protected async Task GetRestaurantsAsync()
        {
            restaurants = await restaurantRepo.GetUserRestaurants(currentUser);
            if (restaurants.Count == 0)
                noBussinesses = true;
        }
        protected async Task ClosePopup()
        {
            showPopup = false;
            //if (!String.IsNullOrEmpty(objRestaurant.Id))
            //{
            //    var unChangedRestaurant = await restaurantRepo.GetByIdAsync(objRestaurant.Id);
            //    var changedRestaurant = restaurants.FirstOrDefault(r => r.Id == objRestaurant.Id);
            //    restaurants[restaurants.IndexOf(changedRestaurant)] = unChangedRestaurant;
            //}
        }
        protected void CloseUserPopup()
        {
            showUserPopup = false;
        }
        protected async Task<string> GetUserRole(User user)
        {
            var roles = await userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }
        protected async Task SaveRestaurantAsync()
        {
            if (objRestaurant.Users == null)
                objRestaurant.Users = new List<User>();

            if (!objRestaurant.Users.Contains(currentUser))
                objRestaurant.Users.Add(currentUser);

            await restaurantRepo.SaveAsync(objRestaurant);
            await GetRestaurantsAsync();
            showPopup = false;
            objRestaurant = null;
        }
        protected async Task RemoveRestaurantAsync()
        {
            await restaurantRepo.RemoveByIdAsync(objRestaurant.Id);
            await GetRestaurantsAsync();
        }
        protected void AddRestaurant()
        {
            objRestaurant = new Restaurant();
            objRestaurant.ImageURL = "images/defaultCardImage.svg";
            showPopup = true;
        }
        protected void EditRestaurant(Restaurant restaurant)
        {
            objRestaurant = restaurant;
            showPopup = true;
        }
        protected async Task FindUser()
        {
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                userFound = false;
                userSearchResult = "User not Found";
            }
            else
            {
                userToAddToRestaurant = user;
                userFound = true;
                userSearchResult = "User Found";
                foundUserRole = await GetUserRole(user);
            }
        }
        protected void SaveUserToRestaurant()
        {
            if (userToAddToRestaurant == null &&
            objRestaurant.Users.Contains(userToAddToRestaurant))
                return;

            if (objRestaurant.Users == null)
                objRestaurant.Users = new List<User>();
            objRestaurant.Users.Add(userToAddToRestaurant);
            userToAddToRestaurant = null;
            userSearchResult = string.Empty;
            showUserPopup = false;
        }
        protected void AddUserToRestaurant()
        {
            showUserPopup = true;
            userToAddToRestaurant = null;
            userSearchResult = string.Empty;
            userEmail = string.Empty;
        }
        protected void RemoveUserFromRestaurant(User user)
        {
            objRestaurant.Users.Remove(user);
        }
        protected void NavigateToBussiness(string id)
        {
            navigationManager.NavigateTo($"Bussiness/{id}");
        }
        protected void ViewOrders(Restaurant restaurant)
        {
            navigationManager.NavigateTo($"/OrderList/{restaurant.Id}");
        }
        protected async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            string result = await amazonS3FileService.UploadFileToAWSAsync(e.File);
            if (result.StartsWith("http"))
                objRestaurant.ImageURL = result;
            this.StateHasChanged();
        }
    }
}
