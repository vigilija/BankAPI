using BankAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BankDbContext>(options =>
    options.UseInMemoryDatabase("BankDB"));

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "BankAPI";
    config.Title = "BankAPI v1";
    config.Version = "v1";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

var accountItems = app.MapGroup("/AccountItems");

accountItems.MapGet("/", GetAccounts);
accountItems.MapGet("/owner/{id}", GetAccountsByOwnerId);
accountItems.MapGet("/{id}", GetAccountById);
accountItems.MapPost("/", CreateAccount);
accountItems.MapPost("/default", CreateDefaultAccounts);
accountItems.MapPut("/{id}", UpdateAccount);

var ownerItems = app.MapGroup("/OwnerItems");

ownerItems.MapGet("/", GetCustomer);
ownerItems.MapGet("/{id}", GetCustomerById);
ownerItems.MapPost("/", CreateCustomer);
ownerItems.MapPut("/{id}", UpdateCustomer);

app.Run();

// Adjusted: Removed [FromBody]
static async Task<IResult> GetAccounts(BankDbContext db)
{
    return TypedResults.Ok(await db.Accounts.Select(x => new AccountItemDTO(x)).ToListAsync());
}

// Adjusted: Removed [FromBody]
static async Task<IResult> GetCustomer(BankDbContext db)
{
    return TypedResults.Ok(await db.Customers.ToListAsync());
}

static async Task<IResult> GetAccountsByOwnerId( long id, BankDbContext db)
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

static async Task<IResult> GetCustomerById(long id, BankDbContext db)
{
    return await db.Customers.FindAsync(id)
       is Customer owner
           ? TypedResults.Ok(owner)
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

static async Task<IResult> CreateCustomer([FromBody]CustomerItemDTO owner, BankDbContext db)
{
    var ownerItem = new Customer
    {
        Id = 2,
        Name = owner.Name,
        PersonalCode = owner.PersonalCode,
        Address = owner.Address,
        Email = owner.Email
    };
    db.Customers.Add(ownerItem);
    await db.SaveChangesAsync();
    return TypedResults.Created($"/owneritem/{ownerItem.Id}", ownerItem);
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
static async Task<IResult> UpdateCustomer(long id, CustomerItemDTO updatedOwner, BankDbContext db)
{
    var owner = await db.Customers.FindAsync(id);
    if (owner is null) return TypedResults.NotFound();

    owner.Name = updatedOwner.Name;
    owner.Address = updatedOwner.Address;
    owner.Email = updatedOwner.Email;

    await db.SaveChangesAsync();
    return TypedResults.NoContent();
}

// Add the following code inside the Program class


