using AccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Account> Account { get; set; }
        public DbSet<Movement> Movement { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Movements)
                .WithOne(m => m.Account)
                .HasForeignKey(m => m.AccountId);

            modelBuilder.Entity<Account>().HasIndex(a => a.AccountNumber).IsUnique();
        }
    }
}
