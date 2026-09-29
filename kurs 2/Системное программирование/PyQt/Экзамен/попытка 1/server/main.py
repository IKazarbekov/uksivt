from json import JSONDecodeError
from socket import socket, AF_INET, SOCK_STREAM
from threading import Thread

from sharing_car import Sharing_car
import json
CODING = 'utf-8'

cars = [
    Sharing_car("Bles", "34", 30, 40),
    Sharing_car("Lores", "53", 55, 53),
    Sharing_car("Glober", "35", 40, 41),
]

def get_json(obj):
    return json.dumps(obj, default=lambda o: o.__dict__, sort_keys=True, indent=4)

def log(msg, tab = 0):
    if True:
        print( "\t" * tab + msg)

with socket(AF_INET, SOCK_STREAM) as s:
    s.bind(('127.0.0.1', 8080))
    s.listen()
    while True:
        conn, addr = s.accept()
        log("Получил соединение")

        def connection():
            global cars
            while True:
                try:
                    json_msg = conn.recv(1024).decode(CODING)
                    dict_msg = json.loads(json_msg)
                    cmd = dict_msg["command"]
                    log("Мне отправили команду: " + cmd, 1)
                    match cmd:
                        case "GET":
                            conn.sendall(get_json(cars).encode(CODING))
                            log("Отправил клиенту все данные", 1)
                            break
                        case "PUT":
                            log("Мне отправили данные", 1)
                            cars = dict_msg["data"]
                            log("Я их сохранил, вот" + str(cars), 1)
                        case _:
                            log("Не знаю эту команду", 1)
                except JSONDecodeError:
                    log("Соединение с клиентом закрыто", 1)
                    break

        thread = Thread(target=connection)
        thread.start()
