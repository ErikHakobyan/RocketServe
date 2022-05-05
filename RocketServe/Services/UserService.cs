using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using RocketServe.Data;
using Microsoft.AspNetCore.Components.Authorization;
using RocketServe.Extensions;

namespace RocketServe.Services
{
    [Authorize(Roles = RoleType.Admin)]
    public class UserService
    {

        //Server=(localdb)\\mssqllocaldb;Database=aspnet-RocketServe-31021593-2A9F-4968-A8B8-E85ABD5183F8;Trusted_Connection=True;MultipleActiveResultSets=true
        public const string defaultPass = "**********";

        [Inject]
        protected virtual UserManager<User> userManager { get; set; }

        [Inject]
        AuthenticationStateProvider authenticationStateProvider { get; set; }


        public UserService(UserManager<User> userManager,
            AuthenticationStateProvider authenticationStateProvider)
        {
            this.userManager = userManager;
            this.authenticationStateProvider = authenticationStateProvider;

        }

        public async Task<User> GetCurrentUserAsync()
        {
            var stateProvider = await authenticationStateProvider.GetAuthenticationStateAsync();
            var currentUser = await userManager.FindByIdAsync(stateProvider.User.GetId().ToString());
            return currentUser;
        }

        /// <summary>
        /// Get users from DB
        /// </summary>
        /// <returns>Task of User collection.
        /// Collection is IEnumerable</returns>
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            // get users from _UserManager
            List<User> usersDTO = new List<User>();
            List<User> users = userManager.Users.ToList();
            foreach (var item in users)
            {
                var user = new User
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    Email = item.Email,
                    PasswordHash = defaultPass,
                };
                user.NormalizedEmail = (await userManager.GetRolesAsync(user)).FirstOrDefault();
                usersDTO.Add(user);
            }
            return usersDTO;
        }


        /// <summary>
        /// Deletes the User
        /// </summary>
        /// <param name="Id">Id of the user to be deleted</param>
        /// <returns></returns>
        public async Task DeleteUserAsync(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user != null)
            {
                // Delete the user
                await userManager.DeleteAsync(user);
            }
        }

        /// <summary>
        /// if user exists methos updates the user
        /// if user is new it will create new user
        /// </summary>
        /// <param name="objUser">User to update or add</param>
        /// <param name="role">role for the user</param>
        /// <returns></returns>
        public async Task<IdentityResult> SaveUserAsync(User objUser, string role)
        {
            IdentityResult identityResult = new IdentityResult(); ;
            // Is this an existing user?
            if (objUser.Id != "")
            {
                var user = await userManager.FindByIdAsync(objUser.Id);
                if (user != null)
                {
                    try
                    {
                        if (objUser.UserName != user.UserName)
                            identityResult = await userManager.SetUserNameAsync(user, objUser.UserName);

                        if (objUser.Email != user.Email)
                        {
                            identityResult = await userManager.SetEmailAsync(user, objUser.Email);
                            string emailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                            identityResult = await userManager.ConfirmEmailAsync(user, emailToken);
                        }
                        if (objUser.PasswordHash != defaultPass)
                        {
                            string token = await userManager.GeneratePasswordResetTokenAsync(user);
                            identityResult = await userManager.ResetPasswordAsync(user, token, objUser.PasswordHash);
                        }

                        if (role != null)
                        {
                            string oldRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();
                            if (oldRole != null)
                            {
                                identityResult = await userManager.RemoveFromRoleAsync(user, oldRole);
                            }
                            if (role != oldRole)
                                identityResult = await userManager.AddToRoleAsync(user, role);
                        }


                    }
                    catch
                    {
                        //return failed result
                        return identityResult;
                    }

                }
                //return last succesfull result
                return identityResult;
            }
            else
            {
                // create new user
                var NewUser =
                    new User
                    {
                        UserName = objUser.UserName,
                        Email = objUser.Email,
                        EmailConfirmed = true
                    };

                //insert new user and return succes result
                identityResult = await userManager.CreateAsync(NewUser, objUser.PasswordHash);
                identityResult = await userManager.AddToRoleAsync(NewUser, role);
                return identityResult;
            }
        }

        public async Task<string> GetRole(User user)
        {
            return (await userManager.GetRolesAsync(user)).FirstOrDefault();
        }
    }
}
