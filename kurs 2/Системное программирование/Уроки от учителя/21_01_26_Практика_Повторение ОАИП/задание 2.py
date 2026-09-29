weight = float(input("Масса(кг): "))
while weight <= 0 or weight > 500:
    print("Неверная масса:")
    weight = float(input("Масса: "))
height = float(input("Рост: (м)"))

while height <= 0 or height > 5:
    print("Неверный рост")
    height = float(input("Рост: "))

IMT = weight / (height ** 2)

print("Ваш IMT:", IMT)
if 18.5 <= IMT <= 25:
    print("Оптимальная масса")
elif IMT < 18.5:
    print("Не достаточная масса")
else:
    print("Избыточная масса")