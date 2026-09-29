text = input("Строка:")

while len(text) == 0:
    print("Нету текста")
    text = input("Строка:")

mount = len(text) * 60

print("Стоимость строки:", mount // 100, "рублей", mount % 100,"копеек")