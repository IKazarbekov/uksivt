import socket
import threading
from threading import Thread

HOST = 'localhost'
PORT = 7780
"0 - 2023 порты заверзерированы системой и + ещё другие"

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.bind((HOST, PORT))
    s.listen()
    conn, addr = s.accept()

    def listen():
        while True:
            data = conn.recv(1024)
            print(f"{data.decode("utf-8")!r}")
            if not data:
                break

    with conn:
        print(f"Connected from{addr}")

        threading.Thread(target=listen).start()

        while True:
            conn.sendall(input().encode("utf-8"))
