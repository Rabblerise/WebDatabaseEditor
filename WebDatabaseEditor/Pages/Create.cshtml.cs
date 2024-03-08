using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using WebDatabaseEditor.Database;
using WebDatabaseEditor.Models;
using WebDatabaseEditor.Moduls;

namespace WebDatabaseEditor.Pages
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public List<SelectListItem> Roles { get; set; }

        public CreateModel(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [BindProperty]
        public string TableName { get; set; }

        [BindProperty]
        public List<string> Columns { get; set; }

        [BindProperty]
        public List<string> Headers { get; set; }

        public string SelectedRole { get; set; }

        public IActionResult OnGet()
        {
            Roles = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
            return Page();
        }

        public async Task<IActionResult> OnGetRoles()
        {
            return new JsonResult(Roles);
        }

        public async Task<IActionResult> OnPostAsync(CustomTable model)
        {
            var user = await _userManager.GetUserAsync(User);
            var superUser = await _userManager.FindByIdAsync("1");
            if (!(await _userManager.IsInRoleAsync(user, model.SelectedRole) || user.UserName == superUser.UserName))
            {
                return Forbid();
            }
            Headers = Request.Form.Keys
            .Where(key => key.StartsWith("CellValues_1"))
            .Select(key => Request.Form[key].ToString())
            .FirstOrDefault()?.Split(",")
            .ToList() ?? new List<string>();

            Columns = Request.Form.Keys
            .Where(key => key.StartsWith("CellValues_2"))
            .Select(key => Request.Form[key].ToString())
            .FirstOrDefault()?.Split(",")
            .ToList() ?? new List<string>();

            string createTableQuery = $"CREATE TABLE {model.TableName} (ID INT PRIMARY KEY IDENTITY (1, 1) NOT NULL, ";
            createTableQuery += string.Join(", ", Headers.Select(c => $"{c} NVARCHAR(MAX)")) + ")";

            try
            {
                Console.WriteLine($"Info: {createTableQuery}");
                await _context.Database.ExecuteSqlRawAsync(createTableQuery);

                List<List<string>> Rows = Columns.Select((value, index) => new { value, index })
                    .GroupBy(x => x.index / Headers.Count)
                    .Select(group => group.Select(x => x.value).ToList())
                    .ToList();

                string insertDataQuery = $"INSERT INTO {model.TableName} ({string.Join(", ", Headers)}) VALUES ";
                IEnumerable<string> values = Rows.Select(row => $"({string.Join(", ", row.Select(c => $"'{c}'"))})");
                insertDataQuery += string.Join(", ", values);

                Console.WriteLine($"Info: {insertDataQuery}");

                await _context.Database.ExecuteSqlRawAsync(insertDataQuery);

                await _context.Database.ExecuteSqlAsync($"INSERT INTO CustomTables (TableName, Columns, SelectedRole) values ({model.TableName}, {string.Join(", ", Headers)}, {model.SelectedRole})");

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Ошибка при создании таблицы: {ex.Message}" });
            }
        }
    }
}