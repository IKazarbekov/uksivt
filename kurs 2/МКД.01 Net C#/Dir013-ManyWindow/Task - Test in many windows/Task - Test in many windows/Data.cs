using System.Collections.Generic;

namespace Task___Test_in_many_windows
{
    internal static class Data
    {
        public abstract class Test
        {

        }
        public class TestString : Test
        {
            public TestString(string question, string answer)
            {
                Question = question;
                Answer = answer;
            }

            public string Question { get; set; }
            public string Answer { get; set; }
        }

        public class TestOneAnswer : Test
        {
            public TestOneAnswer(string question, string trueAnswer, string secondAnswer, string threeAnswer = null, string fourAnswer = null)
            {
                Question = question;
                TrueAnswer = trueAnswer;
                SecondAnswer = secondAnswer;
                ThreeAnswer = threeAnswer;
                FourAnswer = fourAnswer;
            }
            public string Question { get; set; }
            public string TrueAnswer { get; set; }
            public string SecondAnswer { get; set; }
            public string ThreeAnswer { get; set; }
            public string FourAnswer { get; set; }
            public int CountAnswer
            {
                get
                {
                    if (ThreeAnswer == null)
                        return 2;
                    else if (FourAnswer == null)
                        return 3;
                    else return 4;
                }
            }
        }

        public class TestManyAnswers : Test
        {
            public TestManyAnswers(string question, int countTrueAnswers, string firstAnswer, string secondAnswer, string threeAnswer, string fourAnswer = null)
            {
                Question = question;
                CountTrueAnswers = countTrueAnswers;
                FirstAnswer = firstAnswer;
                SecondAnswer = secondAnswer;
                ThreeAnswer = threeAnswer;
                FourAnswer = fourAnswer;
            }

            public int CountTrueAnswers { get; set; }
            public string Question { get; set; }
            public string FirstAnswer { get; set; }
            public string SecondAnswer { get; set; }
            public string ThreeAnswer { get; set; }
            public string FourAnswer { get; set; }
            public List<string> TrueAnswers
            {
                get
                {
                    List<string> list = new List<string>() { FirstAnswer };
                    if (CountTrueAnswers > 1)
                        list.Add(SecondAnswer);
                    if (CountTrueAnswers > 2)
                        list.Add(ThreeAnswer);
                    if (CountTrueAnswers > 3)
                        list.Add(FourAnswer);
                    return list;
                }
            }
        }

