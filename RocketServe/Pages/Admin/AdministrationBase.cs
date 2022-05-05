using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using RocketServe.Data;
using RocketServe.Services;

namespace RocketServe.Pages.Admin
{
    public class AdministrationBase : ComponentBase
    {
        [Inject]
        UserManager<User> userManager { get; set; }
        [Inject]
        RoleManager<IdentityRole> _RoleManager { get; set; }
        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        UserService userService { get; set; }

		// Property used to add or edit the currently selected user
		protected User objUser = new User();

		// Collection to display the existing users
		protected List<User> colUsers = new List<User>();

		// Options to display in the roles dropdown when editing a user
		protected List<string> roles = new List<string>();

		// To enable showing the Popup
		protected bool ShowPopup = false;

		protected override async Task OnInitializedAsync()
		{
			roles = RoleType.GetRoles().ToList();
			await GetUsers();
		}

		protected void AddNewUser()
		{
			// Make new user
			objUser = new User();
			objUser.PasswordHash = UserService.defaultPass;
			// Set Id to blank so we know it is a new record
			objUser.Id = "";

			objUser.NormalizedEmail = roles[0];
			// Open the Popup
			ShowPopup = true;
		}
		protected async Task SaveUser()
		{
			ShowPopup = false;
			objUser.UserName = objUser.Email;
			await userService.SaveUserAsync(objUser, objUser.NormalizedEmail);
			await GetUsers();
		}
		protected void EditUser(User user)
		{

			// Set the selected user
			objUser = user;

			ShowPopup = true;
		}
		protected async Task DeleteUser()
		{
			// Close the Popup
			ShowPopup = false;
			// Delete the user
			await userService.DeleteUserAsync(objUser.Id);
			// Refresh Users
			await GetUsers();
		}
		public async Task GetUsers()
		{
			// Collection to hold users
			colUsers = (await userService.GetUsersAsync()).ToList();
		}
		protected void ClosePopup()
		{
			ShowPopup = false;
		}
	}
}
