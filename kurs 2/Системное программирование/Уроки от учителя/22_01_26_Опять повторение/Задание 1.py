hour = int(input("Кол-во часов:"))
minute = int(input("Кол-во минут:"))
second = int(input("Кол-во секунд:"))

result_second = hour * 60 * 60 + minute * 60 + second

print("Всего секунд с начало дня:", result_second)