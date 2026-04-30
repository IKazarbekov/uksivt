numbers = list(map(int, input().split()))
count = 0
n = len(numbers)

for i in range(n):
    for j in range(i + 1, n):
        if numbers[i] == numbers[j]:
            count += 1

print(count)