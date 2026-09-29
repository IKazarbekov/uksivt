from enum import Enum

class Color(Enum):
    BLACK = 1
    WHITE = 2

class Knight:
    def __init__(self, horizontal, vertical, color):
        self.__hor__ = horizontal.lower()
        self.__ver__ = vertical
        self.__col__ = color

    def get_char(self):
        return '#'

    def can_move(self, h, v):
        x = (ord(self.__hor__) - ord('A')) - (ord(h) - ord('A'))
        y = self.__ver__ - v
        if abs(x) == 1 and abs(y) == 2:
            return True
        elif abs(x) == 2 and abs(y) == 1:
            return True
        return False

    def move_to(self, h, v):
        if self.can_move(h, v):
            self.__hor__ = h
            self.__ver__ = v

    def draw_board(self):
        print(" abcdefgh")
        for y in range(1, 9):
            print(y, end='')
            for x in range(8):
                if x == ord(self.__hor__) - ord('a') and y == self.__ver__:
                    if self.__col__ == Color.BLACK:
                        print('♘' ,end="")
                    else:
                        print('♞' ,end="")
                elif self.can_move(chr(x + ord('a')), y):
                    print("X", end="")
                else:
                    r = (y + x) % 2
                    if r == 0:
                        print("■", end="")
                    else:
                        print("□", end="")
            print()



k = Knight('e', 5, Color.BLACK)

print(k.can_move('h', 4))

k.move_to('g', 4)

k.draw_board()
