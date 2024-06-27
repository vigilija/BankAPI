using System.Runtime.CompilerServices;

namespace BankAPI.BankItems
{
    public static class BankItemsEndpoints
    {
        public static void RegisterBankItemsEndpoints(this WebApplication app)
        {
            app.MapGet("/accounts", (BankDbContext dbContext) =>
            {
                return dbContext.Accounts.ToList();
            });
            app.MapGet("/accounts/{id}", (BankDbContext dbContext, long id) =>
            {
                return dbContext.Accounts.Find(id);
            });
            app.MapGet("/customers", (BankDbContext dbContext) =>
            {
                return dbContext.Customers.ToList();
            });
        }
    }
}
