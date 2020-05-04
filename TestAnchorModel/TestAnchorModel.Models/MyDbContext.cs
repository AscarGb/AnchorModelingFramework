using AnchorModeling.Extensions;
using AnchorModeling.Models;
using Microsoft.EntityFrameworkCore;

namespace TestAnchorModel.Models
{
    public class MyDbContext: AnchorModelContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = "Server=ASUS;Database=MyDb;Integrated Security=true";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SetFKsRestrict();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Car> Cars { get; set; }
    }
}
