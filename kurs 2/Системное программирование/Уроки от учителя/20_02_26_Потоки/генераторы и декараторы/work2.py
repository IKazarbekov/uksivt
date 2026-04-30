import time


def fibonaci():
    a, b = 0, 1
    while True:
        yield a
        a, b = b, a + b

fib = fibonaci()
for i in range(10):
    print(next(fib))

for i in fib:
    print(i)
    time.sleep(1)
