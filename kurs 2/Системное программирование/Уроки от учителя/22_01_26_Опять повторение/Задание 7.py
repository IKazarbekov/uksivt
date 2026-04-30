integer = input("Целое число: ")

while not integer.isdigit():
    print("Ошибка")
    integer = input("Целое число: ")

result = str()
for i in range(len(integer)):
    char = integer[i]
    if char != '1' and char != '0':
        result += char

print("Ответ:", result)