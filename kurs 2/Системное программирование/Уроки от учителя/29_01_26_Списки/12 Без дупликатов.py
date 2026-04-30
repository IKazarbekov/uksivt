n = int(input())
seen = []
for _ in range(n):
    line = input()
    if line not in seen:
        seen.append(line)
        print(line)