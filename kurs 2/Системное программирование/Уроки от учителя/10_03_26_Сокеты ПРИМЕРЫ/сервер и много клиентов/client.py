import socket
import threading


HOST = 'localhost'
from config import PORT
from config import CODE

name = input("Введи ваше имя:")

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.connect((HOST, PORT))
    s.sendall(name.encode(CODE))
    message_status = s.recv(1024).decode(CODE)
    if message_status == "okay":
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

    else:
        print("Такое имя уже занято")