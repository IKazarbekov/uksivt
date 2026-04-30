year = int(input("Год: "))

while year > 2011:
    year -= 12

while year < 2000:
    year += 12

print("Это год:", end="")
match year:
    case 2000:
        print("Дракон")
    case 2001:
        print("Змея")
    case 2002:
        print("Лошадь")
    case 2003:
        print("Овца")
    case 2004:
        print("Обезьяна")
    case 2005:
        print("Петух")
    case 2006:
        print("Собака")
    case 2007:
        print("Свинья")
    case 2008:
        print("Крыса")
    case 2009:
        print("Бык")
    case 2010:
        print("Тигр")
    case 2011:
        print("Заяц")