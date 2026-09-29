def addition(*substrings):
    r = ""
    for c in substrings:
        r += c
    return r

def clearLastChar(string):
    return string[:-2]