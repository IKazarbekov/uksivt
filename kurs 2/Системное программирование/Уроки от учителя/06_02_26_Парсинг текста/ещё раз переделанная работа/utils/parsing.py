import re

# error -

def parsing_line(lines):
    #print('        begin parsing line')
    text = lines
    # example 34.1 + 64
    pattern_simple = (''
                    '\d+\.?\d*'    # double
                    ' *'              # space
                    '[\-,+]'     # operation + -
                    ' *'              # space
                    '\d+\.?\d*'
                    '')       # double

    pattern_simple_priory0 = (''
                    '\d+'                   # double
                    '\^'                       # operation + - * / ^
                    '\d+')               # double

    #pattern_simple_priory1 = r"(?<!\d/)\d+(?:\.\d+)?\s[*/]\s\d+(?:\.\d+)?(?!\d/)"
    pattern_simple_priory1 = (r"(?<!\d/)"
                              r"\d+.?\d*"
                              r"?\s"
                              r"[*/]"
                              r"\s"
                              r"\d+.?\d*"
                              r"(?!/d)")

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

        # insert result in text
        if len(str(result)) > 15:
            text = text[:begin] + f'{result:.2e}' + text[end:]
        else:
            text = text[:begin] + str(result) + text[end:]

    #print('        end rapsing line')
    return text

# find skobka
def parsing_lines(lines):
    #print('    begin parsing lines')
    text = ''.join(lines)

    begins = []
    count = 0

    i = 0
    while i < len(text):
        print(text)
        char = text[i]
        if (char.isalpha() and count != 0) or (char == ')' and count == 0):
            text = text[:i] + 'Error skobka' + text[i:]
            count = 0
            begins.clear()
            continue
        if char == '(':
            begins.append(i)
            count += 1
        if char == ')':
            try:
                end = i
                substring = text[begins[-1] + 1: end]
                prob_text = parsing_line(substring)
                text = text[:begins[-1]] + prob_text + text[end + 1:]
                i -= len(substring)
                begins.pop(-1)
                count -= 1
            except: pass
        i += 1


    text = parsing_line(text)
    #print('    end parsing lines')
    return text

# find potential
def parsing_text(lines):
    result_lines = list()
    for line in lines:
        line = parsing_lines(line)
    return ''.join(lines)


# test module
if __name__ == '__main__':
    text = '5 + 6 + 3 * ( 2.3 + ( 3^2 - 3   * 2 ) - (3 * 1)) goood boy 9 yeee no 40 / 4 '
    print(text)
    result = parsing_lines(text)
    print('result:' ,result)
