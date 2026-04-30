Console.WriteLine("Hello, World!");

Client client = new Client();
client.handler = Console.WriteLine;
client.Name = "Tom";
Console.WriteLine( client.Mount);
client.Mount = -34;
client.Mount = 23;

public delegate void Handler(string message);
public class Client
{
    public string Name { get; set; }
    private int mount;
    public int Mount
    {
        get
        {
            handler?.Invoke($"Было получено данные баланса у {Name}. Текущий баланс: {mount}");
            return mount;
        }
        set
        {
            if (value < 0)
            {
                handler?.Invoke($"Была попытка присвоить отрицательный баланс у {Name}. Текущий баланс: {mount}");
                return;
            }
            handler?.Invoke($"Изменён баланс у {Name}. Текущий баланс: {value}");
            mount = value;
        }
    }
    public Handler handler { get; set; }
    public Client()
    {
        mount = 1000;
    }
}

