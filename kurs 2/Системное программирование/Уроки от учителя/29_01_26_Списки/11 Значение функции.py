n = int(input())
numbers = []
for _ in range(n):
    num = int(input())
    numbers.append(num)

print()
for num in numbers:
    print(num ** 2 + 2 * num + 1)
