using System.Text.RegularExpressions;

void work1()
{
    MatchCollection matches = Regex.Matches(Input("Ваш текст: "), @"\d+");
    if (matches.Count > 0)
    {
        foreach (Match match in matches)
        {
            Print($"Число {match.Value} в позиции {match.Index}");
        }
    }
    else
    {
        Print("Чисел не обнаружено");
    }
}

void work2()
{
    bool isMatch = Regex.IsMatch(Input("Введите текст: "), "^abs");
    if (isMatch)

        Print("Есть подстрока начинающая с abs");
    else
        Print("Нет строки начинающая с abs");
}

void work3()
{
    bool isNumber = Regex.IsMatch(Input("Введите номер телефона"), @"\+\d\(\d{3}\)\-\(\d{3}\)\-\(\d{4}\)");
    if (isNumber)
        Print("Это простой номер телефона");
    else
        Print("Это не простой номер телефона");
}

void work4()
{
    string result = Regex.Replace(Input("Введите текст с лишними пробелами"), @" {2,}", @" ");
    Print($"Результат: {result}");
}

void work5()
{
    MatchCollection matches = Regex.Matches(Input("Введите текст"), @"[Aa]\w+");
    if (matches.Count > 0)
    {
        Print("Слова начинающееся с a или A");
        foreach (Match match1 in matches)
        {
            Print(match1.Value);
        }
    }
    else
        Print("Нет слов начинающееся с a или A");
}

    // See https://aka.ms/new-console-template for more information
    while (true)
    {
        switch (Key("Номер задания "))
        {
            case '1':
                work1();
                break;
            case '2':
                work2();
                break;
            case '3':
                work3();
                break;
            case '4':
                work4();
                break;
            case '5':
                work5();
                break;
            case 'e':
                throw new Exception();
            default:
                Print("Нет такой задачи");
                break;
        }
    }

    void Print(dynamic o)
    {
        Console.WriteLine(o);
    }

    string Input(dynamic o = null)
    {
        if (o != null)
            Console.WriteLine(o);
        return Console.ReadLine();
    }

    char Key(dynamic o = null)
    {
        if (o != null)
            Console.Write(o);
        char c = Console.ReadKey().KeyChar;
        Console.WriteLine();
        return c;
    }