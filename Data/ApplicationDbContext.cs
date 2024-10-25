using Microsoft.EntityFrameworkCore;

namespace soladal_core.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) // Add this constructor
        {
        }

        public DbSet<User> Users { get; set; } 
    }
}