import socket
import threading

HOST = 'localhost'
PORT = 7780

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.connect((HOST, PORT))

    def listen():
        while True:
            data = s.recv(1024)
            print(f"{data.decode("utf-8")!r}")

    threading.Thread(target=listen).start()

    while True:
        s.sendall(input().encode("utf-8"))
