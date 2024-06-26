using BankAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NSwag.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AccountDB>(options =>
    options.UseInMemoryDatabase("AccountList"));

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TodoAPI";
    config.Title = "TodoAPI v1";
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
app.MapPost("/accountitem", CreateAccount);
accountItems.MapPost("/default", CreateDefaultAccounts);
accountItems.MapPut("/{id}", UpdateAccount);

var ownerItems = app.MapGroup("/OwnerItems");

ownerItems.MapGet("/", GetOwners);
ownerItems.MapGet("/{id}", GetOwnerById);
ownerItems.MapPost("/", CreateOwner);

app.Run();

static async Task<IResult> GetAccounts(HttpContext context, AccountDB db)
{
    return TypedResults.Ok(await db.Accounts.Select(x => new AccountItemDTO(x)).ToListAsync());
}

static async Task<IResult> GetOwners(HttpContext context, OwnerDB db)
{
    return TypedResults.Ok(await db.Owners.ToListAsync());
}

static async Task<IResult> GetAccountsByOwnerId(HttpContext context, long id, AccountDB db)
{
    return TypedResults.Ok(await db.Accounts.Where(t => t.AccountOwnerId == id).Select(x => new AccountItemDTO(x)).ToListAsync());
}

static async Task<IResult> GetAccountById(long id, AccountDB db)
{
    return await db.Accounts.FindAsync(id)
       is Account account
           ? TypedResults.Ok(account)
           : TypedResults.NotFound();

}

static async Task<IResult> GetOwnerById(long id, OwnerDB db)
    {
        return await db.Owners.FindAsync(id)
           is Owner owner
               ? TypedResults.Ok(owner)
               : TypedResults.NotFound();
    }

static async Task<IResult> CreateAccount( AccountItemDTO account, AccountDB db)
{
    var accountItem = new Account
    {
        AccountNumber = account.AccountNumber,
        AccountOwnerId = account.AccountOwnerId,
        OpenDate = account.OpenDate,
        Balance = account.Balance,
        AccountType = account.AccountType
    };
    db.Accounts.Add(accountItem);
    await db.SaveChangesAsync();
    return TypedResults.Created($"/accountitem/{account.Id}", account);
}

static async Task<IResult> CreateOwner(OwnerDTO owner, OwnerDB db)
    {
        var ownerItem = new Owner
        {
            Id = 2,
            Name = owner.Name,
            PersonalCode = owner.PersonalCode,
            Address = owner.Address,
            Email = owner.Email
        };
        db.Owners.Add(ownerItem);
        await db.SaveChangesAsync();
        return TypedResults.Created($"/owneritem/{ownerItem.Id}", ownerItem);
    }

static async Task<IResult> CreateDefaultAccounts( AccountDB db)
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

static async Task<IResult> UpdateAccount( int id, AccountItemDTO inputAccount, AccountDB db)
{
    var account = await db.Accounts.FindAsync(id);
    if (account is null) return TypedResults.NotFound();
    account.AccountNumber = inputAccount.AccountNumber;
    account.AccountOwnerId = inputAccount.AccountOwnerId;
    await db.SaveChangesAsync();
    return TypedResults.NoContent();   

}



