def my_logger(func):
    def logger(*args, **kwargs):
        print(f"Хм вы что то сделали с числами{args}, {kwargs}")
        result = func(*args, **kwargs)
        print(f"и получили это {result}")
        return result
    return logger

@my_logger
def add(a, b):
    return a + b
@my_logger
def sub(a, b):
    return a - b

add(2, 4)
add(20, 4)
sub(40,20)