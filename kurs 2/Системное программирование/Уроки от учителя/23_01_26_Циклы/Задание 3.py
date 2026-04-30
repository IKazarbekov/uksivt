import math

n = int(input())
total = 0

for i in range(1, n + 1):
    total += 1 / i

result = total - math.log(n)
print(result)