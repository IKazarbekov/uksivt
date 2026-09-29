from json import JSONDecodeError
from socket import socket, AF_INET, SOCK_STREAM
from threading import Thread
import json
from car import Sharing_car

# data
cars = [
    Sharing_car("HEDA", "Efa", 34, 32),
    Sharing_car("Bloda", "Poe4", 54, 40),
    Sharing_car("GOder", "F3", 23, 22),
]

with socket(AF_INET, SOCK_STREAM) as sosk:
    sosk.bind(('127.0.0.1', 8080))
    sosk.listen()

    while True:
        sock, addr = sosk.accept()

        def listen():
            global cars
            while True:
                # get data
                try:
                    json_data = sock.recv(1024)
                    dict_data = json.loads(json_data)
                except JSONDecodeError as e:
                    break
                except Exception as e:
                    print(e)
                    break
                command: str = dict_data['command']

                match command:
                    case "GET":
                        json_answer = json.dumps(cars, default=lambda o: o.__dict__, indent=4)
                        sock.sendall(json_answer.encode())
                    case "PUT":
                        cars = dict_data['cars']
                        print("Сохранил новые данные")

        thread = Thread(target=listen)
        thread.start()