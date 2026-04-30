Console.WriteLine("Hello, World!");
/*Action Method()
{
    int x = 0;
    void MyMethod()
    {
        Console.WriteLine(x++);
    }
    return MyMethod;
}

Action method =  Method();
method();method();method();method();method();

*/
/*
unsafe
{
    int* x;
    int y = 5;
    x = &y;

    Console.WriteLine(*x);
    y = 23;
    Console.WriteLine(*x);
    *x = 253;
    Console.WriteLine(y);
}*/

unsafe
{
    int a = 50;
    int y = 10;
    int* x = &y;
    int** z = &x;

    y = 20;
    Console.WriteLine(*x);
    Console.WriteLine(**z);

    x = &a;
    Console.WriteLine(*x);
    Console.WriteLine(**z);
}