        /*
         
        public static TestString[] testsString =
        {
            new TestString("2 + 2", "4"),
            new TestString("4 - 2", "2"),
            new TestString("Столица Башкортостана", "Уфа"),
            new TestString("Столица России", "Москва"),
            new TestString("Столица Франции", "Париж"),
            new TestString("Столица Великобритании", "Лондон"),
            new TestString("Столица Италии", "Рим"),
            new TestString("Сколько месяцев в году", "12"),
            new TestString("Сколько дней в неделе", "7"),
            new TestString("Сколько часов в сутках", "24"),
            new TestString("Самая высокая гора в мире", "Эверест"),
            new TestString("Самая длинная река в мире", "Амазонка"),
            new TestString("Какой газ необходим для дыхания", "Кислород"),
            new TestString("Сколько букв в русском алфавите", "33"),
            new TestString("Первый президент России", "Ельцин"),
            new TestString("Год основания Москвы", "1147"),
            new TestString("Автор романа 'Война и мир'", "Толстой"),
            new TestString("Планета, на которой мы живем", "Земля"),
            new TestString("Самая близкая звезда к Земле", "Солнце"),
            new TestString("Сколько материков на Земле", "6")
        };

        public static TestOneAnswer[] testsOneAnswers =
        {
            new TestOneAnswer("3 + 23", "26", "23", "19"),
            new TestOneAnswer("Столица Башкортостана", "Уфа", "Баймак", "Париж"),
            new TestOneAnswer("Когда был открыт УКСИВТ", "1939", "1920", "1982", "2025"),
            new TestOneAnswer("Кто ты?", "Человек", "Робот", "Гений", "Никто"),
            new TestOneAnswer("Какой язык изучают в ОИБ", "C#", "Python", "Java"),
            new TestOneAnswer("Сколько будет 5 × 5", "25", "20", "30", "35"),
            new TestOneAnswer("Столица Германии", "Берлин", "Мюнхен", "Гамбург", "Кёльн"),
            new TestOneAnswer("Столица Испании", "Мадрид", "Барселона", "Валенсия", "Севилья"),
            new TestOneAnswer("Самая большая планета Солнечной системы", "Юпитер", "Сатурн", "Уран", "Нептун"),
            new TestOneAnswer("Сколько континентов на Земле", "6", "5", "7", "4"),
            new TestOneAnswer("Химический символ воды", "H2O", "CO2", "O2", "H2"),
            new TestOneAnswer("Год полета Гагарина в космос", "1961", "1957", "1965", "1970"),
            new TestOneAnswer("Автор 'Евгения Онегина'", "Пушкин", "Лермонтов", "Толстой", "Достоевский"),
            new TestOneAnswer("Самое глубокое озеро в мире", "Байкал", "Танганьика", "Виктория", "Ладожское"),
            new TestOneAnswer("Кто написал 'Гарри Поттера'", "Роулинг", "Толкин", "Мартин", "" +
                "Льюис"),
            new TestOneAnswer("Сколько градусов в прямом угле", "90", "180", "45", "360"),
            new TestOneAnswer("Какое животное является символом Австралии", "Кенгуру", "Коала", "Утконос", "Динго"),
            new TestOneAnswer("В каком году началась Вторая мировая война", "1939", "1941", "1937", "1945"),
            new TestOneAnswer("Самая маленькая страна в мире", "Ватикан", "Монако", "Сан-Марино", "Люксембург"),
            new TestOneAnswer("Сколько цветов в радуге", "7", "6", "8", "5")
        };

        public static TestManyAnswers[] testsManyAnswers =
        {
            new TestManyAnswers("x^2 = 4", 2, "2", "-2", "7", "0"),
            new TestManyAnswers("Когда была вторая мировая война ?", 3, "1940", "1943", "1945", "1946"),
            new TestManyAnswers("Что делает Ильяс, когда грустит ?", 2, "Молчит", "Смотрит в одну точку", "Прыгает", "Стреляет из катапульты в замок цветов"),
            new TestManyAnswers("Какие из этих чисел являются простыми", 3, "2", "3", "5", "4"),
            new TestManyAnswers("Какие из этих планет являются газовыми гигантами", 2, "Юпитер", "Сатурн", "Марс", "Венера"),
            new TestManyAnswers("Какие цвета есть во флаге России", 3, "Белый", "Синий", "Красный", "Зеленый"),
            new TestManyAnswers("Какие из этих животных являются млекопитающими", 3, "Кошка", "Собака", "Кит", "Крокодил"),
            new TestManyAnswers("Какие из этих фруктов растут на деревьях", 3, "Яблоко", "Груша", "Апельсин", "Арбуз"),
            new TestManyAnswers("Какие из этих стран находятся в Европе", 3, "Франция", "Германия", "Италия", "Китай"),
            new TestManyAnswers("Какие из этих металлов являются драгоценными", 2, "Золото", "Серебро", "Железо", "Алюминий"),
            new TestManyAnswers("Кто из этих людей был президентом США", 3, "Вашингтон", "Линкольн", "Рузвельт", "Черчилль"),
            new TestManyAnswers("Какие из этих языков являются официальными в ООН", 3, "Английский", "Французский", "Русский", "Немецкий"),
            new TestManyAnswers("Какие из этих океанов существуют на Земле", 3, "Тихий", "Атлантический", "Индийский", "Южный"),
            new TestManyAnswers("Какие из этих чисел делятся на 3", 3, "3", "6", "9", "10"),
            new TestManyAnswers("Какие из этих музыкальных инструментов струнные", 3, "Скрипка", "Виолончель", "Арфа", "Труба"),
            new TestManyAnswers("Какие из этих видов спорта являются зимними", 3, "Хоккей", "Фигурное катание", "Лыжные гонки", "Футбол"),
            new TestManyAnswers("Какие из этих устройств являются устройствами ввода", 3, "Клавиатура", "Мышь", "Сканер", "Монитор"),
            new TestManyAnswers("Какие из этих овощей являются корнеплодами", 3, "Морковь", "Картофель", "Свекла", "Помидор"),
            new TestManyAnswers("Какие из этих наук являются естественными", 3, "Физика", "Химия", "Биология", "История"),
            new TestManyAnswers("Какие из этих геометрических фигур являются четырехугольниками", 3, "Квадрат", "Прямоугольник", "Ромб", "Треугольник")
        };

        */


