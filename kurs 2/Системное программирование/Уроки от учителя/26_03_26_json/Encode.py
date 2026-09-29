import json
from json import JSONEncoder

class Cat(JSONEncoder):
    def __init__(self, name, age):
        self.name = name
        self.age = age

    def default(self, o):
        return o.__dict__

cat = Cat("Tom", 4)

json_string = json.dumps(cat)

print(json_string)