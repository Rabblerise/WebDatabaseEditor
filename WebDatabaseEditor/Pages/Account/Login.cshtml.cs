using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebDatabaseEditor.Models;

[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<LoginModel> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _logger = logger;
        _userManager = userManager;
    }
    public void OnGet()
    {
        
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
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            ApplicationUser? signedUser = await _userManager.FindByEmailAsync(_userManager.NormalizeEmail(Input.Email));
            if (signedUser != null)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                Console.WriteLine($"Email: {Input.Email}\nPassword: {Input.Password}\nRememberMe:{Input.RememberMe} ");
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToPage("/Index");
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("/LoginWith2fa", new { ReturnUrl = "/Index", RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("/Lockout");
                }
                else
                {
                    _logger.LogError($"Login failed. Details: {result}");
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    ViewData["InvalidLoginAttempt"] = "Invalid login attempt. Please check your email and password.";
                    return Page();
                }
            }
            else
            {
                _logger.LogWarning("User account not found.");
            }

        }

        return Page();
    }
}