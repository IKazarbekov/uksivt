import time
from socket import socket, AF_INET, SOCK_STREAM
from config import PORT, CODE
HOST = "0.0.0.0"
DIR = "server_directory"

with socket(AF_INET, SOCK_STREAM) as sock:
    sock.bind((HOST, PORT))
    sock.listen()
    print("Сервер запущен. Жду подключений...")
    conn, address = sock.accept()

    name_file = conn.recv(1024).decode(CODE)
    print(f"Пользователь запросил файл {name_file}.")
    try:
        with open(f'{DIR}/{name_file}', 'r') as file:
            data = file.read()
            count = len(data.encode())
            print(f"Файл найден. Размер {count}.")

            # отправка статуса и памяти файла
            conn.sendall('OK'.encode(CODE))
            time.sleep(1)
            conn.sendall(f'{count}'.encode(CODE))
            time.sleep(1)

            # отправка данных файла
            conn.sendall(data.encode(CODE))
            print("Отправил данные файла")

    except FileNotFoundError as e:
        print("Файл не был найден")
        conn.send("NOTFOUND".encode(CODE))