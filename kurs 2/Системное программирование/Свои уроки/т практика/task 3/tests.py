from resolution import function

#input() + "\n" + input() + "\n" + input()

print("TEST 1")
input_data = """5 0
4 5 6 7 9
2"""
answer = function(input_data)
print(f"answer:{answer}")
if answer == 6:
    print("yes")
else:
    print("ERROR")


print("TEST 2")
input_data = """5  5
1  4  9  16  25
2"""
answer = function(input_data)
print(f"answer:{answer}")
if answer == 24:
    print("yes")
else:
    print("ERROR")

print("TEST 3")
input_data = """6  4
1  2  3  6  8  25
5"""
answer = function(input_data)
print(f"answer:{answer}")
if answer == 31:
    print("yes")
else:
    print("ERROR")