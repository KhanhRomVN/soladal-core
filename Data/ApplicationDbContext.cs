using Microsoft.EntityFrameworkCore;

namespace soladal_core.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) // Add this constructor
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Clone> Clones { get; set; }
        public DbSet<Google> Googles { get; set; }
        public DbSet<Identity> Identities { get; set; }

    }

}