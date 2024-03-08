using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebDatabaseEditor.Models;

namespace WebDatabaseEditor.Pages.Account
{
    public class EditUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public EditUserModel(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public ApplicationUser User { get; set; }

        [BindProperty]
        public string SelectedRole { get; set; }

        public List<SelectListItem> Roles { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            User = await _userManager.FindByIdAsync(userId);

            if (User == null)
            {
                return NotFound();
            }
            else
            {
                User.Id = userId;
            }

            Roles = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();

            // Set the initial selected role
            var userRoles = await _userManager.GetRolesAsync(User);
            SelectedRole = userRoles.FirstOrDefault();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // ѕолучаем пользовател€ из базы данных дл€ последующего обновлени€
                var user = await _userManager.FindByIdAsync(User.Id);

                // ќчищаем роли пользовател€
                var existingRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, existingRoles);

                if (!string.IsNullOrEmpty(SelectedRole) && await _roleManager.RoleExistsAsync(SelectedRole))
                {
                    // ѕровер€ем, не принадлежит ли пользователь уже к выбранной роли
                    if (!await _userManager.IsInRoleAsync(user, SelectedRole))
                    {
                        await _userManager.AddToRoleAsync(user, SelectedRole);
                    }
                }

                user.UserName = User.UserName; 

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToPage("/Account/Manage");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    Console.WriteLine($"UpdateAsync failed. SelectedRole: {SelectedRole}, UserId: {User.Id}");
                }
            }
            else
            {
                Console.WriteLine($"Invalid model");
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelError: {modelError.ErrorMessage}");
                }
            }

            Roles = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
            return Page();
        }
    }
}