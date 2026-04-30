count = 0
grade = int(input())

while 1 <= grade <= 5:
    if grade == 5:
        count += 1
    grade = int(input())

print(count)