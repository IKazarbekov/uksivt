num = int(input("Введите четырёхзначное число: "))

d1 = num // 1000
d2 = num // 100 % 10
d3 = num // 10 % 10
d4 = num % 10

even_product = 1
odd_product = 1

for digit in (d1, d2, d3, d4):
    if digit % 2 == 0:
        even_product *= digit
    else:
        odd_product *= digit

if odd_product == 0:
    result = "Нельзя делить на ноль"
else:
    result = even_product / odd_product

print(f"Частное произведений чётных и нечётных цифр: {result}")