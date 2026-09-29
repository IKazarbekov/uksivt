n = int(input())
lines = []
for _ in range(n):
    lines.append(input())

k = int(input())
queries = []
for _ in range(k):
    queries.append(input().lower())

for line in lines:
    lower_line = line.lower()

    found_all = True
    for query in queries:
        if query not in lower_line:
            found_all = False
            break

    if found_all:
        print(line)