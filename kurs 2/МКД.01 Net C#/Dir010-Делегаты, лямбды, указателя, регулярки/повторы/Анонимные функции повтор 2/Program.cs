Console.WriteLine("Hello, World!");
int g = 3;
Gg gg = delegate
{
    Console.WriteLine("Hello, Hehe!");
    Console.WriteLine(g);
};
gg("Awdawd");

delegate void Gg(string hehehe);