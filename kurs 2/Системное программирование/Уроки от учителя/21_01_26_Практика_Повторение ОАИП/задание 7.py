n = int(input("Кол-во точек: "))
while not n > 0:
    n = int(input("Кол-во точек: "))

points = list()

for i in range(n):
    texts = input(f"Точка {i}:").split()
    x = int(texts[0])
    y = int(texts[1])
    points.append((x, y))

a1 = 0
a2 = 0
a3 = 0
a4 = 0
ost = 0

for tup in points:
    x = tup[0]
    y = tup[1]
    if x > 0 and y > 0:
        a1 += 1
    elif x < 0 and y > 0:
        a2 += 1
    elif x < 0 and y < 0:
        a3 += 1
    elif x > 0 and y < 0:
        ost += 1

print("Первая четвердь:", a1)
print("Вторая четвердь:", a2)
print("Третья четвердь:", a3)
print("Четвёртая четвердь:", a4)
print("На координатных осях:", ost)
