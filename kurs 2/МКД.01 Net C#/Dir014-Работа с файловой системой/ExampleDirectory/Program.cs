using System.Security.AccessControl;

string dirName = @"C:\\";

if (Directory.Exists(dirName))
{
    Console.WriteLine("directories");
    var dirs = Directory.GetDirectories(dirName);
    Console.WriteLine(dirs.Length);
    foreach (var dir in dirs)
        Console.WriteLine(dir);

    Console.WriteLine("\n\nfiles");
    var files = Directory.GetFiles(dirName);
    Console.WriteLine(files.Length);
    foreach (var dir in files)
        Console.WriteLine(dir);
}