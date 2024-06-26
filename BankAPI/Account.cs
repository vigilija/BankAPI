namespace BankAPI;

public class Account
{
    public long Id { get; set; }
    public string AccountNumber { get; set; }
    public long AccountOwnerId { get; set; }
    public DateTime OpenDate { get; set; }
    public decimal Balance { get; set; }
    public string AccountType { get; set; }

    public void AddBalance(decimal amount)
    {
        // Ensure the amount is not negative
        if (amount < 0) throw new ArgumentException("Amount must be positive", nameof(amount));

        Balance += amount;
    }
}

