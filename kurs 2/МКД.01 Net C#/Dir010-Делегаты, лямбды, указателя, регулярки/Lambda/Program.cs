Console.WriteLine("Hello, World!");

mydDelegat sum = (byte e) => e < 3;

byte[] sort(byte[] array, mydDelegat m)
{
    List<byte> list = new List<byte>();
    foreach (byte b in array)
        if (m(b))
            list.Add(b);
    return list.ToArray();
}

byte[] bytes = { 1,6,2,9,45,2};
var r = sort(bytes, sum);
foreach (byte b in r)
    Console.WriteLine(b);

Operation Iii(string a)
{
    switch (a)
    {
        case "sum":
            return (a, b) => a + b;
            break;
        case "sub":
            return (a, b) => a - b;
            break;
        case "mul":
            return (a, b) => a * b;
            break;
        case "del":
            return (a, b) => a / b;
            break;
    }
    return (a, b) => 0;
}

Console.WriteLine(Iii("sub")(5,3));

delegate bool mydDelegat(byte e);

delegate int Operation(int a, int b);