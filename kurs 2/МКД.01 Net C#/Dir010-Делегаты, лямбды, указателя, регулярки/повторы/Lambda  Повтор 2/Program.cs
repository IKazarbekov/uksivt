Console.WriteLine("Hello, World!");

Func<int, int> fun = (int a) => a * 5;

Console.WriteLine(fun(23));

void Hehe(Func<int, int> func)
{
    for (int i = 0; i < 10; i++)
        Console.WriteLine(func(i));
}

Hehe(fun);

Func<int, int> Hoho()
{
    return fun;
}

Console.WriteLine(Hoho());