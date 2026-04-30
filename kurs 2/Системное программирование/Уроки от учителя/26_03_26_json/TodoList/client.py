from socket import socket, AF_INET, SOCK_STREAM
from config import *
import json

sock = socket(AF_INET, SOCK_STREAM)
sock.connect((HOST, PORT))
print("Подключение завершено")

def send_and_get_answer(command: str, arg = None):
    dictionary = {'command' : command}
    if arg is not None:
        dictionary.setdefault('arg', arg)
    json_string = json.dumps(dictionary)
    sock.sendall(json_string.encode(CODE))
    json_result = sock.recv(1024).decode(CODE)
    result = json.loads(json_result)
    return result

while True:
    command = input("Введите команду: ")
    match command:
        case 'list':
            answer_list = send_and_get_answer(command = 'list')
            list_tasks = answer_list['list']
            for id in range(len(list_tasks)):
                print(f"ID:{id}, task: {list_tasks[id]}")
        case 'add':
            text = input("Текст задачи:")
            answer = send_and_get_answer(command = 'add', arg=text)
            print(answer)
        case 'get':
            id = int(input("ID:"))
            answer = send_and_get_answer(command = 'get', arg=id)
            print(answer)
        case 'delete':
            id = int(input("ID:"))
            answer = send_and_get_answer(command='delete', arg=id)
            print(answer)

sock.close()