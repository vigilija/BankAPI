namespace BankAPI;

public class Account
{
    public int Id { get; set; }
    public long AcountNumber { get; set; }
    public bool AccountOwnerId { get; set; }
    public DateOnly OpenDate { get; set; }
}
