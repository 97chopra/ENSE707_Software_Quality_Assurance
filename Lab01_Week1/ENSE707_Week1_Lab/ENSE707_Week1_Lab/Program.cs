// See https://aka.ms/new-console-template for more information

using ENSE707_Week1_Lab;

BankAccount account = new BankAccount("Student User", 100);

account.Deposit(50);
account.Withdraw(30);

Console.WriteLine($"Account Holder: {account.AccountHolder}");
Console.WriteLine($"Current Balance: {account.Balance}");
Console.WriteLine($"Fee on $100 transaction: {account.CalculateTransactionFee(100)}");