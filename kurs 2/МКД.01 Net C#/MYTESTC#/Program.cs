public void Println(dynamic obj) => Console.WriteLine(obj);
public string Input(dynamic text)
{
    Console.WriteLine(text);
    return Console.ReadLine();
}



// You test

Println("Hello");

public delegate void AccountHandler(string message);
public class Account
{
    int sum;
    AccountHandler? taken;
    public void RegisterHandler(AccountHandler del) =>
        taken += del;
    public void UnregisterHandler(AccountHandler del) =>
        taken -= del;
    public void Add(int sum) => this.sum += sum;
    public void Take(int sum)
    {
        if (this.sum > sum)
        {
            this.sum -= sum;
            Println($"Console");
        }
    }
}