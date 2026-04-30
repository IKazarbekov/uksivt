x = int(input("x точки: "))
y = int(input("y точки: "))

x = abs(x)
y = abs(y)

distance = (x ** 2 + y ** 2) ** 0.5

print("Расстояние до начала координат", distance)