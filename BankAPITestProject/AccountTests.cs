using Xunit;
using BankAPI; // Use the correct namespace for your Account class

namespace BankAPITestProject;

public class AccountTests
{
    [Fact]
    public void AddBalance_WithPositiveAmount_UpdatesBalanceCorrectly()
    {
        // Arrange
        var account = new Account
        {
            Id = 1,
            AccountNumber = "1234567890",
            AccountOwnerId = 1,
            OpenDate = DateTime.Now,
            Balance = 1000.00m,
            AccountType = "Checking"
        };
        var amountToAdd = 500.00m;

        // Act
        account.AddBalance(amountToAdd);

        // Assert
        var expectedBalance = 1500.00m;
        Assert.Equal(expectedBalance, account.Balance);
    }
}
