using Microsoft.EntityFrameworkCore;

namespace BankAPI
{
    public class OwnerDB : DbContext
    {
        public DbSet<Owner> Owners => Set<Owner>();

        public OwnerDB(DbContextOptions<OwnerDB> options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{
        //    options.UseSqlite("Data Source=bank.db");
        //}
    }
}
