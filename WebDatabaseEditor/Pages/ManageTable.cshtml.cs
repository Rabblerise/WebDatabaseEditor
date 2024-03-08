using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Primitives;
using WebDatabaseEditor.Database;
using WebDatabaseEditor.Models;
using WebDatabaseEditor.Moduls;

namespace WebDatabaseEditor.Pages
{
    public class ManageTableModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public List<SelectListItem> Roles { get; set; }

        public ManageTableModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public string TableName { get; set; }

        [BindProperty]
        public List<string> Columns { get; set; }

        [BindProperty]
        public List<string> Headers { get; set; }

        public DynamicTable DynamicTableData { get; set; }

        public async Task<IActionResult> OnGetAsync(string tableName)
        {
            try
            {
                DynamicTableData = await _context.GetDynamicTableDataAsync(tableName);

                if (DynamicTableData == null)
                {
                    return NotFound();
                }
                TempData["TableName"] = tableName;

                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnGetAsync: {ex.Message}");
                return BadRequest();
            }
        }

        public async Task<IActionResult> OnPostAsync(CustomTable model)
        {
            var user = await _userManager.GetUserAsync(User);
            var superUser = await _userManager.FindByIdAsync("1");

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
                List<List<string>> Rows = Columns.Select((value, index) => new { value, index })
                    .GroupBy(x => x.index / Headers.Count)
                    .Select(group => group.Select(x => x.value).ToList())
                    .ToList();

                string insertDataQuery = $"INSERT INTO {model.TableName} ({string.Join(", ", Headers)}) VALUES ";
                IEnumerable<string> values = Rows.Select(row => $"({string.Join(", ", row.Select(c => $"'{c}'"))})");
                insertDataQuery += string.Join(", ", values);

                string tableName = TempData["TableName"] as string;

                Console.WriteLine($"DynamicTableData.TableName: {tableName}");
                //await _context.Database.ExecuteSqlRawAsync($"DROP TABLE {tableName}");
                //await _context.Database.ExecuteSqlRawAsync(createTableQuery);
                //await _context.Database.ExecuteSqlRawAsync(insertDataQuery);
                await _context.Database.ExecuteSqlRawAsync($"UPDATE CustomTables SET TableName = '{model.TableName}', Columns = '{string.Join(",", Headers)}' WHERE TableName = '{tableName}'");

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Ошибка при создании таблицы: {ex.Message}" });
            }
        }
    }
}