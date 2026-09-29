input_data = """5 0
4 5 6 7 9
2"""

#input() + "\n" + input() + "\n" + input()

str1, str2, str3 = input_data.split('\n')
n_emps, second = tuple(map(int, str1.split()))
floors = tuple((map(int, str2.split())))
index = int(str3)

min_f, max_f = min(floors), max(floors)
count = max_f - min_f
floor_emp_exit = floors[index - 1]
down = abs(min_f - floor_emp_exit)
up = abs(max_f - floor_emp_exit)

print(down, up, floor_emp_exit)
if down + 1 < second or up < second:
    print("h1")
    print(count)
else:
    print("h2")
    if down < up:
        print(down + count)
    else:
        print(up + count)
