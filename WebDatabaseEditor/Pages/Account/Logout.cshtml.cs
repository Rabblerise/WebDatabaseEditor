using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebDatabaseEditor.Models;

namespace WebDatabaseEditor.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LogoutModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public void OnGet()
        {
            
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("work");
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}