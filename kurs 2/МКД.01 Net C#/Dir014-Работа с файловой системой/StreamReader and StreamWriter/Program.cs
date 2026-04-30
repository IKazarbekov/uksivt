string Input(string text = null)
{
    if (text != null)
        Console.Write(text);
    Console.Write(" > ");
    return Console.ReadLine();
}

void Println(dynamic text)
{
    Console.WriteLine(text);
}

void Print(dynamic text)
{
    Console.Write(text);
}

bool FileExists(string path)
{
    try
    {
        new StreamReader(path).Close();
        return true;
    }
    catch
    {
        return false;
    }
}

Println("Консольная программа для работы только с StreamReader и StreamWriter");
while (true)
{
    string path = "";
    Println("0) Выход");
    Println("1) Чтение файла");
    Println("2) Запись файла");
    string command = Input();
    if (command == "0")
        break;
    else if (command == "1")
    {
        Println("1) Проверить существование файла");
        Println("2) Прочитать файл целиком");
        Println("3) Прочитать строку файла");
        Println("4) Прочитать нужно кол-во символов файла");
        command = Input();

        switch (command)
        {
            case "1":
                path = Input("Файл: ");
                try
                {
                    new StreamReader(path).Close();
                    Println("Файл существует");
                }
                catch
                {
                    Println("Файл не найден");
                }
                break;
            case "2":
                path = Input("Файл: ");
                if (!FileExists(path))
                {
                    Println("Такого файла не существует");
                    continue;
                }
                using (var sr = new StreamReader(path))
                    Println(sr.ReadToEnd());
                break;
            case "3":
                {
                    path = Input("Файл: ");
                    if (!FileExists(path))
                    {
                        Println("Такого файла не существует");
                        continue;
                    }
                    int index = int.Parse(Input("Номер строки:"));
                    try
                    {
                        string str = "";
                        using (var sr = new StreamReader(path))
                            for (int i = 0; i < index; i++)
                            {
                                str = sr.ReadLine();
                            }
                        Println(str);
                    }
                    catch
                    {
                        Println("Строки нету");
                    }
                break;
                }
            case "4":
                {
                    path = Input("Файл: ");
                    if (!FileExists(path))
                    {
                        Println("Такого файла не существует");
                        continue;
                    }
                    int count = int.Parse(Input("Кол-во символов"));
                    char[] chars = new char[count];
                    using (var sr = new StreamReader(path))
                        if (sr.Read(chars, 0, count) < count)
                            Println("Кол-во символов оказалось меньше");
                    Println(new string(chars));
                    break;
                }
        }
    }
    else if (command == "2")
    {
        Println("1) Создать файл");
        Println("2) Записать строку в конец файла");
        Println("3) Вставить строку в файл по его номеру");
        Println("4) Удалить строку");
        Println("5) Перезаписать весь файл");
        command = Input();

        switch (command)
        {
            case "1":
                path = Input("Файл: ");
                new StreamWriter(path).Close();
                Println("Файл создан");
                break;
            case "2":
                path = Input("Файл: ");
                string str = Input("Строка для добавления в конец: ");
                using (var sw = new StreamWriter(path, true))
                    sw.WriteLine(str);
                Println("Строка добавлена");
                break;
            case "3":
                {
                    path = Input("Файл: ");
                    if (!FileExists(path))
                    {
                        Println("Такого файла не существует");
                        continue;
                    }
                    var text = new List<string>();
                    using (var sr = new StreamReader(path))
                        while (sr.Peek() > 0)
                            text.Add(sr.ReadLine());
                    int index = int.Parse(Input("Номер строки: "));
                    if (index >= text.Count())
                    {
                        Println("Такой строки не существует в файле");
                        continue;
                    }
                    str = Input("Строка для вставки: ");
                    text.Insert(index, str);
                    using (var sw = new StreamWriter(path))
                        foreach (string s in text)
                            sw.WriteLine(s);
                    Println("Строка вставлена");
                    break;
                }
            case "4":
                {
                    path = Input("Файл: ");
                    if (!FileExists(path))
                    {
                        Println("Такого файла не существует");
                        continue;
                    }
                    var text = new List<string>();
                    using (var sr = new StreamReader(path))
                        while (sr.Peek() > 0)
                            text.Add(sr.ReadLine());
                    int index = int.Parse(Input("Номер cтроки для удаления: "));
                    if (index >= text.Count())
                    {
                        Println("Такой строки не существует в файле");
                        continue;
                    }
                    text.RemoveAt(index);
                    using (var sw = new StreamWriter(path))
                        foreach (string s in text)
                            sw.WriteLine(s);
                    Println("Строка удалена");
                    break;
                }
            case "5":
                {
                    path = Input("Файл: ");
                    if (!FileExists(path))
                    {
                        Println("Такого файла не существует");
                        continue;
                    }
                    Println("Вводите текст, когда будет готово напишите ГОТОВО");
                    var text = new List<string>();
                    while (true)
                    {
                        str = Input();
                        if (str == "ГОТОВО")
                            break;
                        text.Add(str);
                    }
                    using (var sw = new StreamWriter(path))
                        foreach (string s in text)
                            sw.WriteLine(s);
                    Println("Файл перезаписан");
                    break;
                }
        }
    }
}

