s = input()
count = s.count('f')
if count == 0:
    print("NO")
elif count == 1:
    print(s.find('f'))
else:
    print(s.find('f'), s.rfind('f'))