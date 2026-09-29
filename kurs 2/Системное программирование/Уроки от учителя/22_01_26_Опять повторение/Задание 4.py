x1 = int(input("x первой точки: "))
y1 = int(input("y первой точки: "))
x2 = int(input("x второй точки: "))
y2 = int(input("y второй точки: "))

diff_x = abs(x1 - x2)
diff_y = abs(y1 - y2)

distance = (diff_x ** 2 + diff_y ** 2 ) ** 0.5

print("Расстояние между точками: ", distance)