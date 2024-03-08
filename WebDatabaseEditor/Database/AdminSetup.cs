using Microsoft.AspNetCore.Identity;
using WebDatabaseEditor.Models;

namespace WebDatabaseEditor.Database
{
    public class AdminSetup
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AdminSetup(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SetupAdminAsync()
        {
            try
            {
                if (await _userManager.FindByEmailAsync("Admin") == null)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = "superuser",
                        Email = "admin@example.com"
                    };
                    Console.WriteLine($"Debug: adminUser.UserName - {adminUser.UserName}");
                    var result = await _userManager.CreateAsync(adminUser, "Ilya789.QAZ"); // Пароль администратора

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"Error: {error.Description}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDynamicTableDataAsync: {ex.Message}");
            }

        }
    }
}
