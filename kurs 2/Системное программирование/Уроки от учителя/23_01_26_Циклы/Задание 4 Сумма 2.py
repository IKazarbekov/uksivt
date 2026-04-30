n = int(input())
total = 0

for x in range(1, n + 1):
    square = x ** 2
    last_digit = square % 10
    if last_digit in (2, 5, 8):
        total += x

print(total)