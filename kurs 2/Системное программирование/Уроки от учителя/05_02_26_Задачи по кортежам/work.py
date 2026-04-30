# ===== 1.0 Вершина параболы =====
def task_parabola():
    a = int(input())
    b = int(input())
    c = int(input())
    x0 = -b / (2 * a)
    y0 = (4 * a * c - b * b) / (4 * a)
    print((x0, y0))

# ===== 1.1 Конкурсный отбор =====
def task_selection():
    n = int(input())
    students = [input() for _ in range(n)]
    for s in students:
        print(s)
    print()
    for s in students:
        if s[-1] in '45':
            print(s)

# ===== 1.2 Последовательность Трибоначчи =====
def task_tribonacci():
    n = int(input())
    t = (1, 1, 1)
    if n == 1:
        print(1)
    elif n == 2:
        print(1, 1)
    elif n == 3:
        print(1, 1, 1)
    else:
        print(1, 1, 1, end=' ')
        a, b, c = 1, 1, 1
        for _ in range(3, n):
            d = a + b + c
            print(d, end=' ')
            a, b, c = b, c, d
        print()

# ===== 2. Тимур и его команда =====
def task_timur():
    n = int(input())
    m = int(input())
    k = int(input())
    x = int(input())
    y = int(input())
    z = int(input())
    sea = n
    village = m
    mountains = k
    sea_and_village = x
    village_and_mountains = y
    only_village = village - sea_and_village - village_and_mountains
    only_sea = sea - sea_and_village
    only_mountains = mountains - village_and_mountains
    total = only_sea + only_village + only_mountains + sea_and_village + village_and_mountains + z
    print(total)

# ===== 3. Книги на прочтение =====
def task_books():
    n = int(input())
    m = int(input())
    k = int(input())
    x = int(input())
    y = int(input())
    z = int(input())
    t = int(input())
    a = int(input())
    only_two = (n + m - x - t) + (m + k - y - t) + (n + k - z - t)
    only_one = (n - (n + m - x) - (n + k - z) + t) + (m - (n + m - x) - (m + k - y) + t) + (k - (m + k - y) - (n + k - z) + t)
    none = a - (only_one + only_two + t)
    print(only_one)
    print(only_two)
    print(none)

# ===== 4. Количество различных символов =====
def task_count_unique():
    s = input()
    print(len(set(s)))

# ===== 5. Неповторимые цифры =====
def task_unique_digits():
    s = input()
    print("YES" if len(set(s)) == len(s) else "NO")

# ===== 6. Все 10 цифр =====
def task_all_digits():
    s1 = input()
    s2 = input()
    print("YES" if len(set(s1 + s2)) == 10 else "NO")

# ===== 7. Одинаковые наборы =====
def task_same_sets():
    s1 = input()
    s2 = input()
    print("YES" if set(s1) == set(s2) else "NO")

# ===== 8. Три слова =====
def task_three_words():
    w1, w2, w3 = input().split()
    print("YES" if set(w1) == set(w2) == set(w3) else "NO")

# ===== 9. Уникальные символы 1 =====
def task_unique_per_word():
    n = int(input())
    for _ in range(n):
        word = input().lower()
        print(len(set(word)))

# ===== 10. Уникальные символы 2 =====
def task_unique_total():
    n = int(input())
    total_set = set()
    for _ in range(n):
        word = input().lower()
        total_set.update(word)
    print(len(total_set))

# ===== 11. Количество слов в тексте =====
def task_unique_words():
    import string
    s = input().lower()
    for p in string.punctuation:
        s = s.replace(p, ' ')
    words = s.split()
    print(len(set(words)))

# ===== 12. Встречалось ли число раньше? =====
def task_seen_before():
    nums = input().split()
    seen = set()
    for num in nums:
        n = str(int(num))  # убираем ведущие нули
        if n in seen:
            print("YES")
        else:
            print("NO")
            seen.add(n)

# ===== 13. Количество совпадающих =====
def task_count_common():
    set1 = set(map(int, input().split()))
    set2 = set(map(int, input().split()))
    print(len(set1 & set2))

# ===== 14. Общие числа =====
def task_common_numbers():
    set1 = set(map(int, input().split()))
    set2 = set(map(int, input().split()))
    print(*sorted(set1 & set2))

# ===== 15. Числа первой строки =====
def task_first_only():
    set1 = set(map(int, input().split()))
    set2 = set(map(int, input().split()))
    print(*sorted(set1 - set2))

# ===== 16. Общие цифры =====
def task_common_digits():
    n = int(input())
    digits_sets = [set(input().strip()) for _ in range(n)]
    common = set.intersection(*digits_sets)
    if common:
        print(*sorted(common))

# ===== 17. Одинаковые цифры =====
def task_same_digit_in_numbers():
    a = input()
    b = input()
    print("YES" if set(a) & set(b) else "NO")

# ===== 18. Все цифры =====
def task_all_digits_in_first():
    a = input()
    b = input()
    print("YES" if set(b).issubset(set(a)) else "NO")

