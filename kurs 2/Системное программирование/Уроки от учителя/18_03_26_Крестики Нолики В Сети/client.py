import threading
from socket import socket, AF_INET, SOCK_STREAM
from config import *

with socket(AF_INET, SOCK_STREAM) as sock:
    sock.connect((HOST, PORT))
    print("Connected in server")
    quite = False

    def listen():
        global quite
        while True:
            try:
                data = sock.recv(1024).decode(CODE)
            except:
                break
            if data == 'Q':
                quite = True
                break
            print(data, end='')

    threading.Thread(target=listen).start()
    while not quite:
        try:
            sock.sendall(input().encode(CODE))
        except:
            break
