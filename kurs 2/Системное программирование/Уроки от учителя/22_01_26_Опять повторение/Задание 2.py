input_second = int(input("Кол-во секунд с начало суток: "))

hour = input_second // 60 // 60
minute = input_second // 60 % 60
second = input_second % 60 % 60

result = f"{hour}:{minute}:{second}"

print("Сейчас время: ", result)