# ===== 19. Урок информатики =====
def task_informatics():
    s1 = set(map(int, input().split()))
    s2 = set(map(int, input().split()))
    s3 = set(map(int, input().split()))
    res = (s1 & s2) - s3
    print(*sorted(res, reverse=True))

# ===== 20. Урок математики =====
def task_mathematics():
    from collections import Counter
    all_grades = list(map(int, input().split())) + list(map(int, input().split())) + list(map(int, input().split()))
    freq = Counter(all_grades)
    res = {grade for grade, count in freq.items() if count <= 2}
    print(*sorted(res))

# ===== 21. Урок физики =====
def task_physics():
    s1 = set(map(int, input().split()))
    s2 = set(map(int, input().split()))
    s3 = set(map(int, input().split()))
    res = s3 - (s1 | s2)
    print(*sorted(res, reverse=True))

# ===== 22. Урок биологии =====
def task_biology():
    s1 = set(map(int, input().split()))
    s2 = set(map(int, input().split()))
    s3 = set(map(int, input().split()))
    all_given = s1 | s2 | s3
    all_possible = set(range(11))
    res = all_possible - all_given
    print(*sorted(res))

# ===== 23. Строковое представление =====
def task_number_to_words():
    d = {
        '0': 'zero', '1': 'one', '2': 'two', '3': 'three', '4': 'four',
        '5': 'five', '6': 'six', '7': 'seven', '8': 'eight', '9': 'nine'
    }
    num = input()
    print(' '.join(d[ch] for ch in num))

# ===== 24. Информация об учебных курсах =====
def task_course_info():
    courses = {
        "CS101": "3004, Хайнс, 8:00",
        "CS102": "4501, Альварадо, 9:00",
        "CS103": "6755, Рич, 10:00",
        "NT110": "1244, Берк, 11:00",
        "CM241": "1411, Ли, 13:00"
    }
    code = input()
    print(f"{code}: {courses[code]}")

# ===== 25. Словарь программиста =====
def task_programmer_dict():
    n = int(input())
    d = {}
    for _ in range(n):
        key, val = input().split(": ")
        d[key.lower()] = val
    m = int(input())
    for _ in range(m):
        word = input().lower()
        print(d.get(word, "Не найдено"))

# ===== 26. Анаграммы 1 =====
def task_anagram1():
    w1 = input()
    w2 = input()
    print("YES" if sorted(w1) == sorted(w2) else "NO")

# ===== 27. Анаграммы 2 =====
def task_anagram2():
    import string
    s1 = input().lower()
    s2 = input().lower()
    for ch in string.punctuation + ' ':
        s1 = s1.replace(ch, '')
        s2 = s2.replace(ch, '')
    print("YES" if sorted(s1) == sorted(s2) else "NO")

# ===== 28. Словарь синонимов =====
def task_synonyms():
    n = int(input())
    d = {}
    for _ in range(n):
        a, b = input().split()
        d[a] = b
        d[b] = a
    word = input()
    print(d[word])

# ===== 29. Страны и города =====
def task_countries_cities():
    n = int(input())
    mapping = {}
    for _ in range(n):
        parts = input().split()
        country = parts[0]
        for city in parts[1:]:
            mapping[city] = country
    m = int(input())
    for _ in range(m):
        city = input()
        print(mapping[city])

# ===== 30. Секретное слово =====
def task_secret_word():
    encrypted = input()
    n = int(input())
    freq_map = {}
    for _ in range(n):
        letter, freq = input().split(": ")
        freq_map[int(freq)] = letter
    # подсчитываем частоты букв в зашифрованном слове
    from collections import Counter
    counts = Counter(encrypted)
    # строим расшифрованное слово
    decrypted = []
    for ch in encrypted:
        freq = counts[ch]
        decrypted.append(freq_map[freq])
    print(''.join(decrypted))

# ==== Выбор задачи ====
task = input("Введите номер задачи (1-30): ")
if task == '1':
    task_tribonacci()
elif task == '2':
    task_timur()
elif task == '3':
    task_books()
elif task == '4':
    task_count_unique()
elif task == '5':
    task_unique_digits()
elif task == '6':
    task_all_digits()
elif task == '7':
    task_same_sets()
elif task == '8':
    task_three_words()
elif task == '9':
    task_unique_per_word()
elif task == '10':
    task_unique_total()
elif task == '11':
    task_unique_words()
elif task == '12':
    task_seen_before()
elif task == '13':
    task_count_common()
elif task == '14':
    task_common_numbers()
elif task == '15':
    task_first_only()
elif task == '16':
    task_common_digits()
elif task == '17':
    task_same_digit_in_numbers()
elif task == '18':
    task_all_digits_in_first()
elif task == '19':
    task_informatics()
elif task == '20':
    task_mathematics()
elif task == '21':
    task_physics()
elif task == '22':
    task_biology()
elif task == '23':
    task_number_to_words()
elif task == '24':
    task_course_info()
elif task == '25':
    task_programmer_dict()
elif task == '26':
    task_anagram1()
elif task == '27':
    task_anagram2()
elif task == '28':
    task_synonyms()
elif task == '29':
    task_countries_cities()
elif task == '30':
    task_secret_word()