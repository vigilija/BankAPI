namespace BankAPI
{
    public class AccountItemDTO
    {
        public long Id { get; set; }
        public string AccountNumber { get; set; }
        public long AccountOwnerId { get; set; }
        public DateTime OpenDate { get; set; } = DateTime.Now;
        public decimal Balance { get; set; }
        public string AccountType { get; set; }

        public AccountItemDTO() { }

        public AccountItemDTO(Account account) =>
            (
                Id,
                AccountNumber,
                AccountOwnerId,
                OpenDate,
                Balance,
                AccountType
            ) =
            (
                account.Id,
                account.AccountNumber,
                account.AccountOwnerId, 
                account.OpenDate, 
                account.Balance, 
                account.AccountType
            );

    }
}
