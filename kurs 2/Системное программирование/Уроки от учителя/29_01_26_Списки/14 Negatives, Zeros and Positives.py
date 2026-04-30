n = int(input())
numbers = []
for _ in range(n):
    numbers.append(int(input()))

for num in numbers:
    if num < 0:
        print(num)

for num in numbers:
    if num == 0:
        print(num)

for num in numbers:
    if num > 0:
        print(num)