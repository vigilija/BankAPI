using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.BankItems
{
    public static class CustomerItemsEndpoints
    {
        public static void RegisterCustomerItemsEndpoints(this WebApplication app)
        {
            var ownerItems = app.MapGroup("/OwnerItems");

            ownerItems.MapGet("/", GetCustomer);
            ownerItems.MapGet("/{id}", GetCustomerById);
            ownerItems.MapPost("/", CreateCustomer);
            ownerItems.MapPut("/{id}", UpdateCustomer);
        }
        static async Task<IResult> GetCustomer(BankDbContext db)
        {
            return TypedResults.Ok(await db.Customers.ToListAsync());
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
        static async Task<IResult> CreateCustomer([FromBody] CustomerItemDTO owner, BankDbContext db)
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

        static async Task<IResult> GetCustomerById(long id, BankDbContext db)
        {
            return await db.Customers.FindAsync(id)
               is Customer owner
                   ? TypedResults.Ok(owner)
                   : TypedResults.NotFound();
        }
    }
}
