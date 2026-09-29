from socket import socket, AF_INET, SOCK_STREAM
HOST, PORT, ENCODING = '127.0.0.1', 8080, "UTF-8"
from data_processor import command_processing
from threading import Thread
import json

def client_handler(conn, addr):
    with conn:
        buffer = b''
        while True:
            data = conn.recv(4096)
            if not data:
                print("Подключение завершенно")
                break
            buffer += data
            dict_data = json.loads(buffer.decode(ENCODING))
            buffer = b''
            response = command_processing(dict_data.get("command"), dict_data.get("payload"))
            json_response = json.dumps(response)
            conn.sendall(json_response.encode(ENCODING))

if __name__ == '__main__':
    with socket(AF_INET, SOCK_STREAM) as sock:
        sock.bind((HOST, PORT))
        sock.listen()
        print("Сервер запущен")
        while True:
            conn, addr = sock.accept()
            print("Принял подключение")
            thread = Thread(target=client_handler, args=(conn, addr))
            thread.start()
