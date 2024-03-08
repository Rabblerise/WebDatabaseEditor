using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebDatabaseEditor.Models;

namespace WebDatabaseEditor.Moduls
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<CustomTable> CustomTables { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
    }
}
