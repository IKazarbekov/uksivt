using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.Json;
using Попытка_13.models;

namespace Попытка_13
{
    public class Data
    {
        public static ObservableCollection<User> Users { get; set; }
        public static ObservableCollection<Brone> Brones { get; set; }
        public static ObservableCollection<Lesson> Lessons { get; set; }

        public static void Read()
        {
            if (!File.Exists("user.json"))
            {
                Users = new ObservableCollection<User>();
                Brones = new ObservableCollection<Brone>();
                Lessons = new ObservableCollection<Lesson>();
                return;
            }

            Users = JsonSerializer.Deserialize<ObservableCollection<User>>(File.ReadAllText("user.json"));
            Brones = JsonSerializer.Deserialize<ObservableCollection<Brone>>(File.ReadAllText("brone.json"));
            Lessons = JsonSerializer.Deserialize<ObservableCollection<Lesson>>(File.ReadAllText("lesson.json"));
        }

        public static void Write()
        {
            File.WriteAllText("user.json", JsonSerializer.Serialize(Users, new JsonSerializerOptions() { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
            File.WriteAllText("brone.json", JsonSerializer.Serialize(Brones, new JsonSerializerOptions() { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
            File.WriteAllText("lesson.json", JsonSerializer.Serialize(Lessons, new JsonSerializerOptions() { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
        }
    }
}
