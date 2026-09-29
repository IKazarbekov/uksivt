import json
import socket
import data_processor
from threading import Thread, Lock

lock = Lock()

def handler_server(conn, addr):
    with conn:
        while True:
            data = conn.recv(1024)
            if not data:
                break
            request = json.loads(data.decode())
            print(request)
            with lock:
                response = data_processor.process(request)
                conn.sendall(json.dumps(response).encode())

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.bind(('127.0.0.1', 8081))
    s.listen()
    while True:
        conn, addr = s.accept()
        print("connect")
        thread = Thread(target=handler_server, args=(conn, addr))
        thread.start()