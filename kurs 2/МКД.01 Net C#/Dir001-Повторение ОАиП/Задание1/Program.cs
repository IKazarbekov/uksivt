class BankAccount
{
    private string accountNumber;
    private decimal balance;
    private string ownerName;

    public string AccountNumber
    {
        get
        {
            return accountNumber;
        }
        set
        {
            accountNumber = value;
        }
    }

    public decimal Balance
    {
        get
        {
            return balance;
        }
        set
        {
            balance = value;
        }
    }

    public string OwnerName
    {
        get
        {
            return ownerName;
        }
        set
        {
            ownerName = value;
        }
    }

    public void Deposit(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("amount negative");
        }

        balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("amount negative");
        }

        if (balance - amount < 0)
            throw new Exception("small balance");

        balance -= amount;
    }

    public decimal GetBalance()
    {
        return balance;
    }

    public void PrintStatement()
    {
        Console.WriteLine($"Владелец: {ownerName}\nСчёт: {accountNumber} \nБаланс: {balance}");
    }



}

class Program
{
    public static void Main(string[] args)
    {

        BankAccount account = new BankAccount();

        account.OwnerName = "Tom";
        account.AccountNumber = "12345";
        account.Deposit(200);

        while (true) {
            Print("Введите команду 1 - пополнить, 2 - снять, 3 - показать баланс");
            switch (Input())
            {
                case "1":
                    int mountP = int.Parse(Input("Сколько пополнить?:"));
                    account.Deposit(mountP);
                    break;
                case "2":
                    int mountD = int.Parse(Input("Сколько снять?:"));
                    account.Withdraw(mountD);
                    break;
                case "3":
                    Print(account.Balance);
                    break;
            }
        }

        account.PrintStatement();
    }

    public static void Print(dynamic text)
    {
        Console.WriteLine(text);
    }

    public static string Input(string text = "")
    {
        Console.WriteLine($"{text}");
        return Console.ReadLine();
    }
}