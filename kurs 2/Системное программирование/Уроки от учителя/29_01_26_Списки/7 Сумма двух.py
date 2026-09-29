n = int(input())
numbers = []
for _ in range(n):
    numbers.append(int(input()))

result = []
for i in range(n - 1):
    result.append(numbers[i] + numbers[i + 1])
print(result)