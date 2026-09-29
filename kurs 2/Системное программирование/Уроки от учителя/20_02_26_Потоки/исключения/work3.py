try:
    with open(input(),'r') as file:
        print(file.read())
except FileNotFoundError:
    print("File not found")