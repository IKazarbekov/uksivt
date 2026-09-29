import json
import socket
from urllib import request

from PyQt6.QtCore import QThread, pyqtSignal
from PyQt6.QtGui import QCloseEvent
from PyQt6.QtWidgets import QMainWindow, QWidget, QVBoxLayout, QLabel, QTableWidget, QApplication, QTableWidgetItem, \
    QSpinBox, QLineEdit, QPushButton, QStackedWidget, QFormLayout
import sys

class SocketWorker(QThread):
    signal = pyqtSignal(dict)
    def __init__(self, sock, request: dict):
        QThread.__init__(self)
        self.sock = sock
        self.request = request

    def run(self):
        self.sock.sendall(json.dumps(self.request).encode("utf-8"))
        response = json.loads(self.sock.recv(1024).decode("utf-8"))
        self.signal.emit(response)

class Window(QMainWindow):
    def __init__(self):

        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.sock.connect(("127.0.0.1", 8080))

        super().__init__()
        self.setWindowTitle("Автомобили")
        self.setFixedSize(500, 200)

        self.stacked = QStackedWidget()
        self.widget = QWidget()
        self.setCentralWidget(self.stacked)
        self.stacked.addWidget(self.widget)
        self.vbox = QVBoxLayout()
        self.widget.setLayout(self.vbox)
        self.line_search = QLineEdit()
        self.line_search.textChanged.connect(self.find)
        self.table = QTableWidget(0, 4)
        self.table.setHorizontalHeaderLabels(["Марка", "Марка", "Стоимость", "Средняя ст-ть"])
        self.button_to_put = QPushButton("Добавить автомобиль")
        self.button_to_put.clicked.connect(lambda: self.stacked.setCurrentIndex(1))

        self.widget2 = QWidget()
        self.stacked.addWidget(self.widget2)
        self.form = QFormLayout()
        def enable_button():
            is_currect = False
            try:
                map(int, self.line_mark.split(","))
                is_currect = True
            except:
                is_currect = False
            self.button_put.setEnabled(
                is_currect and
                len(self.line_mark.text().strip()) > 0 and
                len(self.line_model.text().strip()) > 0
            )
        self.line_model = QLineEdit()
        self.line_mark = QLineEdit()
        self.line_price = QLineEdit()
        self.line_model.textChanged.connect(enable_button)
        self.line_mark.textChanged.connect(enable_button)
        self.line_price.textChanged.connect(enable_button)
        self.form.addRow("Модель: ", self.line_model)
        self.form.addRow("Марка: ", self.line_mark)
        self.form.addRow("Цены: ", self.line_price)
        self.button_put = QPushButton("Добавить")
        self.button_put.clicked.connect(self.put)
        self.button_put.setEnabled(False)
        self.button_back = QPushButton("Назад")
        self.button_back.clicked.connect(lambda: self.stacked.setCurrentIndex(0))
        self.vbox2 = QVBoxLayout()
        self.widget2.setLayout(self.vbox2)

        self.vbox.addWidget(self.line_search)
        self.vbox.addWidget(self.table)
        self.vbox.addWidget(self.button_to_put)
        self.vbox2.addLayout(self.form)
        self.vbox2.addWidget(self.button_put)
        self.vbox2.addWidget(self.button_back)

        self.get()

    def put(self):
        car = {"model": self.line_model.text(),
               "mark": self.line_mark.text(),
               "price": self.line_price.text().split(",")}
        self.send_request({"command": "PUT", "payload": car})

    def get(self):
        self.send_request({"command": "GET"})

    def find(self):
        if self.line_search.text() == "":
            self.get()
            return
        self.send_request({"command": "FIND",
                           "pattern": self.line_search.text()})

    def send_request(self, request: dict):
        self.worker = SocketWorker(self.sock, request)
        self.worker.signal.connect(self.server_handler)
        self.worker.start()

    def server_handler(self, response: dict):
        if "data" in response:
            data = response["data"]

            self.table.setRowCount(len(data))
            for i, row in enumerate(data):
                self.table.setItem(i, 0, QTableWidgetItem(row["model"]))
                self.table.setItem(i, 1, QTableWidgetItem(row["mark"]))
                self.table.setItem(i, 2, QTableWidgetItem(str(row["price"])))
                self.table.setItem(i, 3, QTableWidgetItem(str(row["sr_price"])))

    def closeEvent(self, event: QCloseEvent):
        self.sock.close()

app = QApplication(sys.argv)
window = Window()
window.show()
app.exit(app.exec())