side_1 = int(input("Длина катета 1: "))
side_2 = int(input("Длина катета 2: "))

gipotenuza = ( side_1 ** 2 + side_2 ** 2 ) ** 0.5

perimetr = side_1 + side_2 + gipotenuza

print("Периметр треугольника: ", gipotenuza)