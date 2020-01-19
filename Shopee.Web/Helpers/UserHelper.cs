﻿namespace Shopee.Web.Helpers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Data.Entities;
    using Shopee.Web.Models;

    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public UserHelper(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await userManager.CreateAsync(user, password);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await signInManager.PasswordSignInAsync(model.Username,model.Password,model.RememberMe,false);

        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }
    }
}
