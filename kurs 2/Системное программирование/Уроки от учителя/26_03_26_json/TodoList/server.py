from socket import socket, AF_INET, SOCK_STREAM
from config import *
import json

tasks = ['Помыть посуду','Звонить друзьям','Прогулка','Сделать уроки']

sock = socket(AF_INET, SOCK_STREAM)
sock.bind((HOST, PORT))
sock.listen()
conn, addr = sock.accept()
print("Клиент подключился")

def send_answer(status: str, **dictionary):
    dictionary.setdefault('status', status)
    json_string = json.dumps(dictionary, ensure_ascii=False)
    conn.sendall(json_string.encode(CODE))

while True:
    command_client = conn.recv(1024).decode(CODE)
    dict_command = json.loads(command_client)
    match dict_command['command']:
        case 'list':
            send_answer('ok', list=tasks)
        case 'add':
            text = dict_command['arg']
            tasks.append(text)
            send_answer('ok', id=len(tasks) - 1)
        case 'get':
            id = dict_command['arg']
            try:
                task_text = tasks[id]
                send_answer('ok', text=task_text)
            except IndexError:
                send_answer('error')
        case 'delete':
            id = dict_command['arg']
            try:
                tasks.pop(id)
                send_answer('ok')
            except IndexError:
                send_answer('error')

sock.close()