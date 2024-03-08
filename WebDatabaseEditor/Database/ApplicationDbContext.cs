using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Reflection.Emit;
using WebDatabaseEditor.Models;
using System.Linq;

namespace WebDatabaseEditor.Moduls
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<CustomTable> CustomTables { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public async Task<DynamicTable> GetDynamicTableDataAsync(string tableName)
        {
            try
            {
                var customTable = await CustomTables.FirstOrDefaultAsync(ct => ct.TableName == tableName);

                if (customTable == null)
                {
                    return null; // Таблица с указанным именем не найдена
                }

                var headers = customTable.Columns.Split(',').ToList();

                // Исключаем колонку 'id', если она присутствует
                headers = headers.Where(header => header.ToLower() != "id").ToList();

                using (var connection = Database.GetDbConnection())
                {
                    await connection.OpenAsync();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = $"SELECT * FROM {customTable.TableName}";
                        using (var result = await command.ExecuteReaderAsync())
                        {
                            var data = new List<Dictionary<string, string>>();

                            while (await result.ReadAsync())
                            {
                                var rowData = new Dictionary<string, string>();
                                for (int i = 0; i < result.FieldCount; i++)
                                {
                                    var header = result.GetName(i);

                                    // Пропускаем колонку 'id'
                                    if (header.ToLower() == "id")
                                        continue;

                                    var value = result[i]?.ToString() ?? string.Empty;
                                    rowData.Add(header, value);
                                }
                                data.Add(rowData);
                            }

                            return new DynamicTable
                            {
                                TableName = customTable.TableName,
                                Headers = headers,
                                Data = data
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDynamicTableDataAsync: {ex.Message}");
                return null;
            }
        }
    }
}
