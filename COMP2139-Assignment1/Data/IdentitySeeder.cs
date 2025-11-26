using System;
using System.Threading.Tasks;
using COMP2139_Assignment1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace COMP2139_Assignment1.Data
{
   

    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            // Access the RoleManager + UserManager from DI container
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            // Const Roles
            string[] roles = { "Admin", "Organizer", "Attendee" };

            // Ensure each role exists if not it'll create them
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role)) //preventing duplicates
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Default admin account information
            var adminEmail = "admin@example.com";
            var adminPassword = "Admin123!";

            // Check if the admin user already exists
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                // Create new Admin user
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Default Admin",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    // Assign the Admin role
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    // This is optional – useful for debugging
                    Console.WriteLine("Admin user creation failed:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(" - " + error.Description);
                    }
                }
            }
        }
    }
}