integer = input("Дайте двухзначное число:")

while not len(integer) == 2 or not integer.isdigit():
    integer = input("Дайте двухзначное число !!:")

digit_1 = int(integer[0])
digit_2 = int(integer[1])

summa = digit_1 + digit_2

print("Сумма цифр:", summa)