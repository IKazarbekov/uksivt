n = int(input())
last_digit = n % 10
all_same = True

while n > 0:
    digit = n % 10
    if digit != last_digit:
        all_same = False
        break
    n //= 10

print("YES" if all_same else "NO")