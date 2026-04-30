if __name__ == "__main__":
    a = int(input("a: "))
    b = int(input("b: "))

    while b == 0:
        print("b не должен быть нулём!")
        b = int(input("b: "))

    summa = a + b
    diff = a - b
    mult = a * b
    chas = a / b
    int_chas = a // b
    ost = a % b
    root = (a ** 10 + b ** 10)**0.5

    print("Сумма:", summa)
    print("Разность:", diff)
    print("Произведение:", mult)
    print("Частное:", chas)
    print("Целая часть от частного:", int_chas)
    print("Остаток от деления:", ost)
    print("Корень квадратный из суммы их 10 степеней:", root)
    print("Сумма:", summa)
