import socket
import threading
from fcntl import lockf
from tkinter.font import names

HOST = 'localhost'
from config import PORT
from config import CODE

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.connect((HOST, PORT))
    print("Вы успешно вошли в чат !")

    def listen():
        while True:
            data = s.recv(1024)
            if not data:
                break
            print(f"{data.decode(CODE)!r}")

    thread = threading.Thread(target=listen)
    thread.start()

    while True:
        message = input()
        if message == 'выйти':
            s.sendall('exit'.encode(CODE))
            print("Вы вышли из чата.")
            s.close()
            break
        s.sendall(message.encode(CODE))

