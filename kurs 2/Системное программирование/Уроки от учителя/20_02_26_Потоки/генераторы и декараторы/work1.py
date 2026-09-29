def generator(path):
    with open(path, 'r') as file:
        for line in file:
            yield line

for string in generator('work2.py'):
    print(string)