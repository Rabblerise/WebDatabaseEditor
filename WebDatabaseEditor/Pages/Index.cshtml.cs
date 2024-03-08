using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebDatabaseEditor.Models;
using WebDatabaseEditor.Moduls;

namespace WebDatabaseEditor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<CustomTable> Items { get; set; }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User) ?? new ApplicationUser();
                var superUser = await _userManager.FindByIdAsync("1") ?? new ApplicationUser();

                if (user.UserName?.ToString() == superUser.UserName?.ToString())
                {
                    Items = _context.CustomTables.ToList();
                }
                else if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    Items = _context.CustomTables
                        .Where(table => roles.Any(role => table.SelectedRole == role || table.SelectedRole.StartsWith(role + ".")))
                        .ToList();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrieving data: {ex.Message}");
                Console.WriteLine(ex.StackTrace);

                return BadRequest("Error while retrieving data. Please check the logs for more details.");
            }

            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {
            var itemToDelete = _context.CustomTables.Find(id);
            if (itemToDelete == null)
            {
                return NotFound();
            }

            _context.Database.ExecuteSqlRaw($"DROP TABLE {itemToDelete.TableName}");
            _context.CustomTables.Remove(itemToDelete);
            _context.SaveChanges();

            return RedirectToPage();
        }
    }
}