using System.ComponentModel.DataAnnotations;

namespace BankAPI;

public class Account
{
    [Key]
    public long Id { get; set; }

    public string AccountNumber { get; set; }
    [Required]
    public long AccountOwnerId { get; set; }
    [Required]
    public DateTime OpenDate { get; set; }
    [Required]
    public decimal Balance { get; set; }
    [Required]
    public string AccountType { get; set; }

    public void AddBalance(decimal amount)
    {
        // Ensure the amount is not negative
        if (amount < 0) throw new ArgumentException("Amount must be positive", nameof(amount));

        Balance += amount;
    }
}

