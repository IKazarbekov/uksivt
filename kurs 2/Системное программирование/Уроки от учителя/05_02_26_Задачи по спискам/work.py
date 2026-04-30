# ===== 1. Треугольник Паскаля =====
def task_pascal():
    n = int(input())
    triangle = [[1]]
    for i in range(1, n):
        row = [1]
        for j in range(1, i):
            row.append(triangle[i-1][j-1] + triangle[i-1][j])
        row.append(1)
        triangle.append(row)
    for row in triangle:
        print(*row)

# ===== 2. Упаковка дубликатов =====
def task_pack():
    s = input().split()
    result = []
    temp = [s[0]]
    for i in range(1, len(s)):
        if s[i] == s[i-1]:
            temp.append(s[i])
        else:
            result.append(temp)
            temp = [s[i]]
    result.append(temp)
    print(result)

# ===== 3. Вывести матрицу 1 =====
def task_matrix1():
    n = int(input())
    m = int(input())
    matrix = []
    for i in range(n):
        row = []
        for j in range(m):
            row.append(input())
        matrix.append(row)
    for i in range(n):
        print(*matrix[i])

# ===== 4. Вывести матрицу 2 =====
def task_matrix2():
    n = int(input())
    m = int(input())
    matrix = []
    for i in range(n):
        row = []
        for j in range(m):
            row.append(input())
        matrix.append(row)
    for i in range(n):
        print(*matrix[i])
    print()
    for j in range(m):
        row = [matrix[i][j] for i in range(n)]
        print(*row)

# ===== 5. Больше среднего =====
def task_above_avg():
    n = int(input())
    for _ in range(n):
        row = list(map(int, input().split()))
        avg = sum(row) / n
        count = sum(1 for x in row if x > avg)
        print(count)

# ===== 6. Таблица умножения =====
def task_mult_table():
    n = int(input())
    m = int(input())
    for i in range(n):
        for j in range(m):
            print(str(i*j).ljust(3), end='')
        print()

# ===== 7. Максимум в таблице =====
def task_max_index():
    n = int(input())
    m = int(input())
    max_val = -10**9
    max_i = 0
    max_j = 0
    for i in range(n):
        row = list(map(int, input().split()))
        for j in range(m):
            if row[j] > max_val:
                max_val = row[j]
                max_i, max_j = i, j
    print(max_i, max_j)

# ===== 8. Обмен столбцов =====
def task_swap_cols():
    n = int(input())
    m = int(input())
    matrix = [list(map(int, input().split())) for _ in range(n)]
    i, j = map(int, input().split())
    for r in range(n):
        matrix[r][i], matrix[r][j] = matrix[r][j], matrix[r][i]
    for row in matrix:
        print(*row)

# ===== 9. Симметричная матрица =====
def task_symmetric():
    n = int(input())
    matrix = [list(map(int, input().split())) for _ in range(n)]
    for i in range(n):
        for j in range(i+1, n):
            if matrix[i][j] != matrix[j][i]:
                print("NO")
                return
    print("YES")

# ===== 10. Обмен диагоналей =====
def task_swap_diag():
    n = int(input())
    matrix = [list(map(int, input().split())) for _ in range(n)]
    for i in range(n):
        j = n-1-i
        matrix[i][i], matrix[i][j] = matrix[i][j], matrix[i][i]
    for row in matrix:
        print(*row)

# ===== 11. Зеркальное отображение =====
def task_mirror_h():
    n = int(input())
    matrix = [list(map(int, input().split())) for _ in range(n)]
    for i in range(n//2):
        matrix[i], matrix[n-1-i] = matrix[n-1-i], matrix[i]
    for row in matrix:
        print(*row)

# ===== 12. Поворот матрицы =====
def task_rotate_90():
    n = int(input())
    matrix = [list(map(int, input().split())) for _ in range(n)]
    rotated = [[matrix[n-1-j][i] for j in range(n)] for i in range(n)]
    for row in rotated:
        print(*row)

# ===== 13. Заполнение 1 =====
def task_fill1():
    n, m = map(int, input().split())
    val = 1
    matrix = [[0]*m for _ in range(n)]
    for i in range(n):
        for j in range(m):
            matrix[i][j] = val
            val += 1
    for row in matrix:
        print(' '.join(str(x).ljust(3) for x in row))

# ===== 14. Заполнение змейкой =====
def task_snake():
    n, m = map(int, input().split())
    matrix = [[0]*m for _ in range(n)]
    val = 1
    for i in range(n):
        if i % 2 == 0:
            for j in range(m):
                matrix[i][j] = val
                val += 1
        else:
            for j in range(m-1, -1, -1):
                matrix[i][j] = val
                val += 1
    for row in matrix:
        print(' '.join(str(x).ljust(3) for x in row))

# ==== Выбор задачи ====
task = input("Введите номер задачи (1-14): ")
if task == '1':
    task_pascal()
elif task == '2':
    task_pack()
elif task == '3':
    task_matrix1()
elif task == '4':
    task_matrix2()
elif task == '5':
    task_above_avg()
elif task == '6':
    task_mult_table()
elif task == '7':
    task_max_index()
elif task == '8':
    task_swap_cols()
elif task == '9':
    task_symmetric()
elif task == '10':
    task_swap_diag()
elif task == '11':
    task_mirror_h()
elif task == '12':
    task_rotate_90()
elif task == '13':
    task_fill1()
elif task == '14':
    task_snake()