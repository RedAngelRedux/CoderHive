using CoderHive.Data;
using CoderHive.Enums;
using CoderHive.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoderHive.Services
{
    public class DataService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlogUser> _userManager;

        // This is constructor injection, i.e. grabs an already instantiated db context and assigns it to our private variable.
        // This only works if ApplicationDbContext is a registered service
        public DataService(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<BlogUser> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        /// <summary>
        /// This is wrapper function that will call some private methods
        /// </summary>
        /// <returns></returns>
        public async Task ManageDataAsync()
        {
            // Create the DB from the Migrations
            await _dbContext.Database.MigrateAsync();

            // [] Seeding a few Roles into the system
            await SeedRolesAsync();

            // [] Seed a few users into the system
            await SeedUsersAsync();
        }

        private async Task SeedRolesAsync()
        {
            // If there are already Roles in the system, do nothing.
            if (_dbContext.Roles.Any()) return;

            // otherwise we want to create a few Roles
            foreach(var role in Enum.GetNames(typeof(BlogRole)))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private async Task SeedUsersAsync()
        {

            // If there are already Users in the system, do nothing
            if (_dbContext.Users.Any()) return;

            // otherwise, add a few users

            // Step 1:  Declare an instance of BlogUser
            var adminUser = new BlogUser()
            {
                Email = "sammy@sammynava.com",
                UserName = "sammy@sammynava.com",
                FirstName = "Sammy",
                LastName = "Nava",
                DisplayName = "Sammy",
                PhoneNumber = "(951) 482-5330",
                EmailConfirmed = true
            };

            var modUser = new BlogUser()
            {
                Email = "redangelredux@hotmail.com",
                UserName = "redangelredux@hotmail.com",
                FirstName = "Red",
                LastName = "Angel",
                PhoneNumber = "(951) 482-5330",
                EmailConfirmed = true
            };

            // Step 2:  Use the UserManager to create a new user that is defined by adminUser
            await _userManager.CreateAsync(adminUser,"Abc&123!");
            await _userManager.CreateAsync(modUser, "Abc&123!");

            // Step 3:  Add this new user to the Administrator role
            await _userManager.AddToRoleAsync(adminUser, BlogRole.Administrator.ToString());
            await _userManager.AddToRoleAsync(modUser,BlogRole.Moderator.ToString());
        }



    }
}
