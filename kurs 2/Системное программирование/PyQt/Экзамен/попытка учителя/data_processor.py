import json
# Импорт модуля для работы с файловой системой
import os
# Константа с именем файла для хранения данных
DATA_FILE = "data.json"

# Класс, представляющий студента
class Student:
    # Конструктор класса
    def __init__(self, name, grades):
        self.name = name
        self.grades = grades
        # Вычисление средней оценки (округленной до 2 знаков)
        self.avg = round(sum(grades) / len(grades), 2) if grades else 0.0

    # Метод для преобразования объекта в словарь
    def to_dict(self):
        return {
            "name": self.name,
            "grades": self.grades,
            "avg": self.avg # Средняя оценка уже вычислена в конструкторе
        }

# Функция загрузки данных из файла
def load_data():
    # Проверка существования файла
    #exists - Функция из модуля os.path. Проверяет существование файла или директории по указанному пути
    if not os.path.exists(DATA_FILE):
        return []  # Возвращаем пустой список, если файла нет
    # Открытие файла для чтения
    with open(DATA_FILE, "r", encoding="utf-8") as f:
        data = json.load(f) # Загрузка JSON из файла
        # Преобразование словарей в объекты Student
        return [Student(d["name"], d["grades"]) for d in data]

# Функция сохранения данных в файл
def save_data(students):
    # Открытие файла для записи
    with open(DATA_FILE, "w", encoding="utf-8") as f:
        # Преобразование объектов Student в словари и запись в JSON
        #json.dump() Функция для сериализации Python-объектов в JSON-формат
        #Записывает результат напрямую в файловый объект (в отличие от json.dumps(), который возвращает строку)
        #[s.to_dict() for s in students] - преобразующий список студентов в список словарей
        # indent=4 - Добавляет отступы в 4 пробела для вложенных структур (Без этого параметра JSON записывался бы в одну строку)
        json.dump([s.to_dict() for s in students], f, indent=4)

# Загрузка данных при старте программы
students = load_data()

# Функция обработки команд
def process_command(command, payload):
    global students  # Используем глобальную переменную students. Видна во всех функциях модуля. По умолчанию доступна только для чтения внутри функций
    # Используем match-case для обработки разных команд
    match command:
        case "PUT": # Добавление нового студента. .get() метод Специальный метод словаря для безопасного доступа
            name = payload.get("name")
            grades = payload.get("grades")
            # Проверка корректности данных
            #Это условие выполняет валидацию входных данных перед созданием нового студента
            #isinstance() - безопасная проверка типа, учитывающая наследование
            #Проверяет, что grades НЕ является объектом типа list
            if not name or not isinstance(grades, list):
                return {"error": "Некорректные данные"}
            # Создание нового студента
            new_student = Student(name, grades)
            students.append(new_student)  # Добавление в список
            save_data(students) # Сохранение данных
            return {"status": "ok"}

        case "GET-SORT": # Получение отсортированного списка
            # Сортировка по имени (без учета регистра)
            #Я тут уже устала чет сильно
            #key=lambda s: s.name.lower() - Лямбда-функция, которая для каждого студента: Берет его имя (s.name). Приводит к нижнему регистру (lower())
           #Лямбда-функция (lambda) — это анонимная (безымянная) функция в Python, которая:
            #Не имеет имени (в отличие от def). Может принимать аргументы. Выполняет одно выражение и возвращает его результат. Используется там, где нужна простая функция на короткое время
           #Синтаксис лямбда-функции lambda аргументы: выражение
        # lambda — ключевое слово
        # аргументы — входные параметры (можно несколько через запятую)
            # выражение — что функция возвращает (нельзя писать return явно)
            result = sorted(students, key=lambda s: s.name.lower())
            #s.to_dict() for s in students] - Это генератор списка, который преобразует каждый объект Student
            # в списке students в словарь с помощью метода to_dict().
            return {"students": [s.to_dict() for s in result]}

        case "GET-REVERSE": # Получение списка в обратном порядке
            result = list(reversed(students))
            return {"students": [s.to_dict() for s in result]}

        case "GET-SHUFFLE": # Получение перемешанного списка
            import random # Импорт внутри функции, чтобы не загружать без необходимости
            result = students[:] # Создаем копию списка
            random.shuffle(result)  # Перемешиваем
            return {"students": [s.to_dict() for s in result]}

        case _: # Обработка неизвестной команды
            return {"error": f"Неизвестная команда: {command}"}
