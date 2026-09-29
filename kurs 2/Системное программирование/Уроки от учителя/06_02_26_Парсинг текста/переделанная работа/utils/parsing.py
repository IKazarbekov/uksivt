import re

def parsing_simple(input_text):
    #print('        begin parsing line')
    text = str(input_text)
    # example 34.1 + 64
    pattern_simple = (''
                    '\d+\.?\d*'    # double
                    ' *'              # space
                    '[\-+]'     # operation + -
                    ' *'              # space
                    '\d+\.?\d*'
                    '')       # double

    pattern_simple_priory0 = (''
                    '\d+\.?\d*'    # double
                    '\^'            # operation + - * / ^
                    '\d+\.?\d*')   # double

                              #pattern_simple_priory1 = r"(?<!\d/)\d+(?:\.\d+)?\s[*/]\s\d+(?:\.\d+)?(?!\d/)"
    pattern_simple_priory1 = (
                              r"\d+\.?\d*"
                              r" *"
                              r"[*/]"
                              r" *"
                              r"\d+\.?\d*")

    while True:
        match_ = re.search(pattern_simple_priory0, text)    # find ^
        if match_ is None:
            match_ = re.search(pattern_simple_priory1, text)
            if match_ is None:
                 match_ = re.search(pattern_simple, text)
            if match_ is None :
                    break
        span = match_.span()

        # get substring
        begin, end = span[0], span[1]
        substring = text[begin: end]
        # find numbers and operation
        number_first, number_second = re.findall('\d+\.?\d*', substring)
        operation = re.findall('[+\-*/^]', substring)[0]

        # string to float
        number_first = float(number_first)
        number_second = float(number_second)

        # calculate result
        result = number_first
        match operation:
            case '+':
                result += number_second
            case '-':
                result -= number_second
            case '*':
                result *= number_second
            case '/':
                result /= number_second
            case '^':
                result **= number_second


        text = text[:begin] + str(result) + text[end:]

    #print('        end rapsing line')
    return text

# find skobka
def parsing_skobla(input_text):
    #print('    begin parsing lines')
    text = str(input_text)

    begins = []
    count = 0

    i = 0
    while i < len(text):
        char = text[i]
        if (char.isalpha() and count != 0) or (char == ')' and count == 0):
            count = 0
            begins.clear()
            break
        if char == '(':
            begins.append(i)
            count += 1
        if char == ')':
            try:
                end = i
                substring = text[begins[-1] + 1: end]
                prob_text = parsing_simple(substring)
                text = text[:begins[-1]] + prob_text + text[end + 1:]
                i -= len(substring)
                begins.pop(-1)
                count -= 1
            except: pass
        i += 1

    if count == 0:
        text = parsing_simple(text)
    #print('    end parsing lines')
    return text

# find potential
def parsing_lines(lines: list):
    lst_pattern_error = ['\d{2}[/\-]\d{2}[/\-]\d{4}', '[+]\d\-\(\d{3}\)\-\d{3}\-\d{2}\-\d{2}',
                         '[+]?\d+[\- ]\(?\d+\)?[\- ]\d+[\- ]\d+', '\d{4}[/\-]\d{2}[/\-]\d{2}']

    result_lines = list()
    for line in lines:
        # find error line
        flag_potencal = True
        for pattern_error in lst_pattern_error:
            match_ = re.search(pattern_error, line)
            if match_ != None:
                flag_potencal = False
                break

        if flag_potencal:
            r_line = parsing_skobla(line)
            result_lines.append(r_line)
        else:
            result_lines.append(line)
    return result_lines

# test module
if __name__ == '__main__':
    #text = '5 + 6 + 3 * ( 2.3 + ( 3^2 - 3   * 2 ) - (3 * 1)) goood boy 9 yeee no 40 / 4 '
    text = '(32+1768)/ 45'
    print(text)
    result = parsing_skobla(text)
    print('result:' ,result)
