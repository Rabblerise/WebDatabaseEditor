using Microsoft.AspNetCore.Identity;
using WebDatabaseEditor.Models;

namespace WebDatabaseEditor.Database
{
    public class RegistrationService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegistrationService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IdentityResult> RegisterUserAsync(string username, string email, string password)
        {
            var user = new ApplicationUser { UserName = username, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            return result;
        }
    }
}
