using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace AuthAndReg;

public static class Data
{
    private const string FilePath = "users.json";

    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static ObservableCollection<User> Users { get; set; } = new();

    public static void Save()
    {
        string json = JsonSerializer.Serialize(Users, Options);
        File.WriteAllText(FilePath, json, System.Text.Encoding.UTF8);
    }

    public static void Load()
    {
        if (!File.Exists(FilePath))
        {
            Users = new ObservableCollection<User>();
            return;
        }

        string json = File.ReadAllText(FilePath);
        var loaded = JsonSerializer.Deserialize<ObservableCollection<User>>(json);
        Users = loaded ?? new ObservableCollection<User>();
    }
}