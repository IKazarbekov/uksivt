import random
import socket
import threading
import time

from config import CODE
from config import PORT

def floor_server():
    sock =  socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.connect(('localhost',PORT))

    #message = "X" + str(random.randint(0,100000))
    message = "ДАНИЛА ТВОЕМУ СЕРВЕРУ КОНЕЦ!!"

    while True:
        sock.sendall(message.encode(CODE))

    time.sleep(3600)

for i in range(18_000):
    t = threading.Thread(target=floor_server)
    t.start()
    print("Создал соединение" + str(i))
