Console.WriteLine("Hello, World!");

var client = new Client(34);
client.MyHandler += (h, e) => Console.WriteLine("Awd");

public class Client
{

    public delegate void Handler(object sender, ClientEventArgs e);
    event Handler myEvent;
    public event Handler MyHandler { add { myEvent += value; } remove { } }

    int sum;
    public Client(int sum)
    {
        this.sum = sum;
    }

    public void Add(int x)
    {
        sum += x;
        myEvent(this, new ClientEventArgs(x));
    }
}

public class ClientEventArgs : EventArgs
{
    int sum;
    public ClientEventArgs(int sum)
    {
        this.sum = sum;
    }
}




