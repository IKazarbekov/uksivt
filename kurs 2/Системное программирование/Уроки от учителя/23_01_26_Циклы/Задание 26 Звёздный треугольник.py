n = int(input())

# Верхняя часть треугольника
for i in range(1, n // 2 + 2):
    print('*' * i)

# Нижняя часть треугольника
for i in range(n // 2, 0, -1):
    print('*' * i)