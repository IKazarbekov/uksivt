using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Билет0_Попытка_1.models;

namespace Билет0_Попытка_1.data
{
    internal static class Data
    {
        public static List<User> users { get; set; }
        public static void _ClearData()
        {
            users = new List<User>()
            {
                new User(){Id=0, Login="bob", Password="87654321", Phone="79953452483", Role=Role.Admin},
                new User(){Id=1, Login="tom", Password="12345678", Phone="79953756283", Role=Role.Client}
            };
        }

        public static void LoadAllData()
        {
            users = Load<User>(Config.PATH_USER);
        }
        public static void SaveAllData()
        {
            Save<User>(users, Config.PATH_USER);
        }

        private static void Save<T>(List<T> list, string path)
        {
            string json = JsonSerializer.Serialize(list, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            File.WriteAllText(path, json);
        }

        private static List<T> Load<T>(string path)
        {
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<T>>(json);
        }
    }
}
