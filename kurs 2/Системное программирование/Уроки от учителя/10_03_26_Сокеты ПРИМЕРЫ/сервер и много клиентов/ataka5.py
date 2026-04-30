import socket

from config import CODE, PORT, HOST

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as sock:
    sock.bind(('0.0.0.0', 8085))
    while True:
        packet = sock.recvfrom(0)
        print(f"Captured {packet}")