        public static TestString[] testsString =
       {
            new TestString("В каком году создан УКСИВТ ?", "1932"),
            new TestString("Сколько специальностей в УКСИВТе ?", "12"),
            new TestString("Сколько железных шкафов ?", "16"),
            new TestString("Сколько ключей на железных шкафах?", "14"),

        };

        public static TestOneAnswer[] testsOneAnswers =
        {
            new TestOneAnswer("В каких местах есть электронный дневник", "УКСИВТ", "УТЕК", "Школы"),
            new TestOneAnswer("Сколько всего было директоров УКСИВТа?", "10", "8", "6"),
            new TestOneAnswer("Чем занимается СТУД СОВЕТ?", "Просто сидят", "Развиваются", "Много чего"),
            new TestOneAnswer("Сколько людей учатся в УКСИВТЕ ?", "2500-2700", "1800-2000", "2000-2500",  "2700-3000"),
            new TestOneAnswer("Сайт УКСИВТа правду ли говорит о себе?", "Нет", "Да"),
            new TestOneAnswer("Писать конспекты помогает усвоить материал?", "Нет", "Да"),
            new TestOneAnswer("Коворкинг каким был раньше?", "Читальный зал", "Таким же", "Его не было", "Обычным кабинетом"),

        };

        public static TestManyAnswers[] testsManyAnswers =
        {
            new TestManyAnswers("ОИБ какие языки изучают?", 1, "C#", "C++", "Python", "Java"),
            new TestManyAnswers("Какие группы изучают С++?", 1, "Компьют.компесы", "Интегрирован. системы", "ИС", "Программисты"),
            };



        public class TestClose
        {
            public Test test { get; set; }
            public string UserAnswer { get; set; }
        }

        public const int COUNT_QUESTION = 3;
        public static string currentName { get; set; }
        public static int questionNumber { get; set; }
        public static int currentQuestionNumber { get; set; }
        public static List<Test> testsClose { get; set; } = new List<Test>();
        public static Dictionary<string, int> statistic { get; set; } = new Dictionary<string, int>();
        public static List<TestClose> testClosesUser = new List<TestClose>();

        public static int ResultPoints
        {
            get
            {
                double summa = 0;
                double max = 0;
                foreach (TestClose testUser in testClosesUser)
                {
                    if (testUser.test is TestOneAnswer test)
                    {
                        if (testUser.UserAnswer == test.TrueAnswer)
                            summa++;
                        else
                        { }
                        max++;
                    }
                    else if (testUser.test is TestString testString)
                    {
                        if (testUser.UserAnswer == testString.Answer)
                            summa++;
                        else
                            { }
                        max++;
                    }
                    else if (testUser.test is TestManyAnswers testMany)
                    {
                        List<string> trueAnswers = testMany.TrueAnswers;
                        string[] userAnswers = testUser.UserAnswer.Split('_');
                        foreach (string userAnswer in userAnswers)
                        {
                            if (trueAnswers.Contains(userAnswer))
                                summa++;
                            else
                                { summa--; }
                            max++;
                        }
                    }
                }

                int result = (int)(summa / max * 100);
                statistic.Add(currentName, result);
                return result;
            }
        }
    }
}
