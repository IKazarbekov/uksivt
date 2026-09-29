n = int(input())
words = []
for _ in range(n):
    words.append(input())

k = int(input())
result = ''
for word in words:
    if k <= len(word):
        result += word[k - 1]
print(result)