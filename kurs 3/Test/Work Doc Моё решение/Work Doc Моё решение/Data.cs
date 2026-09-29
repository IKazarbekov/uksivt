using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Work_Doc_Моё_решение
{
    public static class Data
    {
        public static ObservableCollection<User> users = new ObservableCollection<User>();
        public static ObservableCollection<Problem> problems = new ObservableCollection<Problem>();

        public static void Save()
        {
            string textUser = JsonSerializer.Serialize(users, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            File.WriteAllText("users.json", textUser);

            string textProblems = JsonSerializer.Serialize(problems, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            File.WriteAllText("problems.json", textProblems);
        }

        public static void Load()
        {
            string textUser = File.ReadAllText("users.json");
            users = JsonSerializer.Deserialize<ObservableCollection<User>>(textUser);

            string textProblems = File.ReadAllText("problems.json");
            problems = JsonSerializer.Deserialize<ObservableCollection<Problem>>(textProblems);
        }
    }
}
