n = int(input())
original_n = n

# Инициализируем все переменные
count_3 = 0
last_digit = n % 10
count_last_digit = 0
count_even = 0
sum_greater_5 = 0
product_greater_7 = 1
count_0_5 = 0

while n > 0:
    digit = n % 10

    # 1) количество цифр 3
    if digit == 3:
        count_3 += 1

    # 2) сколько раз встречается последняя цифра
    if digit == last_digit:
        count_last_digit += 1

    # 3) количество чётных цифр
    if digit % 2 == 0:
        count_even += 1

    # 4) сумма цифр, больших пяти
    if digit > 5:
        sum_greater_5 += digit

    # 5) произведение цифр, больших семи
    if digit > 7:
        product_greater_7 *= digit

    # 6) сколько раз встречаются цифры 0 и 5
    if digit == 0 or digit == 5:
        count_0_5 += 1

    n //= 10

# Корректировка для product_greater_7
# Если ни одной цифры >7 не было, product_greater_7 останется равным 1
# Если была только одна такая цифра, product_greater_7 будет равен этой цифре

print(count_3)
print(count_last_digit)
print(count_even)
print(sum_greater_5)
print(product_greater_7)
print(count_0_5)