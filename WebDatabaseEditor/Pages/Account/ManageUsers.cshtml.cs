using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebDatabaseEditor.Models;
using WebDatabaseEditor.Moduls;

namespace WebDatabaseEditor.Pages.Account
{
    public class ManageUsersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageUsersModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public List<ApplicationUser> Users { get; set; }

        public async Task OnGetAsync()
        {
            Users = await _userManager.Users.ToListAsync();
        }

        public async Task<List<string>> GetRolesAsync(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }
        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var userToDelete = await _userManager.FindByIdAsync(id);

            if (userToDelete != null)
            {
                var result = await _userManager.DeleteAsync(userToDelete);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    return RedirectToPage("/Index");
                }
            }
            else
            {
                // Пользователь с указанным Id не найден
                ModelState.AddModelError(string.Empty, "User not found.");
            }

            return Page();
        }
    }
}