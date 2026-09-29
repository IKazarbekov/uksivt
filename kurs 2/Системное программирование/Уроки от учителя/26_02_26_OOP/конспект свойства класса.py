class Cat1:
    def __init__(self, name):
        self._name = name

    @property
    def name(self):
        return self._name

    @name.setter
    def name(self, name):
        self._name = name

    @name.deleter
    def name(self):
        del self._name

class Cat2:
    def __init__(self, name):
        self._name = name

    @property
    def name(self):
        return self._name

    @name.setter
    def set_name(self, name):
        self._name = name

    @name.deleter
    def del_name(self):
        del self._name

cat2 = Cat2("Tom")
print(f"Cat 2 name is {cat2.name}")
try:
    cat2.name = "Bob"
except AttributeError as e:
    print(e)
cat2.set_name = 2
print(f"Cat have attribute set_name ?? {cat2.set_name}")

class Cat3:
    def __init__(self, name):
        self._name = name

    def get_name(self):
        return self._name

    name = property(get_name)

    def set_name(self, name):
        self._name = name

    name = name.setter(set_name)

    @name.deleter
    def name(self):
        del self._name

cat3 = Cat3("Tomas")
cat3.name = "Bobas"
print('name cat3 is',cat3.name)
del cat3.name
try:
    print(cat3.name)
except AttributeError as e:
    print(e)
