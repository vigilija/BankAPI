using Microsoft.EntityFrameworkCore;

namespace BankAPI
{
    public class BankDbContext : DbContext
    {
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Customer> Customers => Set<Customer>();

        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
        {
        }
    }
}
