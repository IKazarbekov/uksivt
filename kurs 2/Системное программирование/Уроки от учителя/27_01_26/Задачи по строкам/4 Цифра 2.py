s = input()
has_digit = False
for char in s:
    if char.isdigit():
        has_digit = True
        break
print("Цифра" if has_digit else "Цифр нет")