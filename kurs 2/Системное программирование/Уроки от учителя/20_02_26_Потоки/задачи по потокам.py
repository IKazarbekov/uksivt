import random
import time, threading
from sqlite3 import threadsafety
from threading import Thread

# work 1
"""
def worker():
    for i in range(5):
        print(i)
        time.sleep(1)
Thread(target=worker).start()
Thread(target=worker).start()
"""

# work 2
"""
count = 0
lock = threading.Lock()
def worker():
    global count
    for _ in range(1000):
        with lock:
            current = count
            current += 1
            count = current
th1 = Thread(target=worker)
th2 = Thread(target=worker)
th1.start()
th2.start()
th1.join()
th2.join()
print(count)
"""

#work 3
"""

items = []
conditions = threading.Condition()

def producer():
    for i in range(5):
        with conditions:
            items.append(f"Товар-{i}")
            print(f"Произвёл: Товар-{i}")
            conditions.notify()
        time.sleep(3)

def consumer():
    for i in range(5):
        with conditions:
            while not items:
                print("Потребитель ждёт...")
                conditions.wait()
            item = items.pop(0)
            print(f"Потребил: {i} товар")
        time.sleep(random.random())

th1 = threading.Thread(target=producer)
th2 = threading.Thread(target=consumer)

th1.start()
th2.start()

th1.join()
th2.join()

print('END')
"""

# work 4
semaphore = threading.Semaphore(5)

def worker(name):
    print(f"{name} хочет работать")

    with semaphore:
        print(f"{name} начал работать")
        time.sleep(random.random())
        print(f"{name} закончил работу")

Thread(target=worker, args=['Make']).start()
Thread(target=worker, args=['John']).start()
Thread(target=worker, args=['Bob']).start()
Thread(target=worker, args=['Tom']).start()
Thread(target=worker, args=['Spaik']).start()
Thread(target=worker, args=['Geri']).start()
Thread(target=worker, args=['Dog']).start()

# work 5
"""
from queue import Queue
q = Queue()
def getter(q: Queue):
    q.put("Hm")
    time.sleep(1)
    q.put("Hello")
    time.sleep(1)
    q.put("World")
    time.sleep(1)
    q.put("!")

def setter(q: Queue):
    while True:
        s = q.get()
        if s == '123':
            break
        print(s)
        q.task_done()

th1 = Thread(target=getter, args=(q,))
th2 = Thread(target=setter, args=(q,))
th1.start()
th2.start()
th1.join()
q.put('123')
th2.join()
"""
