f = float(input("Вещественное число: "))

strs = str(f).split(".")

result = strs[1][1]

print(result)