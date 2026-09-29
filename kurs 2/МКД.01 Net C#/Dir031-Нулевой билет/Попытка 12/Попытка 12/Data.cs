using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;
using Попытка_12.models;

namespace Попытка_12
{
    static class Data
    {
        public static ObservableCollection<User> Users { get; set; }
        public static ObservableCollection<Brone> Brones { get; set; }
        public static ObservableCollection<Lesson> Lessons { get; set; }

        public static void ReadData()
        {
            if (File.Exists("user.json") && File.Exists("user.json") && File.Exists("user.json"))
            {
                var json_user = File.ReadAllText("user.json");
                var json_brone = File.ReadAllText("brone.json");
                var json_lesson = File.ReadAllText("lesson.json");

                Users = JsonSerializer.Deserialize<ObservableCollection<User>>(json_user);
                Brones = JsonSerializer.Deserialize<ObservableCollection<Brone>>(json_brone);
                Lessons = JsonSerializer.Deserialize<ObservableCollection<Lesson>>(json_lesson);
            }
            else
            {
                Users = new ObservableCollection<User>();
                Brones  = new ObservableCollection<Brone>();
                Lessons = new ObservableCollection<Lesson>();
            }
        }

        public static void WriteData()
        {
            var json_user = JsonSerializer.Serialize(Users, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            var json_brone = JsonSerializer.Serialize(Brones, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            var json_lesson = JsonSerializer.Serialize(Lessons, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            File.WriteAllText("user.json", json_user);
            File.WriteAllText("lesson.json", json_lesson);
            File.WriteAllText("brone.json", json_brone);
        }
    }
}
