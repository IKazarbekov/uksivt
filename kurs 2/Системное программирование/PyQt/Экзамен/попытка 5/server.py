import socket, data_processed, threading
HOST_PORT = ('127.0.0.1', 8080)
import json

def client_handler(conn, addr):
    with conn:
        print("Принял соединение в отдельный поток")
        buffer = b''
        while True:
            data = conn.recv(1024)
            if not data:
                print("Закрыли соединение успешно")
                break
            print("Получил какие-то данные", data)
            buffer += data
            request = json.loads(buffer.decode('utf-8'))
            print("Вот запрос:", request)
            buffer = b''
            response = data_processed.prosecced(request["command"], request.get("payload"))
            response = {"students": [s.to_dict() for s in response]}
            print("Получило ответ от модуля data_processed:", response)
            conn.sendall(json.dumps(response).encode('utf-8'))
            print("Отправил ответ клиенту")

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    print("Сервер запущен по ", HOST_PORT)
    print()
    s.bind(HOST_PORT)
    s.listen()
    while True:
        conn, addr = s.accept()
        print("Принимается соединение...")
        thread = threading.Thread(target=client_handler, args=(conn, addr))
        thread.start()
