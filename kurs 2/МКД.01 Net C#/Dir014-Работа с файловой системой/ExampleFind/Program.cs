using System.Text.RegularExpressions;

void Println(dynamic obj)
{
    Console.WriteLine(obj.ToString());
}

void Print(dynamic obj)
{
    Console.Write(obj.ToString());
}

string Input(dynamic obj = null)
{
    if (obj != null)
        Console.Write(obj.ToString());
    return Console.ReadLine();
}

void AdditionToList(ref List<string> listMain, List<string> list)
{
    foreach (string el in list)
        listMain.Add(el);
}

Println("Программа для поиска файла");

// Получить директорию
string dir = "";
do dir = Input("Введите существующий каталог: ");
while (!Directory.Exists(dir));

// Получение всех файлов в подкаталогах
List<string> pathsAll = new List<string>();
void AddAllFiles(string dir)
{
    List<string> files = Directory.GetFiles(dir).ToList();
    AdditionToList(ref pathsAll, files);
    List<string> dirs = Directory.GetDirectories(dir).ToList();
    foreach (string d in dirs)
        AddAllFiles(d);
}
AddAllFiles(dir);

// Фильтр
var format = "." + Input("Введите расширение файла: ");
var pathsResult = pathsAll.Where(p => new FileInfo(p).Extension == format).ToList();

while (true)
{
    // Получение имени файла
    var name = Input("Введите название файла: ");
    bool isHave = false;
    foreach (string path in pathsResult)
        if (Regex.IsMatch(path, $"{name}.(txt|png|doc)"))
        {
            isHave = true;
            break;
        }
    if (isHave)
        Println("Файл существует");
    else
        Println("Файл не существует");
}