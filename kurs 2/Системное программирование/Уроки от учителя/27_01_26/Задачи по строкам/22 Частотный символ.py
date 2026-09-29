s = input()
max_char = ''
max_count = 0
for char in s:
    char_count = s.count(char)
    if char_count >= max_count:
        max_count = char_count
        max_char = char
print(max_char)