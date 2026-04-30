using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Задание10._1__11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (CreateInputFile("file.bin", 1, 2, 3, 5, 4, 5))
                Console.WriteLine("Входной файл записан");
            else
                Console.WriteLine("Входной файл НЕ записан");

            if (CreateOutputFiles("file.bin", "out1.bin", "out2.bin"))
                Console.WriteLine("Выходной файл записан");
            else
                Console.WriteLine("Выходной файл НЕ записан");

            Console.WriteLine(ReadFile("out1.bin"));
            Console.WriteLine(ReadFile("out2.bin"));
        }

        static bool CreateInputFile(string path, params double[] doubles)
        {
            if (!File.Exists(path))
                File.Create(path);
            else
                File.Delete(path);
            try
            {
                using (var fs = File.OpenWrite(path))
                using (var bw = new BinaryWriter(fs))
                    foreach (double d in doubles)
                        bw.Write(d);
                return true;
            }
            catch
            {
                return false;
            }
        }

        static bool CreateOutputFiles(string pathInputFile, string pathOut1, string pathOut2)
        {
            try
            {
                int countFife = 0;

                using (FileStream fsInput = File.OpenRead(pathInputFile), fsOut1 = File.OpenWrite(pathOut1), fsOut2 = File.OpenWrite(pathOut2))
                using (BinaryReader brInput = new BinaryReader(fsInput))
                using (BinaryWriter bwOut1 = new BinaryWriter(fsOut1), bwOut2 = new BinaryWriter(fsOut2))
                {
                    List<double> lst1 = new List<double>();
                    List<double> lst2 = new List<double>();

                    while (true)
                        try
                        {
                            double d = brInput.ReadDouble();
                            if (d % 2 == 0)
                                lst1.Add(d);
                            else
                                lst2.Add(d);

                            if (d == 5)
                                countFife++;
                        }
                        catch
                        {
                            break;
                        }

                    bwOut1.Write(lst1.Count);
                    bwOut1.Write(0);

                    foreach (double d in lst1)
                        bwOut1.Write((double)d);


                    bwOut2.Write(lst2.Count);
                    bwOut2.Write(countFife);

                    foreach (double d in lst2)
                        bwOut2.Write((double)d);


                    for (int i = 0; i < countFife; i++)
                        bwOut2.Write("Пятёрка ! ");

                }

                return true;
            }
            catch { return false; }
        }

        static string ReadFile(string path)
        {
            StringBuilder strBuild = new StringBuilder();
            try
            {
                using (FileStream fs = File.OpenRead(path))
                using (var br = new BinaryReader(fs))
                {
                    int countNumber = br.ReadInt32();
                    int countFife = br.ReadInt32();

                    while(countNumber-- > 0)
                        strBuild.Append(" " + br.ReadDouble());


                    while (countFife-- > 0)
                        strBuild.Append(" " + br.ReadString());

                }
            }
            catch
            {
                strBuild.Append("Error");
            }
            return strBuild.ToString();
        }
    }
}
