using Microsoft.EntityFrameworkCore;
using WebDatabaseEditor.Models;
using WebDatabaseEditor.Moduls;

namespace WebDatabaseEditor.Database
{
    public class DatabaseService
    {
        private readonly ApplicationDbContext _context;
        public DatabaseService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void CreateCustomTable(string tableName, string columnNames, string selectedRole) {
            string creataTableQuery = $"CREATE TABLE {tableName} ({string.Join(",", columnNames.Select(c => $"{c} NVARCHAR(255)"))})";
            _context.Database.ExecuteSqlRaw(creataTableQuery);

            var customTable = new CustomTable
            {
                TableName = tableName,
                Columns = columnNames,
                SelectedRole = selectedRole
            };

            _context.CustomTables.Add(customTable);
            _context.SaveChanges();
        }

        public List<string> GetRoles()
        {
            var roles = _context.Roles.Select(role => role.Name).ToList();
            return roles;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<DatabaseService>();
        }
    }
}
