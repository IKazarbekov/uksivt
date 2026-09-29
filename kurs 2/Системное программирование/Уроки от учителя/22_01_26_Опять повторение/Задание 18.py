A = float(input("A: "))
B = float(input("B: "))
C = float(input("C: "))

temp_A = A
temp_B = B
temp_C = C

A = temp_C
B = temp_A
C = temp_B

print("Новые значения:")
print(f"A = {A}")
print(f"B = {B}")
print(f"C = {C}")