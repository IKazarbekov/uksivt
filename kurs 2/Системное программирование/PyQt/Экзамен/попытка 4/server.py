import socket
from threading import Thread
import json
from data_processor import processing

def handler_client(conn, addr):
    with conn:
        print("Принял подключение")
        buffer = b''
        while True:
            try:
                data = conn.recv(1024)
            except Exception as e:
                print("Завершил принудительно;", str(e))
                break
            if not data:
                print("Завершил подключение")
                break
            buffer += data
            request = json.loads(buffer.decode('utf-8'))
            buffer = b''
            command = request["command"]
            payload = request.get("payload")
            result = processing(command, payload)
            print("    Я отправил ему", result)
            conn.sendall(json.dumps({"students":result}).encode())

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    print("Сервер запущен")
    s.bind(('127.0.0.1', 8080))
    s.listen()
    while True:
        conn, addr = s.accept()
        thread = Thread(target=handler_client, args=(conn, addr))
        thread.start()