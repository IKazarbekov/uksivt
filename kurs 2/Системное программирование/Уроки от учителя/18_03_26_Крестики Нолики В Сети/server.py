import time
from itertools import count
from time import sleep
from socket import socket, AF_INET, SOCK_STREAM
from config import *
HOST = '0.0.0.0'

with socket(AF_INET, SOCK_STREAM) as sock:
    sock.bind((HOST, PORT))
    sock.listen()
    print("Server started. Waiting connection.")

    # connected and return connection of client
    def accept_client():
        conn, addr = sock.accept()
        return conn

    conn1 = accept_client()
    print("1 client connected.")
    conn2 = accept_client()
    print("2 client connected.")

    def send_all_client(text):
        conn1.sendall(text.encode(CODE))
        conn2.sendall(text.encode(CODE))
    def print_client1(text):
        conn1.sendall(text.encode(CODE))
    def print_client2(text):
        conn2.sendall(text.encode(CODE))
    def input_client1(text: str = ''):
        print_client1(text)
        return conn1.recv(1024)
    def input_client2(text: str = ''):
        print_client2(text)
        return conn2.recv(1024)
    def close_server():
        time.sleep(1)
        send_all_client("Q")
        sock.close()

    # player 1 - X
    # player 2 - O
    map = [['1','2','3'],
           ['4','5','6'],
           ['7','8','9']]
    def set_step_player1():
        pass
    def map_to_str():
        result = ""
        for string in map:
            for e in string:
                result += e + " "
            result += "\n"
        return result
    def is_win():
        # Проверка строк
        for row in range(3):
            if map[row][0] == map[row][1] == map[row][2]:
                return map[row][0]

        # Проверка колонок (ИСПРАВЛЕНО)
        for col in range(3):
            if map[0][col] == map[1][col] == map[2][col]:
                return map[0][col]

        # Проверка диагоналей
        if map[0][0] == map[1][1] == map[2][2]:
            return map[0][0]
        if map[0][2] == map[1][1] == map[2][0]:
            return map[0][2]
        return None
    def input_step_player1():
        try:
            data = int(input_client1("Ваш ход : ")) - 1
        except:
            data = 0
        while not 0 <= data < 9 or map[data // 3][data % 3] == 'X' or map[data // 3][data % 3] == 'Y':
            try:
                data = int(input_client1("Неверный ход : ")) - 1
            except:
                continue
        return data
    def input_step_player2():
        try:
            data = int(input_client2("Ваш ход : ")) - 1
        except:
            data = 0
        while not 0 <= data < 9 or map[data // 3][data % 3] == 'X' or map[data // 3][data % 3] == 'Y':
            try:
                data = int(input_client2("Неверный ход : ")) - 1
            except:
                continue
        return data
    count = 0
    def is_draw():
        global count
        count+= 1
        if count > 8:
            return True
        else:
            return False

    send_all_client("НАЧАЛО ИГРЫ\n" + map_to_str())
    sleep(1)
    isFirstPlayerStep = True
    try:
        while True:
            if isFirstPlayerStep:
                print_client2("Ждём ход соперника.\n")
                data = input_step_player1()
                map[data // 3][data % 3] = 'X'
            else:
                print_client1("Ждём ход соперника.\n")
                data = input_step_player2()
                map[data // 3][data % 3] = '0'
            send_all_client("Ход был сделан.\n" + map_to_str())
            isFirstPlayerStep = not isFirstPlayerStep
            if is_win() is not None:
                send_all_client("ПОБЕДИЛ ИГРОК " + is_win())
                close_server()
                break
            if is_draw():
                send_all_client("НИЧЬЯ")
                close_server()
                break
    except KeyboardInterrupt:
        send_all_client('Q')

