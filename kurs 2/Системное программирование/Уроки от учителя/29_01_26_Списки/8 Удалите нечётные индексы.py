n = int(input())
lst = []
for _ in range(n):
    lst.append(int(input()))

i = 1
while i < len(lst):
    del lst[i]
    i += 1
print(lst)