b1 = int(input())
b2 = int(input())
b3 = int(input())

sorted_nums = sorted([b1, b2, b3], reverse=True)

for num in sorted_nums:
    print(num)