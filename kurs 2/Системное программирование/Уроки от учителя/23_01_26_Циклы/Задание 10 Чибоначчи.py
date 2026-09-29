n = int(input())

if n == 1:
    print(1)
else:
    a, b = 1, 1
    print(a, b, end=' ')

    for _ in range(n - 2):
        a, b = b, a + b
        print(b, end=' ')