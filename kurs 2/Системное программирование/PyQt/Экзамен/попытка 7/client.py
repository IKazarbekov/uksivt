import json
import socket, sys

from PyQt6.QtCore import QThread, pyqtSignal
from PyQt6.QtWidgets import QMainWindow, QVBoxLayout, QTableWidget, QApplication, QWidget, QTableWidgetItem, QLineEdit, \
    QLabel, QPushButton, QSpinBox


class SocketWorker(QThread):
    signal = pyqtSignal(dict)
    def __init__(self, sock, request):
        super().__init__()
        self.sock = sock
        self.request = request

    def run(self):
        self.sock.sendall(json.dumps(self.request).encode())
        mesg = self.sock.recv(1024).decode()
        response = json.loads(mesg)
        self.signal.emit(response)

class Window(QMainWindow):
    def __init__(self):
        super().__init__()

        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.sock.connect(('127.0.0.1', 8081))

        self.widget = QWidget()
        self.vbox = QVBoxLayout()
        self.setCentralWidget(self.widget)
        self.widget.setLayout(self.vbox)
        self.line_search = QLineEdit()
        self.line_search.textChanged.connect(lambda:(
          self.send_request({"command":"FIND","pattern":self.line_search.text()})
          if self.line_search.text() else self.send_request({"command":"GET"})
        ))
        self.table = QTableWidget(0, 4)
        self.table.setEnabled(False)
        self.table.setHorizontalHeaderLabels(["Название","Стоимость","Кол-во","Средняя цена"])
        self.vbox.addWidget(self.line_search)
        self.vbox.addWidget(self.table)
        self.label_name = QLabel("Название")
        self.line_name = QLineEdit()
        self.label_count = QLabel("Кол-во")
        self.spin_count = QSpinBox()
        self.button = QPushButton("Добавить")
        self.button.clicked.connect(lambda: (
            self.send_request(
            {"command":"PUT",
             "payload":
                 {
                     "name":self.line_name.text(),
                    "count":self.spin_count.value()
                }
             }
        )
        ))
        self.vbox.addWidget(self.label_name)
        self.vbox.addWidget(self.line_name)
        self.vbox.addWidget(self.label_count)
        self.vbox.addWidget(self.spin_count)
        self.vbox.addWidget(self.button)

        self.send_request({"command" : "GET"})

    def send_request(self, request):
        self.worker = SocketWorker(sock=self.sock, request=request)
        self.worker.signal.connect(self.handler_server)
        self.worker.start()

    def handler_server(self, responce):
        if "status" in responce:
            return
        if "data" in responce:
            data = responce["data"]
            self.table.setRowCount(len(data))
            for i, prod in enumerate(data):
                self.table.setItem(i, 0, QTableWidgetItem(prod["name"]))
                self.table.setItem(i, 1, QTableWidgetItem(str(prod["price"])))
                self.table.setItem(i, 2, QTableWidgetItem(str(prod["count"])))
                self.table.setItem(i, 3, QTableWidgetItem(str(prod["sr_price"])))

app = QApplication(sys.argv)
window = Window()
window.show()
app.exit(app.exec())