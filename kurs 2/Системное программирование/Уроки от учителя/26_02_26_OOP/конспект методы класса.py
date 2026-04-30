class MyClass:
    # class method have access only to class
    @classmethod
    def my_class_method(cls):
        print("Это простейший метод класса")
        print(cls)
MyClass.my_class_method()

class Cat:
    def __init__(self, breed, name):
        self.breed = breed
        self.name = name
    # return
    @classmethod
    def british(cls, name):
        return cls('british',name)
cat = Cat.british("Gon")
print(cat.name, cat.breed)

# example with dict and datetime
from datetime import datetime
cats = dict.fromkeys(['Джон', 'Роджер'])
dt = datetime.strptime('03.03.2026', '%d.%m.%Y')
print(cats)
print(dt)

#example simple static method
class MyClass:
    @staticmethod
    #no self and cls
    def my_static_method():
        print("Это мой статический метод")
MyClass.my_static_method()

