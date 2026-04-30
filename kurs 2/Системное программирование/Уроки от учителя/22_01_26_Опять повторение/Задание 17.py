import math
import math

x1 = float(input("x1: "))
y1 = float(input("y1: "))
x2 = float(input("x2: "))
y2 = float(input("y2: "))
x3 = float(input("x3: "))
y3 = float(input("y3: "))

a = math.hypot(x2 - x1, y2 - y1)
b = math.hypot(x3 - x2, y3 - y2)
c = math.hypot(x1 - x3, y1 - y3)

perimeter = a + b + c
p = perimeter / 2

area = math.sqrt(p * (p - a) * (p - b) * (p - c))

print(f"Периметр = {perimeter:.3f}")
print(f"Площадь = {area:.3f}")