s = input()
count = 0
for char in s:
    if char.islower():
        count += 1
print(count)