try:
    file = open(input(), 'r')
    print(file.read())
except:
    print("ERROR")
finally:
    file.close()
