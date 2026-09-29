import time
from itertools import count
from socket import socket, AF_INET, SOCK_STREAM
from config import PORT, HOST, CODE
DIR = "client_directory"

with socket(AF_INET, SOCK_STREAM) as sock:
    name_file = input("Введите название файла: ")
    sock.connect((HOST, PORT))

    print("Подключился к серверу.")
    sock.sendall(name_file.encode(CODE))
    status = sock.recv(1024).decode(CODE)

    # Проверка статусов
    if status == "NOTFOUND":
        print('Сервер не нашёл файл.')

    # Если файл на сервере найден
    elif status == "OK":
        print('Сервер нашёл файл, жду сообщение о кол-ве памяти файла...')
        count = int(sock.recv(1024))
        print(f'Кол-во памяти - {count}')

        # Чтение файла
        with open(f'{DIR}/{name_file}', 'w') as file:
            data = b''
            while len(data) < count:
                time.sleep(1)
                data += sock.recv(count - len(data))
            print("Принял данные файла.")
            file.write(data.decode(CODE))
            print("Файлы записаны")
    else:
        print("Не известный статус")
