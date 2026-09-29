# simple example
class ParentClass:
    pass
class ChildClass(ParentClass):
    pass

# two example
class Animal:
    def __init__(self, name, age):
        self._name = name
        self._age = age

    def sleep(self):
        print( f"{self._name} спит" )
class Cat(Animal):
    pass
cat = Cat("Tom", 5)
cat.sleep()

# initializator
class Animal:
    def __init__(self, name, age):
        self._name = name
        self._age = age

    def sleep(self):
        print( f"{self._name} спит" )
class Cat(Animal):
    def __init__(self, name, age, eye):
        Animal.__init__(self, name, age)
        self._eye = eye

# all class extend object
class MyClass(object):
    pass