address = input().split('.')
valid = True

for num in address:
    if not (0 <= int(num) <= 255):
        valid = False
        break

print("ДА" if valid else "НЕТ")