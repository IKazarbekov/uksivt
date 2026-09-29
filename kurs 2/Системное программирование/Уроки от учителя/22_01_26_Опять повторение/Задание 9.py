A = float(input("A: "))
B = float(input("B: "))

# Уравнение: A*x + B = 0
if A == 0:
    if B == 0:
        print("Бесконечное количество решений")
    else:
        print("Нет решений")
else:
    x = -B / A
    print(f"x = {x}")