
namespace Shopee.Web.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.AspNetCore.Identity;
    using Shopee.Web.Helpers;

    public class SeedDb
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;
        private readonly Random random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            this.context = context;
            this.userHelper = userHelper;
            random = new Random();
        }

        public async Task SeedAsync()
        {
            await context.Database.EnsureCreatedAsync();

            var user = await userHelper.GetUserByEmailAsync("grados_2008@hotmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Raúl",
                    LastName = "Grados",
                    Email = "grados_2008@hotmail.com",
                    UserName = "rgrados",
                    PhoneNumber = "+51961819297"
                };

                var result = await userHelper.AddUserAsync(user, "mejorando");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
            }

            if (!context.Products.Any())
            {
                AddProduct("iPhone X", user);
                AddProduct("Magic Mouse", user);
                AddProduct("Keyboard Logitech", user);
                await context.SaveChangesAsync();
            }
        }

        private void AddProduct(string name, User user)
        {
            context.Products.Add(new Product
            {
                Name = name,
                Price = random.Next(100),
                IsAvailabe = true,
                Stock = random.Next(100),
                User = user,
            });
        }
    }
}