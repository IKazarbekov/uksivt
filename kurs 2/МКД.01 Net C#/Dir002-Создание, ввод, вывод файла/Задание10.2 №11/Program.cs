using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Задание_2
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Olimp.AddStudent("Tom", 15, "UGPED", 51);
            Olimp.AddStudent("Bob", 17, "UKSIVT", 53);
            Olimp.AddStudent("Hey", 15, "UUNIT", 12);
            Olimp.AddStudent("Gey", 20, "EEYD", 62);

            if (Olimp.ToPrintOnFile("students.txt"))
                Print("Текстовый файл записан");

            if (Olimp.ToWriteOnFile("students.bin"))
                Print("Бинарный файл записан");

            Olimp.ReadAndPrintFile("students.bin");

        }
        struct Student
        {
            public string name;
            public byte age;
            public string college;
            public int ball;

            public Student(string name, byte age, string college, int ball)
            {
                this.name = name;
                this.age = age;
                this.college = college;
                this.ball = ball;
            }

            public string ToString()
            {
                return $"Студент {name}, {age} лет, учится в {college}, получил {ball} балла";
            }
        }

        static class Olimp
        {
            private static List<Student> students = new List<Student>();

            public static void AddStudent(string name, byte age, string college, int ball)
            {
                students.Add(new Student(name, age, college, ball));
            }

            public static bool ToWriteOnFile(string path)
            {
                if (!path.EndsWith(".bin") || string.IsNullOrWhiteSpace(path))
                    return false;
                try
                {
                    using (FileStream fs = File.OpenWrite(path))
                    {
                        using (BinaryWriter bwr = new BinaryWriter(fs))
                        {
                            List<Student> list = new List<Student>();
                            foreach (Student student in students)
                            {
                                if (student.ball >= 30)
                                {
                                    list.Add(student);
                                }
                            }
                            bwr.Write(list.Count);
                            foreach (Student student in list)
                            {
                                bwr.Write(student.name);
                                bwr.Write(student.age);
                                bwr.Write(student.college);
                                bwr.Write(student.ball);
                            }
                        }
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public static bool ToPrintOnFile(string path)
            {
                if (!path.EndsWith(".txt") || string.IsNullOrWhiteSpace(path))
                    return false;
                try
                {
                    using (FileStream fs = File.OpenWrite(path))
                    {
                        using (BinaryWriter bwr = new BinaryWriter(fs))
                        {
                            foreach (Student student in students)
                            {
                                if (student.ball >= 30)
                                    bwr.Write(student.ToString() + "\n");
                            }
                        }
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public static void ReadAndPrintFile(string path)
            {
                if (!path.EndsWith(".bin") || string.IsNullOrWhiteSpace(path))
                    return;

                StringBuilder result = new StringBuilder();

                using (FileStream fs = File.OpenRead(path))
                using (BinaryReader br = new BinaryReader(fs))
                {
                    int count = br.ReadInt32();
                    while (count-- > 0)
                    {
                        string name = br.ReadString();
                        byte age = br.ReadByte();
                        string college = br.ReadString();
                        int ball = br.ReadInt32();
                        Student student = new Student { name = name, age = age, college = college, ball = ball};
                        result.Append(student.ToString() + "\n");
                    }
                }

                Print(result.ToString());
            }

        }

        static void Print(dynamic obj)
        {
            Console.WriteLine(obj);
        }
    }
}
