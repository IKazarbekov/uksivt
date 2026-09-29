import json
import socket
from data_processor import process
from threading import Thread

def handler_client(conn, addr):
    with conn:
        buffer = b''
        while True:
            data = conn.recv(1024)
            if not data:
                break
            buffer += data
            request = json.loads(buffer.decode('utf-8'))
            buffer = b''
            print(request)
            response = process(request["command"], request.get("payload"))
            json_response = json.dumps(response).encode()
            conn.sendall(json_response)

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.bind(('127.0.0.1', 8080))
    s.listen()
    while True:
        conn, addr = s.accept()
        thread = Thread(target=handler_client, args=(conn, addr))
        thread.start()