import json
from json import JSONDecoder


class Book:
    def __init__(self, title, author, year):
        self.title = title
        self.author = author
        self.year = year

def book_to_json(book):
    if isinstance(book, Book):
        return json.dumps({'title':book.title, 'author':book.author, 'year':book.year}, ensure_ascii=False)
    raise Exception()

my_book = Book("Мастер и Маргарита", "М.А. Булгаков", 1967)
json = book_to_json(my_book)
print(json)