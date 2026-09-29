import re

text = ""
with open("file.txt") as file:
    text = file.read()

match = re.findall(r"Билет \d{1,2}\D", text)
for i in range(0, 1000000):
    try:
        print(match[i])
    except:
        break