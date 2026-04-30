import re
import socket
import threading
import time
from config import PORT
from config import CODE

HOST = '0.0.0.0'
slow_mode_second = 5
limit_connect = 5
malicious_payloads = [
    "\x1b[2J",  # Clear screen
    "\x1b[3J",  # Clear scrollback
    "\x1b[?25l",  # Hide cursor
    "\x1b[0m\x1b[41m\x1b[37m",  # Red background, white text
    "\a\a\a",  # Bell sound (beep) x3
    "\x1b[10;10H",  # Move cursor to 10,10
]

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.bind((HOST, PORT))
    s.listen()

    clients = dict()

    def remove_client(name: str):
        print(f"Сервер: Отключение клиента: {name}")
        send_all_message(f"{name} вышел".encode(CODE))
        conn_addr = clients[name]
        conn = conn_addr[0]
        addr = conn_addr[1]
        del clients[name]
        conn.close()

    def send_all_message(text, skip_conn = None, name_of_sender = None):
        for name_data, conn_addr in clients.items():
            name = name_data.encode(CODE)
            conn = conn_addr[0]
            addr = conn_addr[1]
            if conn == skip_conn:
                continue
            result = str()
            if name_of_sender is not None:
                result += f"{name}: "
            result += text.decode(CODE)
            conn.sendall(result.encode(CODE))

    def listen(conn, name):
        while True:
            try:
                time.sleep(slow_mode_second)
                data = conn.recv(1024)
                data_str = data.decode(CODE)
                # проверка на escape последовательности
                is_escape_sequence = False
                for str_esc_seq in malicious_payloads:
                    if re.search(data_str, str_esc_seq):
                        is_escape_sequence = True
                        break
                if "x1b" in data_str:
                    is_escape_sequence = True
                if data_str == "exit" or not data or len(data) > 200 or is_escape_sequence:
                    remove_client(name)
                    break
                print(f"{name} printed: <{data.decode(CODE)!r}>")
                send_all_message(f"{name}: {data.decode(CODE)}".encode(CODE), conn)
            except Exception as e:
                print(f"Error {e} of client {name}")
                remove_client(name)
                break

    def accept_client():
        while True:
            conn, addr = s.accept()
            name_data = conn.recv(1024)
            name = name_data.decode(CODE)
            if len(clients) > limit_connect:
                conn.sendall("max_limit".encode(CODE))
                conn.close()
                continue
            if name in clients:
                conn.sendall("name_contains".encode(CODE))
                conn.close()
                continue
            conn.sendall("okay".encode(CODE))
            send_all_message(f"Вошёл новый пользователь: {name}".encode(CODE))
            print(f"Принял пользователя {name}")
            thread = threading.Thread(target=listen, args=[conn,name])
            thread.start()
            clients.setdefault(name, (conn, addr))

    print("Сервер включён.")
    # try:
    accept_client()
    # except:
    #     list_names = list(clients.keys())
    #     for name in list_names:
    #         remove_client(name)