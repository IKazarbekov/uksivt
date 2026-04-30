n = int(input())
total = 0

while n > 0:
    digit = n % 10
    if digit % 2 == 0:
        total += digit
    n //= 10

print(total)