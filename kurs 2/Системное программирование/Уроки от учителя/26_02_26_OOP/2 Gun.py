class Gun:
    def __init__(self):
        self.counter = 0

    def shoot(self):
        self.counter += 1
        if self.counter % 2 == 1:
            print('pif')
        else:
            print('paf')