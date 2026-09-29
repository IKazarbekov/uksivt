import os
import json
import random

FILE = "data.json"
students = []

class Student:
    def __init__(self, name: str, grades: list):
        self.name = name
        self.grades = grades
        self.avg = sum(grades) / len(grades) if len(grades) > 0 else 0

def load_data():
    if not os.path.exists(FILE):
        return [Student("Tom", [2,2,3,5])]
    with open(FILE, 'r') as f:
        return json.load(f)

def save_data():
    with open(FILE, 'w') as f:
        json.dump(students, f)

def processing(command: str, payload):
    print("\tПринял команду", command)
    match command:
        case "GET-SORT":
            return sorted(students, key=lambda student: student.name, reverse=False)
        case "GET-REVERSE":
            return sorted(students, key=lambda student: student.name, reverse=True)
        case "GET-SHUFFLE":
            return random.shuffle(students)
        case _:
            print("    НЕ ИЗВЕСТНАЯ КОМАНДА")
    print("\tОбработал команду")
