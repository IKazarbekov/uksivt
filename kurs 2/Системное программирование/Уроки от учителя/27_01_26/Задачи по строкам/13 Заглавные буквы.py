name = input()
parts = name.split()
if len(parts) == 2 and parts[0][0].isupper() and parts[1][0].isupper():
    print("YES")
else:
    print("NO")