n = int(input())
original_n = n

total_sum = 0
count = 0
product = 1
last_digit = n % 10

while n >= 10:
    n //= 10
first_digit = n

n = original_n

while n > 0:
    digit = n % 10
    total_sum += digit
    count += 1
    product *= digit
    n //= 10

average = total_sum / count
sum_first_last = first_digit + last_digit

print(total_sum)
print(count)
print(product)
print(average)
print(first_digit)
print(sum_first_last)