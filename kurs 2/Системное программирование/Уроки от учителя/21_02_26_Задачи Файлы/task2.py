# file = open("students.txt", "w", encoding="utf-8")
# file.write("Казарбеков Ильяс Халимович")
# file.close()

# Задание 1 и 2
file = open("students.txt", "r", encoding="utf-8")
lines = file.read()
file.close()

students_list = lines.splitlines()
print("Список строк из файла Students.txt:")
for line in students_list:
    print(line)

print("\n" + "="*50 + "\n")

# Задание 4
file = open("info.txt", "r", encoding="utf-8")
content = file.read()
file.close()

print("Содержимое файла info.txt:")
print(content)

print("\n" + "="*50 + "\n")

# Задание 5
with open("students.txt", "r", encoding="utf-8") as file:
    content = file.read()
    print("Содержимое Students.txt (менеджер контекста):")
    print(content)

print("\n" + "="*50 + "\n")

# Задание 6
file = open("students.txt", "r", encoding="utf-8")
content = file.read()
print("Содержимое Students.txt (первое чтение):")
print(content)

file.seek(0)
content_again = file.read()
print("\nСодержимое Students.txt (после перемещения курсора):")
print(content_again)

file.close()