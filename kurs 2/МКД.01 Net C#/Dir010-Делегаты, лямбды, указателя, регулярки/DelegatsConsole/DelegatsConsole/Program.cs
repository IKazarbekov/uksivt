using System.ComponentModel;

void PrintMessage(string message)
{
    Console.WriteLine(message);
}

Client client = new Client();

//client.SetHandler(PrintMessage);
client.Add(3000);
client.Pop(2000);
client.Pop(20);
client.Pop(5000);

public delegate void Handler(string message);

public class Client()
{
    public int Name { get; set; }
    public int Mount { get; set; }
    private Handler handler;

    public void SetHandler(Handler handler)
    {
        this.handler = handler;
    }

    public void Add(int mount)
    {
        Mount += mount;
        handler($"Добавлены деньги {mount}");
    }

    public void Pop(int mount)
    {
        if (Mount < mount)
            handler?.Invoke($"Не достаточно средств чтобы снять {mount} денег");
        else
        {
            Mount -= mount;
            handler?.Invoke($"Средства сняты с карты, {mount} денег");
        }
    }
}


