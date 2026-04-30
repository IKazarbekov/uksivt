n = int(input())
count = 0
for _ in range(n):
    message = input()
    if message.count('11') >= 3:
        count += 1
print(count)