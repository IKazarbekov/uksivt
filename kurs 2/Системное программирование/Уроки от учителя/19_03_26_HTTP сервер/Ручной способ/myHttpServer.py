from socket import socket, AF_INET, SOCK_STREAM
HOST, PORT, CODE, PAGE_CODE = 'localhost', 8080, 'ascii', 'utf-8'
import os

#start server
sock = socket(AF_INET, SOCK_STREAM)
sock.bind((HOST, PORT))
sock.listen()

# functions for work
def input_meta_data(conn):
    text = conn.recv(1024).decode(CODE)
    first_string = text.split('\n')[0]
    path_with_slash = text.split(' ')[1]
    path = path_with_slash[1:]
    return path
def file_exists(path: str):
    return os.path.exists(path)
def send_http_404(conn):
    conn.sendall("HTTP/1.1 404".encode(CODE))
    conn.close()
def read_file(path):
    with open(path, 'rb') as file:
        return file.read()
def send_html_200(conn, body):
    conn.sendall((f'''HTTP/1.1 200 OK
Date: Fri, 20 Mar 2026 10:00:00 GMT
Server: Apache
Content-Type: text/html; charset={PAGE_CODE.upper()}
Content-Length: {len(body)}#.encode(PAGE_CODE))
Connection: close
\r\n
''').encode(CODE))
    conn.sendall(body)#.encode(PAGE_CODE)).encode(PAGE_CODE))

while True:
    conn, addr = sock.accept()
    print("Ко мне подключился кто-то...")

    path = input_meta_data(conn)
    print("Получил данные! От меня требуют этот файл :", path)

    if file_exists(path):
        print("Ага нашёл файл >:)")
    else:
        print("Не могу найти файл :(")
        send_http_404(conn)
        continue

    page = read_file(path)
    print("Я взял этот файл, тяжёленький :O")

    send_html_200(conn, page)
    print("Я отправил ему файл :'")

    conn.close()




