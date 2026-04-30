lines = []
while True:
    line = input()
    if line == "КОНЕЦ":
        break
    lines.append(line)
if lines:
    print(f"Минимальная строка 1: {min(lines)}")
    print(f"Максимальная строка 1: {max(lines)}")