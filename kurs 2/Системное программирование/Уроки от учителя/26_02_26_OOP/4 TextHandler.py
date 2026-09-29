class TextHundler:
    def __init__(self):
        self.__words__ = list()

    def add_words(self, text):
        self.__words__.extend(text.split())

    def get_shortest_words(self):
        result = self.__words__.copy()
        count = len(result) // 2
        for i in range(count):
            result.remove(
                max(result,
                    key=len)
            )
        return result

    def get_longest_words(self):
        result = self.__words__.copy()
        count = len(result) // 2
        for i in range(count):
            result.remove(
                min(result,
                    key=len)
            )
        return result

text_hundler = TextHundler()

text_hundler.add_words("Privet world! how are you?")

print("short words")
print(text_hundler.get_shortest_words())

print("long words")
print(text_hundler.get_longest_words())

