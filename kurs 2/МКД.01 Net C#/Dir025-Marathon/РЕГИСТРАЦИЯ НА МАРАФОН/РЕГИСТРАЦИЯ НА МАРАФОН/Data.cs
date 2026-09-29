using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace РЕГИСТРАЦИЯ_НА_МАРАФОН
{
    static class Data
    {
        static public ObservableCollection<Runner> Runners = new ObservableCollection<Runner>()
            {
                new Runner { Email = "ivanov@mail.ru", Password = "pass1", FirstName = "Иван", LastName = "Иванов", Gender = "Мужской", PhotoPath = "images/ivanov.jpg", BirthDate = new DateTime(1992, 5, 12), Country = "Россия", Role = "Бегун" },
                new Runner { Email = "smith@gmail.com", Password = "pass2", FirstName = "John", LastName = "Smith", Gender = "Мужской", PhotoPath = "images/smith.jpg", BirthDate = new DateTime(1988, 11, 23), Country = "США", Role = "Бегун" },
                new Runner { Email = "petrova@yandex.ru", Password = "pass3", FirstName = "Анна", LastName = "Петрова", Gender = "Женский", PhotoPath = "images/petrova.jpg", BirthDate = new DateTime(1995, 3, 7), Country = "Россия", Role = "Бегун" },
                new Runner { Email = "muller@web.de", Password = "pass4", FirstName = "Hans", LastName = "Müller", Gender = "Мужской", PhotoPath = "images/muller.jpg", BirthDate = new DateTime(1990, 8, 19), Country = "Германия", Role = "Бегун" },
                new Runner { Email = "dubois@fr.fr", Password = "pass5", FirstName = "Marie", LastName = "Dubois", Gender = "Женский", PhotoPath = "images/dubois.jpg", BirthDate = new DateTime(1994, 1, 30), Country = "Франция" , Role = "Бегун"},
                new Runner { Email = "sato@yahoo.co.jp", Password = "pass6", FirstName = "Takashi", LastName = "Sato", Gender = "Мужской", PhotoPath = "images/sato.jpg", BirthDate = new DateTime(1985, 4, 15), Country = "Япония" , Role = "Бегун"},
                new Runner { Email = "silva@uol.com.br", Password = "pass7", FirstName = "Carlos", LastName = "Silva", Gender = "Мужской", PhotoPath = "images/silva.jpg", BirthDate = new DateTime(1991, 10, 2), Country = "Бразилия" , Role = "Бегун"},
                new Runner { Email = "smirnova@inbox.ru", Password = "pass8", FirstName = "Елена", LastName = "Смирнова", Gender = "Женский", PhotoPath = "images/smirnova.jpg", BirthDate = new DateTime(1997, 12, 14), Country = "Россия" , Role = "Бегун"},
                new Runner { Email = "wong@hk.com", Password = "pass9", FirstName = "David", LastName = "Wong", Gender = "Мужской", PhotoPath = "images/wong.jpg", BirthDate = new DateTime(1989, 7, 25), Country = "Китай" , Role = "Координатор"},
                new Runner { Email = "garcia@es.es", Password = "pass10", FirstName = "Sofia", LastName = "García", Gender = "Женский", PhotoPath = "images/garcia.jpg", BirthDate = new DateTime(1993, 9, 8), Country = "Испания" , Role = "Координатор"},
                new Runner { Email = "kaza@kaza.za", Password = "1213!", FirstName = "Kaza", LastName = "Zabekov", Gender = "Мужской", PhotoPath = "images/garcia.jpg", BirthDate = new DateTime(2008, 5, 14), Country = "Россия" , Role = "Админ"}
            };
    }

    public class Runner
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string PhotoPath { get; set; }
        public DateTime BirthDate { get; set; }
        public string Country { get; set; }
        public string Role { get; set; }
    }
}
