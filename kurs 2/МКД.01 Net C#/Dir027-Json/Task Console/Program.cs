using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class Meme
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public string SemanticLoad { get; set; }
    public List<int> Ratings { get; set; }

    public Meme()
    {
        CreatedDate = DateTime.Now;
        Ratings = new List<int>();
    }
}

public class MemeManagerConsole
{
    private string _currentFile;
    private List<Meme> _memes;
    private const string Options = "1. Создать мем\n2. Редактировать мем\n3. Удалить мем\n4. Добавить оценку\n5. Вывести все мемы\n0. Выход";

    public MemeManagerConsole(string path)
    {
        _currentFile = path;
        _memes = ReadFromFile();
    }

    public void SaveToFile()
    {
        string json = JsonSerializer.Serialize(_memes, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_currentFile, json);
    }

    public List<Meme> ReadFromFile()
    {
        if (!File.Exists(_currentFile))
            return new List<Meme>();

        string json = File.ReadAllText(_currentFile);
        return JsonSerializer.Deserialize<List<Meme>>(json) ?? new List<Meme>();
    }

    public string GetAllMemesString()
    {
        if (_memes.Count == 0)
            return "Список мемов пуст.";

        string result = "";
        foreach (var meme in _memes)
        {
            result += $"ID: {meme.Id}\nНазвание: {meme.Name}\nОписание: {meme.Description}\n" +
                     $"Дата создания: {meme.CreatedDate}\nСмысловая нагрузка: {meme.SemanticLoad}\n" +
                     $"Оценки: {(meme.Ratings.Count > 0 ? string.Join(", ", meme.Ratings) : "нет оценок")}\n" +
                     $"Средняя оценка: {(meme.Ratings.Count > 0 ? Math.Round(meme.Ratings.Average(), 2) : 0)}\n" +
                     new string('-', 50) + "\n";
        }
        return result;
    }

    private void RefreshAndDisplay()
    {
        SaveToFile();
        _memes = ReadFromFile();
        Console.WriteLine(GetAllMemesString());
    }

    public void CreateMeme()
    {
        try
        {
            Console.Write("Введите название мема: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Название не может быть пустым");

            Console.Write("Введите описание мема: ");
            string description = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(description)) throw new Exception("Описание не может быть пустым");

            Console.Write("Введите смысловую нагрузку: ");
            string semanticLoad = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(semanticLoad)) throw new Exception("Смысловая нагрузка не может быть пустой");

            int newId = _memes.Count > 0 ? _memes.Max(m => m.Id) + 1 : 1;

            Meme newMeme = new Meme
            {
                Id = newId,
                Name = name,
                Description = description,
                SemanticLoad = semanticLoad
            };

            _memes.Add(newMeme);
            RefreshAndDisplay();
            Console.WriteLine($"Мем с ID {newId} успешно создан!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}. Изменения не сохранены.");
        }
    }

    public void EditMeme()
    {
        try
        {
            Console.Write("Введите ID мема для редактирования: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
                throw new Exception("Неверный формат ID");

            Meme meme = _memes.FirstOrDefault(m => m.Id == id);
            if (meme == null) throw new Exception("Мем с таким ID не найден");

            Console.Write($"Введите новое название (было: {meme.Name}): ");
            string name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name)) meme.Name = name;

            Console.Write($"Введите новое описание (было: {meme.Description}): ");
            string description = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(description)) meme.Description = description;

            Console.Write($"Введите новую смысловую нагрузку (была: {meme.SemanticLoad}): ");
            string semanticLoad = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(semanticLoad)) meme.SemanticLoad = semanticLoad;

            RefreshAndDisplay();
            Console.WriteLine($"Мем с ID {id} успешно отредактирован!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}. Изменения не сохранены.");
        }
    }

    public void DeleteMeme()
    {
        try
        {
            Console.Write("Введите ID мема для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
                throw new Exception("Неверный формат ID");

            Meme meme = _memes.FirstOrDefault(m => m.Id == id);
            if (meme == null) throw new Exception("Мем с таким ID не найден");

            _memes.Remove(meme);
            RefreshAndDisplay();
            Console.WriteLine($"Мем с ID {id} успешно удалён!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}. Изменения не сохранены.");
        }
    }

    public void AddRating()
    {
        try
        {
            Console.Write("Введите ID мема для добавления оценки: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
                throw new Exception("Неверный формат ID");

            Meme meme = _memes.FirstOrDefault(m => m.Id == id);
            if (meme == null) throw new Exception("Мем с таким ID не найден");

            Console.Write("Введите оценку (от 1 до 10): ");
            if (!int.TryParse(Console.ReadLine(), out int rating) || rating < 1 || rating > 10)
                throw new Exception("Оценка должна быть целым числом от 1 до 10");

            meme.Ratings.Add(rating);
            RefreshAndDisplay();
            Console.WriteLine($"Оценка {rating} добавлена мему {meme.Name}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}. Изменения не сохранены.");
        }
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine("\n" + Options);
            Console.Write("Выберите действие: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateMeme();
                    break;
                case "2":
                    EditMeme();
                    break;
                case "3":
                    DeleteMeme();
                    break;
                case "4":
                    AddRating();
                    break;
                case "5":
                    Console.WriteLine(GetAllMemesString());
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }
}

class Program
{
    static void Main()
    {
        MemeManagerConsole manager = new MemeManagerConsole("memes.json");
        manager.Run();
    }
}