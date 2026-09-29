import json
import socket
from threading import Thread
from data_processor import process_data

def client_handler(conn, addr):
    with conn:
        while True:
            data = conn.recv(1024)
            if not data: break
            request = json.loads(data.decode("utf-8"))
            response = process_data(request)
            conn.sendall(json.dumps(response, ensure_ascii=False).encode("utf-8"))

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.bind(("127.0.0.1", 8080))
    s.listen()
    while True:
        conn, addr = s.accept()
        thread = Thread(target=client_handler, args=(conn, addr))
        thread.start()