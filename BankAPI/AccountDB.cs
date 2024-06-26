using Microsoft.EntityFrameworkCore;

namespace BankAPI
{
    public class AccountDB : DbContext
    {
        public DbSet<Account> Accounts => Set<Account>();

        public AccountDB(DbContextOptions<AccountDB> options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{
        //    options.UseSqlite("Data Source=bank.db");
        //}
    }
}
