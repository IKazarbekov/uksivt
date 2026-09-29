from turtle import Turtle, Screen
import turtle
import threading

class Interface:
    _speed = 10
    function = None
    ms = 0
    turn = None
    enemies = list()

    # Функции для обработки нажатий
    @classmethod
    def move_forward(cls):
        cls.turt.forward(cls._speed)

    @classmethod
    def move_backward(cls):
        cls.turt.backward(cls._speed)

    @classmethod
    def turn_left(cls):
        cls.turt.left(15)

    @classmethod
    def turn_right(cls):
        cls.turt.right(15)

    @classmethod
    def clear_screen(cls):
        cls.turt.clear()

    @classmethod
    def pen_up(cls):
        cls.turt.penup()

    @classmethod
    def pen_down(cls):
        cls.turt.pendown()

    @property
    @classmethod
    def speed(cls):
        return cls._speed

    @speed.setter
    @classmethod
    def speed(cls, new_speed):
        cls._speed = new_speed * 10

    @classmethod
    def set_timer(cls, func, ms):
        cls.function = func
        cls.ms = ms

    @classmethod
    def set_text(cls, text):
        cls.turt.clear()
        cls.turt.write(text, align='left', font=('Arial', 30, 'bold'))



    @classmethod
    def create_enemy(cls):
        turt = Turtle()
        cls.enemies.append(turt)
        turt.shape('arrow')
        turt.color('red')

    @classmethod
    def start_window(cls):
        screen = turtle.Screen()
        cls.turt = Turtle()

        # base setting
        cls.turt.shape('turtle')
        cls.turt.shapesize(5)

        # listen key event
        screen.onkey(cls.move_forward, "w")
        screen.onkey(cls.move_backward, "s")
        screen.onkey(cls.turn_left, "a")
        screen.onkey(cls.turn_right, "d")
        screen.listen()

        # timer
        def function_and_restart_timer():
            cls.function()
            screen.ontimer(function_and_restart_timer, cls.ms)
        if cls.function is not None:
            screen.ontimer(function_and_restart_timer, cls.ms)


        screen.mainloop()

if __name__ == '__main__':
    pass