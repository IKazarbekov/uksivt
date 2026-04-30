s = input()
digit_count = 0
for char in s:
    if char.isdigit():
        digit_count += 1
print(digit_count)