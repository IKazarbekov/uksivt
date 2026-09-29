using PROJECT_ЭКЗАМЕН_1_ПОПЫТКА.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;

namespace PROJECT_ЭКЗАМЕН_1_ПОПЫТКА
{
    class Data
    {
        static public User CurrentUser;

        static public ObservableCollection<User> Users = new ObservableCollection<User>();
        static public ObservableCollection<Lesson> Lessons = new ObservableCollection<Lesson>();
        static public ObservableCollection<Brone> Brones = new ObservableCollection<Brone>();

        const string USER_FILE = "user.json";
        const string BRONE_FILE = "brone.json";
        const string LESSON_FILE = "lesson.json";

        static public void ClearData()
        {
            File.Delete(USER_FILE);
            File.Delete(BRONE_FILE);
            File.Delete(LESSON_FILE);
        }
        static public void ReadOrCreateData()
        {
            if (
                File.Exists(USER_FILE) &&
                File.Exists(BRONE_FILE) &&
                File.Exists(LESSON_FILE)
                )
            {
                string userJson = File.ReadAllText(USER_FILE);
                string broneJson = File.ReadAllText(BRONE_FILE);
                string lessonJson = File.ReadAllText(LESSON_FILE);
                Users = JsonSerializer.Deserialize<ObservableCollection<User>>(userJson);
                Lessons = JsonSerializer.Deserialize<ObservableCollection<Lesson>>(lessonJson);
                Brones = JsonSerializer.Deserialize<ObservableCollection<Brone>>(broneJson);
            }
            else
            {
                Users = new ObservableCollection<User>()
                {
                    new User(){ID = 0, Login = "ad", Password="ad", Role = UserRole.ADMIN},
                    new User(){ID = 0, Login = "tom", Password="tom", Role= UserRole.CLIENT}
                };
            }
        }

        static public void WriteData()
        {
            string userJson = JsonSerializer.Serialize(Users, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            string broneJson = JsonSerializer.Serialize(Brones, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            string lessonJson = JsonSerializer.Serialize(Lessons, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            File.WriteAllText(USER_FILE, userJson);
            File.WriteAllText(BRONE_FILE, broneJson);
            File.WriteAllText(LESSON_FILE, lessonJson);
        }
    }
}
