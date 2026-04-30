a, b = int(input()), int(input())
count = 0

for x in range(a, b + 1):
    cube = x ** 3
    last_digit = cube % 10
    if last_digit == 4 or last_digit == 9:
        count += 1

print(count)