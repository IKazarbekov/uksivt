"""
МЕТОДЫ
    sodked()        - открыть соединение
    bind()          - связать
    listen()        - слушать
    accept()        - принять
    connect()       - подключится
    connect_ex()    -
    send()          - отправить
    recv()          -
    close()         - закрыть соединение

ПРОТОКОЛЫ СВЯЗИ
    TCP - проверяет был ои доставлен пакет
        - информация отравляется последовательно
    UDP - не проверяет, доставилась ли информация

socket                   server_messages socket
client                   bind
                         listen
connect ---------------> accept

send ------------------> recv

recv<------------------- send
                        
recv<-------------------close
close

"""