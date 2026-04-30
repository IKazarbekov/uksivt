def function(inputs: str):
    str1, str2, str3 = inputs.split('\n')
    n_emps, second = tuple(map(int, str1.split()))
    floors = tuple((map(int, str2.split())))
    index = int(str3)

    min_f, max_f = min(floors), max(floors)
    count = max_f - min_f
    floor_emp_exit = floors[index - 1]
    down = abs(min_f - floor_emp_exit)
    up = abs(max_f - floor_emp_exit)

    if down + 1 < second or up < second:
        return count
    else:
        if down < up:
            return  down + count
        else:
            return up + count

if __name__ == "__main__":
    print((function(input() + "\n" + input() + "\n" + input())))