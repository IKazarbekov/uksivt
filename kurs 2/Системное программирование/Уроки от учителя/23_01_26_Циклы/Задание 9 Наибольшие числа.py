n = int(input())
max1 = 0  # наибольшее
max2 = 0  # второе наибольшее

for _ in range(n):
    num = int(input())
    if num > max1:
        max2 = max1
        max1 = num
    elif num > max2:
        max2 = num

print(max1)
print(max2)