Client client = new Client();
client.Handler += (o, e) => Console.WriteLine(e.message);
client.Run();

class Client
{
    public event Handler handler;
    public event Handler Handler { add
        {
            handler += value;
            Console.WriteLine("34");
        }
        remove
        {
            handler -= value;
        }
    }
    public void Run()
    {
        handler.Invoke(this, new EventArgsClient("Heheh"));
    }
}

class EventArgsClient
{
    public string message;
    public EventArgsClient(string message)
    {
        this.message = message;
    }
}

delegate void Handler(object sender, EventArgsClient e);