n = int(input())
max_digit = 0
min_digit = 9

while n > 0:
    digit = n % 10
    if digit > max_digit:
        max_digit = digit
    if digit < min_digit:
        min_digit = digit
    n //= 10

print(f"Максимальная цифра равна {max_digit}")
print(f"Минимальная цифра равна {min_digit}")