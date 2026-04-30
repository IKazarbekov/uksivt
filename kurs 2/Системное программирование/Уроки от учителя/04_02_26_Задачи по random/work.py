import random

# 1. Броски монеты
n = int(input())
for _ in range(n):
    print('Орел' if random.randint(0, 1) == 0 else 'Решка')

# 2. Броски кубика
n = int(input())
for _ in range(n):
    print(random.randint(1, 6))

# 3. Случайный пароль из букв
length = int(input())
password = ''
for _ in range(length):
    if random.randint(0, 1) == 0:
        password += chr(random.randint(65, 90))
    else:
        password += chr(random.randint(97, 122))
print(password)

# 4. Лотерейный билет (7 чисел от 1 до 49)
numbers = random.sample(range(1, 50), 7)
print(' '.join(map(str, sorted(numbers))))

# 5. Генерация IP-адреса
def generate_ip():
    return '.'.join(str(random.randint(0, 255)) for _ in range(4))

# 6. Генерация почтового индекса Латверии
def generate_index():
    letters = [chr(random.randint(65, 90)) for _ in range(4)]
    numbers = [str(random.randint(0, 99)).zfill(2) for _ in range(2)]
    return f'{letters[0]}{letters[1]}{numbers[0]}_{numbers[1]}{letters[2]}{letters[3]}'

# 7. 100 различных лотерейных билетов
tickets = random.sample(range(1000000, 10000000), 100)
for ticket in tickets:
    print(ticket)

# 8. Случайная анаграмма слова
word = input()
letters = list(word)
random.shuffle(letters)
print(''.join(letters))

# 9. Карточка для бинго
numbers = random.sample(range(1, 76), 24)
card = []
index = 0
for i in range(5):
    row = []
    for j in range(5):
        if i == 2 and j == 2:
            row.append(0)
        else:
            row.append(numbers[index])
            index += 1
    card.append(row)
for row in card:
    print(' '.join(str(num).rjust(3) for num in row))

# 10. Генератор паролей (без похожих символов)
def generate_password(length):
    chars = 'abcdefghjkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789'
    return ''.join(random.choice(chars) for _ in range(length))

def generate_passwords(count, length):
    return [generate_password(length) for _ in range(count)]

n = int(input())
m = int(input())
passwords = generate_passwords(n, m)
for p in passwords:
    print(p)