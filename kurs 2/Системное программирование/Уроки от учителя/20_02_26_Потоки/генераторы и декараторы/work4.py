import functools


def work( iter ):
    def worker(func):
        @functools.wraps(func)
        def runner(*args, **kwargs):
            for i in range(iter):
                result = func(*args, **kwargs)
            return result
        return runner
    return worker

@work(iter=2)
def printHello():
    print("Hello")

printHello()
printHello()
