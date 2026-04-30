class NegativeNumberError(Exception):
    '''negative number'''

    def __init__(self, value):
        self.value = value

    def __str__(self):
        return f"input invalid negative number: {self.value}"

a = int(input())
if a > 0:
    pass
else:
    raise NegativeNumberError(a)