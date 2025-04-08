using Microsoft.EntityFrameworkCore;

namespace BankingApi.Models
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "User1",
                    Password = "Password123"
                },
                new User
                {
                    Id = 2,
                    Name = "User2",
                    Password = "Password456"
                }
            );

            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    AccountId = 1,
                    UserId = 1,
                    Balance = 1000.00m
                },
                new Account
                {
                    AccountId = 2,
                    UserId = 2,
                    Balance = 2000.00m
                }
            );
        }
    }

}
