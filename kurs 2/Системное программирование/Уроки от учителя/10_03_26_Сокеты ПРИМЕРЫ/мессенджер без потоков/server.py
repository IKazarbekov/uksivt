import socket

HOST = '127.0.0.1'
PORT = 65438
"0 - 2023 порты заверзерированы системой и + ещё другие"

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.bind((HOST, PORT))
    s.listen()
    conn, addr = s.accept()
    with conn:
        print(f"Connected from{addr}")
        while True:
            data = conn.recv(1024)
            print(f"{data.decode("utf-8")!r}")
            if not data:
                break
            conn.sendall(input().encode("utf-8"))
            print(f"{data.decode("utf-8)}")}")