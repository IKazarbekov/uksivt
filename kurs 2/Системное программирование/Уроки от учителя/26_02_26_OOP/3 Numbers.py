class Numbers:
    def __init__(self):
        self.numbers = []

    def add_number(self, num):
        self.numbers.append(num)

    def get_even(self):
        return [num for num in self.numbers if num % 2 == 0]

    def get_odd(self):
        return [num for num in self.numbers if num % 2 != 0]