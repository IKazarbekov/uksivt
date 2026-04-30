import time

print("STEP 1")
def create_generator(x):
    return (i for i in range(x, 0, -1))
generator = create_generator(524)
for i in generator:
    print(i)

generator2 = create_generator(200)
print(generator2.__next__())
print(next(generator2))
print(generator2.__next__())
print(next(generator2))
try:
    print(generator2.__next__())
except StopIteration as s:
    print(s.value)
