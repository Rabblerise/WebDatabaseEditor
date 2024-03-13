using Microsoft.AspNetCore.Identity;

namespace WebDatabaseEditor.Models
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User" };

            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Создаем роль, если ее нет
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            ApplicationUser user = await userManager.FindByEmailAsync("admin@example.com");

            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                };
                await userManager.CreateAsync(user, "85912"); // Задайте здесь пароль для админа

                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}