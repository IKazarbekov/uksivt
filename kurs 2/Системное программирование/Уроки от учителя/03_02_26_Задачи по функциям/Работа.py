import math

# 1. Звёздный прямоугольник 1
def draw_box():
    for i in range(14):
        if i == 0 or i == 13:
            print('*' * 10)
        else:
            print('*' + ' ' * 8 + '*')

# 2. Звёздный треугольник 1
def draw_triangle():
    for i in range(1, 11):
        print('*' * i)

# 3. Равнобедренный треугольник
def draw_triangle2(fill, base):
    h = (base + 1) // 2
    for i in range(1, h + 1):
        print(fill * i)
    for i in range(h - 1, 0, -1):
        print(fill * i)

# 4. ФИО
def print_fio(name, surname, patronymic):
    result = surname[0].upper() + name[0].upper() + patronymic[0].upper()
    print(result)

# 5. Сумма цифр
def print_digit_sum(num):
    print(sum(int(d) for d in str(num)))

# 6. Конвертер километров
def convert_to_miles(km):
    return km * 0.6214

# 7. Количество дней
def get_days(month):
    days = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]
    return days[month - 1]

# 8. Делители 1
def get_factors(num):
    return [i for i in range(1, num + 1) if num % i == 0]

# 9. Делители 2
def number_of_factors(num):
    return len(get_factors(num))

# 10. Найти всех
def find_all(target, symbol):
    return [i for i in range(len(target)) if target[i] == symbol]

# 11. Merge lists 1
def merge(list1, list2):
    return sorted(list1 + list2)

# 12. Merge lists 2 (quick_merge для нескольких списков)
def quick_merge(lists):
    result = []
    for lst in lists:
        result.extend(lst)
    return sorted(result)

# 13. Is the Number Prime?
def is_prime(num):
    if num < 2:
        return False
    for i in range(2, int(num ** 0.5) + 1):
        if num % i == 0:
            return False
    return True

# 14. Next Prime
def get_next_prime(num):
    n = num + 1
    while not is_prime(n):
        n += 1
    return n

# 15. Good password
def is_password_good(password):
    if len(password) < 8:
        return False
    if not any(c.isupper() for c in password):
        return False
    if not any(c.islower() for c in password):
        return False
    if not any(c.isdigit() for c in password):
        return False
    return True

# 16. Ровно в одном
def is_one_away(word1, word2):
    if len(word1) != len(word2):
        return False
    diff = sum(1 for a, b in zip(word1, word2) if a != b)
    return diff == 1

# 17. Палиндром
def is_palindrome(text):
    text = ''.join(c.lower() for c in text if c.isalpha())
    return text == text[::-1]

# 18. Valid password BEEGEEK
def is_valid_password(password):
    parts = password.split(':')
    if len(parts) != 3:
        return False
    a, b, c = parts
    if not (a.isdigit() and b.isdigit() and c.isdigit()):
        return False
    a, b, c = int(a), int(b), int(c)
    cond1 = str(a) == str(a)[::-1]
    cond2 = is_prime(b)
    cond3 = c % 2 == 0
    return cond1 and cond2 and cond3

# 19. Правильная скобочная последовательность
def is_correct_bracket(text):
    balance = 0
    for ch in text:
        if ch == '(':
            balance += 1
        elif ch == ')':
            balance -= 1
        if balance < 0:
            return False
    return balance == 0

# 20. Середина отрезка
def get_middle_point(x1, y1, x2, y2):
    return (x1 + x2) / 2, (y1 + y2) / 2

# 21. Площадь и длина
def get_circle(radius):
    c = 2 * math.pi * radius
    s = math.pi * radius ** 2
    return c, s

import math

# ... предыдущие функции ...

# 22. Корни квадратного уравнения
def solve(a, b, c):
    d = b**2 - 4*a*c
    if d > 0:
        x1 = (-b - math.sqrt(d)) / (2*a)
        x2 = (-b + math.sqrt(d)) / (2*a)
        return sorted([x1, x2])
    elif d == 0:
        return [-b / (2*a)]
    else:
        return []

if __name__ == "__main__":
    # Примеры вызовов:
    #draw_box()
    #draw_triangle()
    #draw_triangle2('*', 9)
    #print_fio("Раиль", "Пушкин", "Сергеевич")
    #print_digit_sum(95)
    #print(convert_to_miles(1))
    #print(get_days(5))
    #print(get_factors(10))
    #print(number_of_factors(5))
    #print(find_all('abcdabcaaa', 'a'))
    #print(merge([1,2,3], [5,6,7,8]))
    #lists = [[1,2,3,4], [5,6,7], [10,11,17]]
    #print(*quick_merge(lists))
    #print(is_prime(1))
    #print(get_next_prime(14))
    #print(is_password_good('aabbcc110P'))
    #print(is_one_away('bike', 'hike'))
    #print(is_palindrome('QWEEWQ'))
    #print(is_valid_password('1221:101:252'))
    #print(is_correct_bracket('()((()))'))
    #print(get_middle_point(0,5,10,0))
    #print(get_circle(1.5))
    print(solve(-2, 7, -5))
    pass