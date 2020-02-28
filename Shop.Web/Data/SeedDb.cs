using Microsoft.AspNetCore.Identity;
using Shop.Web.Data.Entities;
using Shop.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;

        // Quitar la inyeccion de  userManager  por que ?
        //  private readonly UserManager<User> userManager;
        private Random random;

        //  public SeedDb(DataContext context, UserManager<User> userManager)
        // public SeedDb(DataContext context)  
        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            this.context = context;
            this.userHelper = userHelper;
          //  this.userManager = userManager;
            this.random = new Random();
        }

        public async Task SeedAsync()
        {
            await this.context.Database.EnsureCreatedAsync();

            // Crear Un Usuario
            // var user = await this.userManager.FindByEmailAsync("vgalvez10@gmail.com");

            var user = await this.userHelper.GetUserByEmailAsync("vgalvez10@gmail.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Valentin",
                    LastName = "Galvez",
                    Email = "vgalvez10@gmail.com",
                    UserName = "vgalvez10@gmail.com"
                };

                // var result = await this.userManager.CreateAsync(user, "123456");
                var result = await this.userHelper.AddUserAsync(user, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
            }



            if (!this.context.Products.Any())
            {
                this.AddProduct("Iphone x", user);
                this.AddProduct("Samsung Galaxy j", user);
                this.AddProduct("Motorola", user);
                await this.context.SaveChangesAsync();
            }
        }

        // private void AddProduct(string name)
        private void AddProduct(string name, User user)
        {
            this.context.Products.Add(new Product
            {
                Name = name,
                Price = this.random.Next(100),
                IsAvailabe = true,
                Stock = this.random.Next(100),
                User = user
            });
        }

    }
}
