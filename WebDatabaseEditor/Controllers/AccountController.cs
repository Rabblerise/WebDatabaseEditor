using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebDatabaseEditor.Models;

namespace WebDatabaseEditor.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            Console.WriteLine("Logout");
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index"); // Или любая другая страница, куда вы хотите перейти после выхода
        }
    }
}
