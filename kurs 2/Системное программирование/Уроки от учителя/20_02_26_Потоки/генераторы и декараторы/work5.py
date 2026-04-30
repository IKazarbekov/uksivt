import time


def my_logger(func):
    def logger(*args, **kwargs):
        print(f"Хм вы что то сделали с числами{args}, {kwargs}")
        result = func(*args, **kwargs)
        print(f"и получили это {result}")
        return result
    return logger

def my_timer(func):
    def timer(*args, **kwargs):
        s1 = time.time()
        result = func(*args, **kwargs)
        s2 = time.time()
        print(f"Функция выполнялась {s2 - s1} секунд")
        return result
    return timer

@my_logger
@my_timer
def printHEllos():
    for i in range(10):
        print("Hello world!")

printHEllos()