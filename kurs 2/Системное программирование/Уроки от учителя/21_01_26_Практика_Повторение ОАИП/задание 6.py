text = input("Число натаральное 5 или 6 значное: ")

while not text.isdigit() or not 5 <= len(text) < 7 or [0] == '0':
    print("не  верно дано число")
    text = input("Число натаральное 5 или 6 значное: ")

length = len(text)
if length == 6:
    print(text[0] + text[:-6: -1])
else:
    print(text[:-6: -1])