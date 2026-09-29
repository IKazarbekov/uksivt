import json, os, random

FILE = "data.json"

class Student:
    def __init__(self, name: str, grades: list):
        self.name = name
        self.grades = grades
        self.avg = sum(grades) / len(grades) if len(grades) > 0 else 0

    def to_dict(self):
        return {"name": self.name, "grades": self.grades, "avg": self.avg}

def load_data():
    if not os.path.exists(FILE):
        return [Student("tom", [2,2,3])]
    with open(FILE, "r") as f:
        data = json.load(f)
        students = [Student(s["name"], s["grades"]) for s in data]
        return students

def write_data(students: list[Student]):
    with open(FILE, "w") as f:
        json.dump([s.to_dict() for s in students], f, indent=4)

def prosecced(command: str, payload = None):
    print("\tПолучил команду", command)
    students = load_data()
    match command:
        case "GET-SORT":
            print("\tВернул сортированный список")
            return sorted(students, key=lambda student: student.name)
        case "GET-REVERSE":
            print("\tВернул сортированный обратный список")
            return sorted(students, key=lambda student: student.name, reverse=True)
        case "GET-SHUFFLE":
            print("\tВернул смешанный список")
            random.shuffle(students)
            return students
        case "PUT":
            print("\tСохранил нового студента и вернул весь список ещё раз")
            student = Student(**payload)
            students.append(student)
            write_data(students)
            return sorted(students, key=lambda student: student.name)
        case _:
            print("\tОШИБКА - НЕ ИЗВЕСТНАЯ КОМАНДА! ВЕРНУЛ ПУСТОЙ СПИСОК")
            return []