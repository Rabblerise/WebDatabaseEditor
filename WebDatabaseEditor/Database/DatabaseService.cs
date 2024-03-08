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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<DatabaseService>();
        }
    }
}
