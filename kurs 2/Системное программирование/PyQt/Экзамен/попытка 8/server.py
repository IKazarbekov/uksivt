import json
import socket, data_processor
from threading import Thread, Lock

lock = Lock()

def client_handler(conn, addr):
    with conn:
        while True:
            data = conn.recv(1024)
            if not data:
                break
            request = json.loads(data.decode())
            with lock:
                response = data_processor.process_request(request)
            conn.sendall(json.dumps(response).encode())

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.bind(('127.0.0.1', 8081))
    s.listen()
    while True:
        conn, addr = s.accept()
        thread = Thread(target=client_handler, args=(conn, addr))
        thread.start()