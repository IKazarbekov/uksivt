texts = input("Строка: ").split()

count = 0
for i in range(1, len(texts)):
    b = texts[i - 1]
    a = texts[i]
    if a > b:
        count += 1

print("Ответ:", count)