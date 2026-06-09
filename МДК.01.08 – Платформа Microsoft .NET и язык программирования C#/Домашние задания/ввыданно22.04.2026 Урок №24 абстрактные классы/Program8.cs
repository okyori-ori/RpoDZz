using System;

abstract class BankAccount
{

    protected string AccountNumber;
    protected decimal Balance;

    public BankAccount(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public void Deposit(int amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Ошибка: сумма пополнения должна быть больше 0!");
            return;
        }

        Balance += amount;
        Console.WriteLine($"Пополнение на {amount} руб. Успешно выполнено!");
    }

    public abstract void Withdraw(int amount);

    public void DisplayBalance()
    {
        Console.WriteLine($"Счет №{AccountNumber}");
        Console.WriteLine($"Текущий баланс: {Balance} руб.");
    }
}

class SavingsAccount : BankAccount
{
    private decimal minimumBalance;  

    public SavingsAccount(string accountNumber, decimal initialBalance, decimal minBalance)
        : base(accountNumber, initialBalance)  
    {
        minimumBalance = minBalance;
    }

    public override void Withdraw(int amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Ошибка: сумма снятия должна быть больше 0!");
            return;
        }

        if (Balance - amount < minimumBalance)
        {
            Console.WriteLine($"Ошибка: нельзя снять {amount} руб. Баланс не может быть ниже {minimumBalance} руб.");
            Console.WriteLine($"Доступно для снятия: {Balance - minimumBalance} руб.");
            return;
        }

        Balance -= amount;
        Console.WriteLine($"Снятие {amount} руб. Успешно выполнено!");
    }
}

class CheckingAccount : BankAccount
{
    private decimal commission; 

    public CheckingAccount(string accountNumber, decimal initialBalance, decimal comm)
        : base(accountNumber, initialBalance)
    {
        commission = comm;
    }

    public override void Withdraw(int amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Ошибка: сумма снятия должна быть больше 0!");
            return;
        }

        decimal totalToWithdraw = amount + commission;  

        if (Balance < totalToWithdraw)
        {
            Console.WriteLine($"Ошибка: недостаточно средств!");
            Console.WriteLine($"Нужно: {totalToWithdraw} руб. (включая комиссию {commission} руб.)");
            Console.WriteLine($"Доступно: {Balance} руб.");
            return;
        }

        Balance -= totalToWithdraw;
        Console.WriteLine($"Снятие {amount} руб. + комиссия {commission} руб. = {totalToWithdraw} руб.");
        Console.WriteLine($"Успешно выполнено!");
    }
}

class CreditAccount : BankAccount
{
    private decimal creditLimit; 

    public CreditAccount(string accountNumber, decimal initialBalance, decimal limit)
        : base(accountNumber, initialBalance)
    {
        creditLimit = limit;
    }

    public override void Withdraw(int amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Ошибка: сумма снятия должна быть больше 0!");
            return;
        }

        decimal minAllowedBalance = -creditLimit; 

        if (Balance - amount < minAllowedBalance)
        {
            Console.WriteLine($"Ошибка: превышен кредитный лимит!");
            Console.WriteLine($"Кредитный лимит: {creditLimit} руб. (можно уйти в минус до {minAllowedBalance} руб.)");
            Console.WriteLine($"Текущий баланс: {Balance} руб.");
            Console.WriteLine($"Максимально доступно: {Balance - minAllowedBalance} руб.");
            return;
        }

        Balance -= amount;

        if (Balance < 0)
        {
            Console.WriteLine($"Снятие {amount} руб. Успешно! (Вы в кредите на {Math.Abs(Balance)} руб.)");
        }
        else
        {
            Console.WriteLine($"Снятие {amount} руб. Успешно выполнено!");
        }
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== БАНКОВСКАЯ СИСТЕМА ===\n");

        Console.WriteLine("--- СБЕРЕГАТЕЛЬНЫЙ СЧЕТ (SavingsAccount) ---");
        Console.WriteLine("Правило: нельзя опускать баланс ниже 1000 руб.\n");

        SavingsAccount savings = new SavingsAccount("SAV-001", 5000, 1000);
        savings.DisplayBalance();

        Console.WriteLine("\nПробуем пополнить на 2000 руб.:");
        savings.Deposit(2000);
        savings.DisplayBalance();

        Console.WriteLine("\nПробуем снять 3000 руб. (должно быть можно):");
        savings.Withdraw(3000);
        savings.DisplayBalance();

        Console.WriteLine("\nПробуем снять 4000 руб. (должно быть НЕЛЬЗЯ, упадёт ниже 1000):");
        savings.Withdraw(4000);
        savings.DisplayBalance();

        Console.WriteLine("\n\n--- РАСЧЕТНЫЙ СЧЕТ (CheckingAccount) ---");
        Console.WriteLine("Правило: при снятии фиксированная комиссия 50 руб.\n");

        CheckingAccount checking = new CheckingAccount("CHK-001", 1000, 50);
        checking.DisplayBalance();

        Console.WriteLine("\nПробуем пополнить на 500 руб.:");
        checking.Deposit(500);
        checking.DisplayBalance();

        Console.WriteLine("\nПробуем снять 200 руб. (с комиссией 50 руб. = 250 руб.):");
        checking.Withdraw(200);
        checking.DisplayBalance();

        Console.WriteLine("\nПробуем снять 900 руб. (нужно 950 руб., а есть 1250 руб.):");
        checking.Withdraw(900);
        checking.DisplayBalance();

        Console.WriteLine("\nПробуем снять 400 руб. (нужно 450 руб., а есть 350 руб. - НЕЛЬЗЯ):");
        checking.Withdraw(400);
        checking.DisplayBalance();

        Console.WriteLine("\n\n--- КРЕДИТНЫЙ СЧЕТ (CreditAccount) ---");
        Console.WriteLine("Правило: можно уходить в минус до -5000 руб.\n");

        CreditAccount credit = new CreditAccount("CRD-001", 3000, 5000);
        credit.DisplayBalance();

        Console.WriteLine("\nПробуем пополнить на 1000 руб.:");
        credit.Deposit(1000);
        credit.DisplayBalance();

        Console.WriteLine("\nПробуем снять 2000 руб.:");
        credit.Withdraw(2000);
        credit.DisplayBalance();

        Console.WriteLine("\nПробуем снять 3000 руб. (баланс станет -1000, это в пределах лимита):");
        credit.Withdraw(3000);
        credit.DisplayBalance();

        Console.WriteLine("\nПробуем снять 5000 руб. (баланс станет -6000 - ПРЕВЫШАЕТ ЛИМИТ):");
        credit.Withdraw(5000);
        credit.DisplayBalance();

        Console.WriteLine("\n\nНажмите Enter, чтобы выйти...");
        Console.ReadLine();
    }
}