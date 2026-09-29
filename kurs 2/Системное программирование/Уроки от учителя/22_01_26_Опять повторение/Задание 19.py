A = float(input("A: "))
B = float(input("B: "))
C = float(input("C: "))

old_A = A
old_B = B
old_C = C

A = old_B
B = old_C
C = old_A

print(f"A = {A}")
print(f"B = {B}")
print(f"C = {C}")