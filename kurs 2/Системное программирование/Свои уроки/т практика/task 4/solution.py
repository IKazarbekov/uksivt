def sort_key_by_str_len(number: int):
    string = str(number)
    return len(string)

def sort_key_by_figure(number: int):
    string = str(number)
    return 10 ** len(string) - number

def get_answer(input_string: str):
    str1, str2 = tuple(input_string.split("\n"))
    count_numbers, count_step = tuple(map(int, str1.split()))
    numbers = tuple(map(int, str2.split()))

    #1 max length string
    #2 min first figure

    sorted_numbers = sorted(numbers, key=sort_key_by_str_len)[::-1]
    sorted_numbers = sorted(sorted_numbers, key=sort_key_by_figure)

    print(sorted_numbers)

if __name__ == '__main__':
    print(sort_key_by_figure(1452))