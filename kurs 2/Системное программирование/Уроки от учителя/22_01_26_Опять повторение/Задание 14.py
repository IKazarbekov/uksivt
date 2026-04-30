X = float(input("X кг = "))
A = float(input("Стоимость X кг = "))
Y = float(input("Y кг = "))

price_kg = A / X
result = price_kg * Y

print(f"1 кг: {price_kg:.2f} руб.")
print(f"{Y} кг: {result:.2f} руб.")