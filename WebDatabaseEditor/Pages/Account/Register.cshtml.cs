using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebDatabaseEditor.Models;

namespace WebDatabaseEditor.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Login")]
            public string Login { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Remember me")]
            public bool RememberMe { get; set; }

            [Display(Name = "Role")]
            public string Role { get; set; }

            public List<SelectListItem> Roles { get; set; }
        }

        public async Task<IActionResult> OnGet()
        {
            var roles = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
            Input = new InputModel
            {
                Roles = roles
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Login, Email = Input.Email };

                var emailExists = await _userManager.FindByEmailAsync(Input.Email);
                var userNameExists = await _userManager.FindByNameAsync(Input.Login);

                if (emailExists != null || userNameExists != null)
                {
                    ModelState.AddModelError(string.Empty, "Email or Login is already taken.");
                    return Page();
                }

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(Input.Role) && await _roleManager.RoleExistsAsync(Input.Role))
                    {
                        await _userManager.AddToRoleAsync(user, Input.Role);
                    }

                    await _signInManager.SignInAsync(user, isPersistent: Input.RememberMe);
                    return RedirectToPage("/Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            Input.Roles = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
            return Page();
        }
    }
}