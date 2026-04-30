#1
print("STEP 1====================")
def func_generator():
    yield 1
    yield 5
    yield 9

generator = func_generator()
print(generator.__next__())
print(next(generator))
print(generator.__next__())

for i in func_generator():
    print(i * 2)


print("STEP 2====================")
#generator.__next__()
def func_generator():
    try:
        yield 1
        yield 5
        return 999#None
    except GeneratorExit:
        print("EXIT")

gen = func_generator()
gen.__next__()
#gen.__next__()
gen.close()
try:
    gen.__next__()
except StopIteration as e:
    print(e.value)



print("STEP 3====================")

generator_func = (1 + r * 4 for r in range(3))

for i in generator_func:
    print(i)