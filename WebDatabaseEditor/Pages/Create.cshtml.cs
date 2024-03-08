using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebDatabaseEditor.Database;
using WebDatabaseEditor.Models;

namespace WebDatabaseEditor.Pages
{
    public class CreateModel : PageModel
    {
        private readonly DatabaseService _databaseService;

        public CreateModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [BindProperty]
        public string TableName { get; set; }

        [BindProperty]
        public List<string> ColumnTable { get; set; }

        [BindProperty]
        public string SelectedRole { get; set; }

        public List<SelectListItem> Roles { get; set; }

        public IActionResult OnGetRoles()
        {
            Roles = _databaseService.GetRoles()
                .Select(role => new SelectListItem { Value = role, Text = role })
                .ToList();

            return new JsonResult(Roles);
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPostCreate([FromBody] CustomTable model)
        {
            if (string.IsNullOrEmpty(TableName) || ColumnTable == null || !ColumnTable.Any() || string.IsNullOrEmpty(SelectedRole))
            {
                ModelState.AddModelError(string.Empty, "¬ведите все необходимые данные.");
                return Page();
            }

            if (model == null || string.IsNullOrEmpty(model.TableName) || model.Columns == null || !model.Columns.Any() || string.IsNullOrEmpty(model.SelectedRole))
            {
                return BadRequest("Invalid data.");
            }

            _databaseService.CreateCustomTable(model.TableName, model.Columns, model.SelectedRole);

            return RedirectToPage("/Index");
        }
    }
}