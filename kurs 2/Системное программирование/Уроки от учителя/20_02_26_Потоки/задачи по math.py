import math


def zadanie1():
    R = 6371.0
    phi1 = float(input("Введите широту первой точки (в градусах): "))
    lambda1 = float(input("Введите долготу первой точки (в градусах): "))
    phi2 = float(input("Введите широту второй точки (в градусах): "))
    lambda2 = float(input("Введите долготу второй точки (в градусах): "))

    phi1 = math.radians(phi1)
    lambda1 = math.radians(lambda1)
    phi2 = math.radians(phi2)
    lambda2 = math.radians(lambda2)

    d = R * math.acos(math.sin(phi1) * math.sin(phi2) + math.cos(phi1) * math.cos(phi2) * math.cos(lambda2 - lambda1))
    print(f"Расстояние между точками: {d:.2f} км")


def zadanie2():
    r1 = float(input("Введите радиус первого круга: "))
    r2 = float(input("Введите радиус второго круга: "))
    d = float(input("Введите расстояние между центрами кругов: "))

    if d > 0 and d < r1 + r2:
        A = (r1 ** 2 * math.acos((d ** 2 + r1 ** 2 - r2 ** 2) / (2 * d * r1)) +
             r2 ** 2 * math.acos((d ** 2 + r2 ** 2 - r1 ** 2) / (2 * d * r2)) -
             0.5 * math.sqrt((d + r1 + r2) * (d + r1 - r2) * (d - r1 + r2) * (d - r1 - r2)))
        print(f"Площадь пересечения кругов: {A:.2f}")
    else:
        print("Круги не пересекаются или введены некорректные данные")


def zadanie3():
    n = int(input("Введите число для вычисления факториала: "))
    print(f"Факториал числа {n} = {math.factorial(n)}")


def zadanie4():
    x = float(input("Введите дробное число: "))
    print(f"Округление вверх: {math.ceil(x)}")
    print(f"Округление вниз: {math.floor(x)}")


def zadanie5():
    angle_deg = float(input("Введите угол в градусах: "))
    angle_rad = math.radians(angle_deg)
    print(f"Синус: {math.sin(angle_rad):.4f}")
    print(f"Косинус: {math.cos(angle_rad):.4f}")
    print(f"Тангенс: {math.tan(angle_rad):.4f}")


print("Задача 1: Расстояние между точками на Земле")
zadanie1()
print("\nЗадача 2: Площадь пересечения двух кругов")
zadanie2()
print("\nЗадача 3: Факториал числа")
zadanie3()
print("\nЗадача 4: Округление чисел")
zadanie4()
print("\nЗадача 5: Тригонометрические функции")
zadanie5()