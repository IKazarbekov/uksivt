hypotenuse = int(input("Введите гипотенузу: "))
catheter = int(input("Введите катет: "))

second_catheter = ( hypotenuse ** 2 - catheter ** 2 ) ** 0.5

print("Второй катет равен:", second_catheter)