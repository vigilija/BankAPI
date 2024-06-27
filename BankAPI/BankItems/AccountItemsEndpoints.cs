using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.BankItems
{
    public class AccountItemsEndpoints
    {
        public static void RegisterAccountItemsEndpoints(WebApplication app)
        {
            var accountItems = app.MapGroup("/AccountItems");

            accountItems.MapGet("/", GetAccounts);
            accountItems.MapGet("/owner/{id}", GetAccountsByOwnerId);
            accountItems.MapGet("/{id}", GetAccountById);
            accountItems.MapPost("/", CreateAccount);
            accountItems.MapPost("/default", CreateDefaultAccounts);
            accountItems.MapPut("/{id}", UpdateAccount);
        }
        static async Task<IResult> GetAccounts(BankDbContext db)
        {
            return TypedResults.Ok(await db.Accounts.Select(x => new AccountItemDTO(x)).ToListAsync());
        }

        // Adjusted: Removed [FromBody]


        static async Task<IResult> GetAccountsByOwnerId(long id, BankDbContext db)
        {
            return TypedResults.Ok(await db.Accounts.Where(t => t.AccountOwnerId == id).Select(x => new AccountItemDTO(x)).ToListAsync());
        }

        static async Task<IResult> GetAccountById(long id, BankDbContext db)
        {
            return await db.Accounts.FindAsync(id)
               is Account account
                   ? TypedResults.Ok(account)
                   : TypedResults.NotFound();

        }



        static async Task<IResult> CreateAccount([FromBody] AccountItemDTO account, BankDbContext db)
        {
            // Check if the owner exists
            var ownerExists = await db.Customers.AnyAsync(o => o.Id == account.AccountOwnerId);
            if (!ownerExists)
            {
                return TypedResults.BadRequest("Owner does not exist");
            }

            var accountItem = new Account
            {
                AccountNumber = account.AccountNumber,
                AccountOwnerId = account.AccountOwnerId,
                Balance = account.Balance,
                AccountType = account.AccountType
            };
            db.Accounts.Add(accountItem);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/accountitem/{account.Id}", account);
        }



        static async Task<IResult> CreateDefaultAccounts(BankDbContext db)
        {
            var account = new Account
            {
                Id = 1,
                AccountNumber = "1234567890",
                AccountOwnerId = 1,
                OpenDate = new DateTime(2021, 10, 9),
                Balance = 1000.00M,
                AccountType = "Checking"
            };
            var account2 = new Account
            {
                Id = 2,
                AccountNumber = "1234567890",
                AccountOwnerId = 1,
                OpenDate = new DateTime(2021, 10, 9),
                Balance = 1000.00M,
                AccountType = "Checking"
            };
            var account3 = new Account
            {
                Id = 3,
                AccountNumber = "1234567890",
                AccountOwnerId = 1,
                OpenDate = new DateTime(2021, 10, 9),
                Balance = 1000.00M,
                AccountType = "Checking"
            };
            db.Accounts.Add(account);
            db.Accounts.Add(account2);
            db.Accounts.Add(account3);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/accountitem/{account.Id}, {account2.Id}, {account3.Id}", account);
        }

        static async Task<IResult> UpdateAccount(int id, AccountItemDTO inputAccount, BankDbContext db)
        {
            var account = await db.Accounts.FindAsync(id);
            if (account is null) return TypedResults.NotFound();
            account.AccountNumber = inputAccount.AccountNumber;
            account.AccountOwnerId = inputAccount.AccountOwnerId;
            await db.SaveChangesAsync();
            return TypedResults.NoContent();

            // Add the following method to handle the update customer request

        }
    }
}
