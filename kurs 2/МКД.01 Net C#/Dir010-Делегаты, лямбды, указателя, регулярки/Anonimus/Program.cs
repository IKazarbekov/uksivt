Console.WriteLine("Hello, World!");

int c = 34;
Handler handler;
handler = null;
handler += delegate {
    Console.WriteLine(c);
};

handler(2);

delegate void Handler(int a);