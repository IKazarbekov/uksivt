X = float(input("X кг конфет = "))
A = float(input("Стоимость конфет = "))
Y = float(input("Y кг ирисок = "))
B = float(input("Стоимость ирисок = "))

price_candy = A / X
price_toffee = B / Y
result = price_candy / price_toffee

print(f"1 кг конфет: {price_candy:.2f} руб.")
print(f"1 кг ирисок: {price_toffee:.2f} руб.")
print(f"Конфеты дороже в {result:.2f} раз")