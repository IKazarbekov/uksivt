n = int(input())
valid_letters = "АБВГДЕЁЖЗИЙКЛМНОП"
for _ in range(n):
    class_name = input()
    if (len(class_name) == 2 and
        class_name[0].isdigit() and
        0 <= int(class_name[0]) <= 9 and
        class_name[1] in valid_letters):
        print("YES")
    else:
        print("NO")