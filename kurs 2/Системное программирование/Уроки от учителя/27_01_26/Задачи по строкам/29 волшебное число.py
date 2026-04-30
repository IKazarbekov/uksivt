words = [input() for _ in range(4)]
min_word = min(words)
max_word = max(words)
result = (ord(min_word[-1]) * ord(max_word[-1])) ** 2
print(result)