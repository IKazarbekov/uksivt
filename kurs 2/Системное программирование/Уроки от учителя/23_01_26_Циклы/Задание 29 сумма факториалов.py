n = int(input())
total = 0

for i in range(1, n + 1):
    factorial = 1
    for j in range(1, i + 1):
        factorial *= j
    total += factorial

print(total)