import threading
import time

def worler():
    print("Hello")
    time.sleep(1)

thread = threading.Thread(target=worler)

thread.start()

thread.join()