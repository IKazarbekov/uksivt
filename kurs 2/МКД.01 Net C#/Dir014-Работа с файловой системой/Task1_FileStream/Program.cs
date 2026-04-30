using System.Text;

string Input(string text = null)
{
    if (text != null)
        Console.Write(text);
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

while (true)
{
    var text = Input("Command:");
    if (text == "0")
        break;
    switch (text)
    {
        case "1":
            new FileStream("data.txt", FileMode.Create).Close();
            Println("Created file data.txt");
            break;
        case "2":
            var fs = new FileStream("data.txt", FileMode.Truncate);
            var textUser = Input("Что запишите в файл? :");
            byte[] byffer = Encoding.Default.GetBytes(textUser);
            fs.Write(byffer);
            fs.Close();
            break;
        case "3":
            File.Delete("data.txt");
            Println("Удалил файл");
            break;
        case "4":
            using(var fs1 = new FileStream("data.txt", FileMode.Append))
            {
                var textUser1 = "\r\n" + Input("Что запишите в файл? :");
                byte[] buffer = Encoding.Default.GetBytes(textUser1);
                fs1.Write(buffer);
            }
            break;
        case "5":
            new FileStream("data.txt", FileMode.Truncate).Close();
            break;
        case "6":
            if (!File.Exists("data.txt"))
            {
                Println("Файла нет");
                break;
            }
            using(var fs2 = new FileStream("data.txt", FileMode.Open))
            {
                var pos = int.Parse(Input("Position:"));
                fs2.Seek(pos, SeekOrigin.Begin);
                byte[] buffer = new byte[fs2.Length];
                fs2.Read(buffer, 0, buffer.Length);
                string text2 = Encoding.Default.GetString(buffer);
                Println(text2);
            }
            break;
        case "7":
            using (var fs2 = new FileStream("data.txt", FileMode.Open, FileAccess.ReadWrite))
            {
                var pos = int.Parse(Input("Position:"));
                fs2.Seek(pos, SeekOrigin.Begin);
                var textUser1 = "\r\n" + Input("Что запишите в файл? :");
                byte[] buffer = Encoding.Default.GetBytes(textUser1);
                Println("Записал");
            }
            break;
    }
}