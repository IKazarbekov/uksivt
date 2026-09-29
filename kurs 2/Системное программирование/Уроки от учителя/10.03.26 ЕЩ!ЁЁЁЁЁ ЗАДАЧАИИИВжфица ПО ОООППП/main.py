import math
from abc import ABC, abstractmethod


class Shape(ABC):
    @abstractmethod
    def area(self):
        raise NotImplementedError

    @abstractmethod
    def perimeter(self):
        raise NotImplementedError

    @abstractmethod
    def __str__(self):
        pass


class Circle(Shape):
    def __init__(self, radius):
        if radius <= 0:
            raise ValueError("Радиус должен быть положительным числом")
        self.radius = radius

    def area(self):
        return math.pi * self.radius ** 2

    def perimeter(self):
        return 2 * math.pi * self.radius

    def __str__(self):
        return f"Круг: радиус={self.radius}, площадь={self.area():.2f}, периметр={self.perimeter():.2f}"


class Rectangle(Shape):
    def __init__(self, width, height):
        if width <= 0 or height <= 0:
            raise ValueError("Ширина и высота должны быть положительными числами")
        self.width = width
        self.height = height

    def area(self):
        return self.width * self.height

    def perimeter(self):
        return 2 * (self.width + self.height)

    def __str__(self):
        return f"Прямоугольник: ширина={self.width}, высота={self.height}, площадь={self.area():.2f}, периметр={self.perimeter():.2f}"


class Triangle(Shape):
    def __init__(self, a, b, c):
        if a <= 0 or b <= 0 or c <= 0:
            raise ValueError("Стороны должны быть положительными числами")
        if not (a + b > c and a + c > b and b + c > a):
            raise ValueError("Треугольник с такими сторонами не существует")
        self.a = a
        self.b = b
        self.c = c

    def area(self):
        s = self.perimeter() / 2
        return math.sqrt(s * (s - self.a) * (s - self.b) * (s - self.c))

    def perimeter(self):
        return self.a + self.b + self.c

    def __str__(self):
        return f"Треугольник: стороны={self.a}, {self.b}, {self.c}, площадь={self.area():.2f}, периметр={self.perimeter():.2f}"


class Square(Shape):
    def __init__(self, side):
        if side <= 0:
            raise ValueError("Сторона должна быть положительным числом")
        self.side = side

    def area(self):
        return self.side ** 2

    def perimeter(self):
        return 4 * self.side

    def __str__(self):
        return f"Квадрат: сторона={self.side}, площадь={self.area():.2f}, периметр={self.perimeter():.2f}"


class Ellipse(Shape):
    def __init__(self, a, b):
        if a <= 0 or b <= 0:
            raise ValueError("Полуоси должны быть положительными числами")
        self.a = a
        self.b = b

    def area(self):
        return math.pi * self.a * self.b

    def perimeter(self):
        return math.pi * (3 * (self.a + self.b) - math.sqrt((3 * self.a + self.b) * (self.a + 3 * self.b)))

    def __str__(self):
        return f"Эллипс: полуоси={self.a}, {self.b}, площадь={self.area():.2f}, периметр={self.perimeter():.2f}"


class ShapeCollection:
    def __init__(self):
        self.shapes = []

    def add_shape(self, shape):
        self.shapes.append(shape)

    def remove_shape(self, index):
        if 0 <= index < len(self.shapes):
            self.shapes.pop(index)
            return True
        return False

    def total_area(self):
        return sum(shape.area() for shape in self.shapes)

    def total_perimeter(self):
        return sum(shape.perimeter() for shape in self.shapes)

    def get_sorted_by_area(self, ascending=True):
        sorted_shapes = sorted(self.shapes, key=lambda x: x.area(), reverse=not ascending)
        return sorted_shapes

    def __str__(self):
        if not self.shapes:
            return "Коллекция пуста"
        result = "Список фигур:\n"
        for i, shape in enumerate(self.shapes):
            result += f"{i}. {shape}\n"
        return result


def get_positive_float(prompt):
    while True:
        try:
            value = float(input(prompt))
            if value <= 0:
                print("Ошибка: значение должно быть положительным")
                continue
            return value
        except ValueError:
            print("Ошибка: введите число")


def add_shape_menu(collection):
    print("\nВыберите тип фигуры:")
    print("1. Круг")
    print("2. Прямоугольник")
    print("3. Треугольник")
    print("4. Квадрат")
    print("5. Эллипс")

    try:
        choice = int(input("Ваш выбор: "))

        if choice == 1:
            radius = get_positive_float("Введите радиус: ")
            collection.add_shape(Circle(radius))
            print("Круг добавлен")

        elif choice == 2:
            width = get_positive_float("Введите ширину: ")
            height = get_positive_float("Введите высоту: ")
            collection.add_shape(Rectangle(width, height))
            print("Прямоугольник добавлен")

        elif choice == 3:
            a = get_positive_float("Введите сторону a: ")
            b = get_positive_float("Введите сторону b: ")
            c = get_positive_float("Введите сторону c: ")
            try:
                collection.add_shape(Triangle(a, b, c))
                print("Треугольник добавлен")
            except ValueError as e:
                print(f"Ошибка: {e}")

        elif choice == 4:
            side = get_positive_float("Введите сторону: ")
            collection.add_shape(Square(side))
            print("Квадрат добавлен")

        elif choice == 5:
            a = get_positive_float("Введите большую полуось: ")
            b = get_positive_float("Введите малую полуось: ")
            collection.add_shape(Ellipse(a, b))
            print("Эллипс добавлен")

        else:
            print("Неверный выбор")

    except ValueError:
        print("Ошибка: введите число")


def main():
    collection = ShapeCollection()

    while True:
        print("\n" + "=" * 40)
        print("МЕНЮ:")
        print("1. Добавить фигуру")
        print("2. Показать все фигуры")
        print("3. Удалить фигуру по индексу")
        print("4. Показать суммарную площадь и периметр")
        print("5. Показать фигуры, отсортированные по площади")
        print("6. Выход")

        try:
            choice = int(input("Выберите действие: "))

            if choice == 1:
                add_shape_menu(collection)

            elif choice == 2:
                print(collection)

            elif choice == 3:
                print(collection)
                if collection.shapes:
                    try:
                        index = int(input("Введите индекс фигуры для удаления: "))
                        if collection.remove_shape(index):
                            print("Фигура удалена")
                        else:
                            print("Неверный индекс")
                    except ValueError:
                        print("Ошибка: введите число")

            elif choice == 4:
                print(f"Суммарная площадь: {collection.total_area():.2f}")
                print(f"Суммарный периметр: {collection.total_perimeter():.2f}")

            elif choice == 5:
                print("\nВыберите порядок сортировки:")
                print("1. По возрастанию")
                print("2. По убыванию")
                sort_choice = input("Ваш выбор: ")
                ascending = sort_choice == "1"

                sorted_shapes = collection.get_sorted_by_area(ascending)
                if not sorted_shapes:
                    print("Коллекция пуста")
                else:
                    print("\nФигуры, отсортированные по площади:")
                    for i, shape in enumerate(sorted_shapes):
                        print(f"{i}. {shape}")

            elif choice == 6:
                print("Программа завершена")
                break

            else:
                print("Неверный выбор, попробуйте снова")

        except ValueError:
            print("Ошибка: введите число")
        except Exception as e:
            print(f"Произошла ошибка: {e}")


if __name__ == "__main__":
    main()