import json
import os

DATA_FILE = "data.json"

class Student(object):
    def __init__(self, name: str, grades: list[int]):
        self.name = name
        self.grades = grades
        self.avg = round(sum(grades) / len(grades)) if grades else 0.0

    def to_dict(self):
        return {
            "name": self.name,
            "grades": self.grades,
            "avg": self.avg
        }

def load_data():
    if not os.path.exists(DATA_FILE):
        return []
    with open(DATA_FILE, "r") as f:
        data = json.load(f)
        return [Student(d["name"], d["grades"]) for d in data]

def save_data(data):
    with open(DATA_FILE, "w") as f:
        json.dump(data, f, indent=4)

def command_processing(command, payload):
    match command:
        case "GET":
            print("    Обработал команду GET")
            students = load_data()
            return {"students": [s.to_dict() for s in students]}
        case "PUT":
            print("    Обработал команду PUT")
            students = load_data()
            student = Student(**payload)
            students.append(student)
            save_data([s.to_dict() for s in students])
            return {"students": [s.to_dict() for s in students]}
        case _:
            ...

if __name__ == "__main__":
    print(load_data())