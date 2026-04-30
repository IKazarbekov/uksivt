
n = int(input())
books = []

for _ in range(n):
    book = input()
    surname = book.split()[0]

    start = book.find('<')
    end = book.find('>')
    title = book[start + 1:end]

    books.append((surname, title))

sorted_flag = True
for i in range(1, n):
    prev_surname, prev_title = books[i - 1]
    curr_surname, curr_title = books[i]

    if prev_surname > curr_surname:
        sorted_flag = False
        break
    elif prev_surname == curr_surname and prev_title > curr_title:
        sorted_flag = False
        break

print("YES" if sorted_flag else "NO")