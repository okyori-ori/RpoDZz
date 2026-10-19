using System;

abstract class BankAccount
{
    public string AccountNumber;
    public int Balance;

    protected BankAccount(string accountNumber, int balance)
    {
        AccountNumber = accountNumber;
        Balance = balance;
    }

    public void Deposit(int amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Сумма пополнения должна быть положительной.");
            return;
        }
        Balance += amount;
        Console.WriteLine($"Счёт {AccountNumber} пополнен на {amount}.");
    }

    public abstract void Withdraw(int amount);

    public void DisplayBalance()
    {
        Console.WriteLine($"Счёт: {AccountNumber}, баланс: {Balance}");
    }
}

class SavingsAccount : BankAccount
{
    public int MinimumBalance;

    public SavingsAccount(string number, int balance, int minimumBalance) : base(number, balance)
    {
        MinimumBalance = minimumBalance;
    }

    public override void Withdraw(int amount)
    {
        if (amount <= 0 || (long)Balance - amount < MinimumBalance)
        {
            Console.WriteLine("Снятие запрещено: нарушается минимальный остаток или сумма некорректна.");
            return;
        }
        Balance -= amount;
        Console.WriteLine($"Снято: {amount}");
    }
}

class CheckingAccount : BankAccount
{
    public int Commission;

    public CheckingAccount(string number, int balance, int commission) : base(number, balance)
    {
        Commission = commission;
    }

    public override void Withdraw(int amount)
    {
        if (amount <= 0 || (long)amount + Commission > Balance)
        {
            Console.WriteLine("Недостаточно средств с учётом комиссии или сумма некорректна.");
            return;
        }
        Balance -= amount + Commission;
        Console.WriteLine($"Снято: {amount}, комиссия: {Commission}");
    }
}

class CreditAccount : BankAccount
{
    public int CreditLimit;

    public CreditAccount(string number, int balance, int creditLimit) : base(number, balance)
    {
        CreditLimit = creditLimit;
    }

    public override void Withdraw(int amount)
    {
        if (amount <= 0 || (long)Balance - amount < -CreditLimit)
        {
            Console.WriteLine("Превышен кредитный лимит или сумма некорректна.");
            return;
        }
        Balance -= amount;
        Console.WriteLine($"Снято: {amount}");
    }
}

class Program
{
    static void Main()
    {
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        SavingsAccount savings = new SavingsAccount("1001", 1000, 500);
        savings.Deposit(500);
        savings.Withdraw(1000);
        savings.DisplayBalance();
        savings.Withdraw(1);
        savings.DisplayBalance();

        CheckingAccount checking = new CheckingAccount("1002", 1000, 50);
        checking.Deposit(500);
        checking.Withdraw(1000);
        checking.DisplayBalance();
        checking.Withdraw(450);
        checking.DisplayBalance();

        CreditAccount credit = new CreditAccount("1003", 0, 1000);
        credit.Deposit(200);
        credit.Withdraw(1200);
        credit.DisplayBalance();
        credit.Withdraw(1);
        credit.DisplayBalance();
    }
}
