import json, sys
import socket
from tkinter import Spinbox
from urllib import request

from PyQt6.QtCore import QThread, pyqtSignal, Qt
from PyQt6.QtWidgets import QMainWindow, QWidget, QVBoxLayout, QLabel, QTableWidget, QTableWidgetItem, QApplication, \
    QLineEdit, QPushButton, QSpinBox, QAbstractItemView


class SocketWorker(QThread):
    signal = pyqtSignal(dict)
    def __init__(self, sock, request):
        super().__init__()
        self.sock = sock
        self.request = request

    def run(self):
        self.sock.sendall(json.dumps(self.request).encode())
        response = json.loads(self.sock.recv(1024).decode())
        self.signal.emit(response)

class Window(QMainWindow):
    def __init__(self):
        super().__init__()
        # INIT UI
        self.setFixedSize(550, 600)
        self.setWindowTitle("Профессии")

        self.centralwidget = QWidget()
        self.setCentralWidget(self.centralwidget)
        self.vbox = QVBoxLayout()
        self.centralwidget.setLayout(self.vbox)
        self.header_table = QLabel("Список")
        self.header_table.setStyleSheet("font-size: 20px")
        self.table = QTableWidget(0, 5)
        self.table.verticalHeader().setVisible(False)
        self.table.currentCellChanged.connect(lambda: self.spin_delete.setValue(self.table.currentRow()))
        self.table.setHorizontalHeaderLabels(["id", "Название", "Зарплата","Бонус","Средняя плата"])
        self.table.setShowGrid(True)

        self.header_put = QLabel("Добавление")
        self.header_put.setStyleSheet("font-size: 20px")
        self.label_name = QLabel("Название")
        self.line_name = QLineEdit()
        self.line_name.textChanged.connect(lambda: self.button_put.setEnabled(len(self.line_name.text()) > 0))
        self.label_salary = QLabel("Зарплата")
        self.spin_salary = QSpinBox(value=50)
        self.label_bonus = QLabel("Бонус")
        self.spin_bonus = QSpinBox(value=50)
        self.label_sp_salary = QLabel("Средняя плата")
        self.spin_sr_salary = QSpinBox(value=50)
        self.button_put = QPushButton("Добавить")
        self.button_put.clicked.connect(self.put)
        self.button_put.setEnabled(False)

        self.header_delete = QLabel("Удаление")
        self.header_delete.setStyleSheet("font-size: 20px")
        self.label_delete = QLabel("Удалить строку под ID")
        self.spin_delete = QSpinBox()
        self.button_delete = QPushButton("Удалить")
        self.button_delete.clicked.connect(self.delete)

        self.vbox.addWidget(self.header_table)
        self.vbox.addWidget(self.table)
        self.vbox.addWidget(self.header_put)
        self.vbox.addWidget(self.label_name)
        self.vbox.addWidget(self.line_name)
        self.vbox.addWidget(self.label_salary)
        self.vbox.addWidget(self.spin_salary)
        self.vbox.addWidget(self.label_bonus)
        self.vbox.addWidget(self.spin_bonus)
        self.vbox.addWidget(self.label_sp_salary)
        self.vbox.addWidget(self.spin_sr_salary)
        self.vbox.addWidget(self.button_put)
        self.vbox.addWidget(self.header_delete)
        self.vbox.addWidget(self.label_delete)
        self.vbox.addWidget(self.spin_delete)
        self.vbox.addWidget(self.button_delete)

        # CONNECT TO SERVER
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.sock.connect(('127.0.0.1', 8081))
        self.get()

    def get(self):
        self.send_response({"command":"GET"})

    def put(self):
        self.send_response(
            {
                "command":"PUT",
                "payload": {
                    "name": self.line_name.text(),
                    "salary": self.spin_salary.value(),
                    "bonus": self.spin_bonus.value(),
                    "sr_salary": self.spin_sr_salary.value(),
                }
            })
        self.line_name.setText("")
        self.spin_salary.setValue(50)
        self.spin_bonus.setValue(50)
        self.spin_sr_salary.setValue(50)

    def delete(self):
        self.send_response({"command":"DELETE", "id":self.spin_delete.value()})

    def send_response(self, request):
        self.worker = SocketWorker(self.sock, request)
        self.worker.signal.connect(self.server_handler)
        self.worker.start()

    def server_handler(self, response):
        if "data" in response:
            data = response["data"]

            self.table.setRowCount(len(data))
            for i, row in enumerate(data):
                self.table.setItem(i, 0, QTableWidgetItem(str(row["id"])))
                self.table.setItem(i, 1, QTableWidgetItem(str(row["name"])))
                self.table.setItem(i, 2, QTableWidgetItem(str(row["salary"])))
                self.table.setItem(i, 3, QTableWidgetItem(str(row["bonus"])))
                self.table.setItem(i, 4, QTableWidgetItem(str(row["sr_salary"])))

            self.spin_delete.setMaximum(len(data) - 1)
        elif "status" in response:
            self.get()

    def closeEvent(self, event):
        self.sock.close()

app = QApplication(sys.argv)
window = Window()
window.show()
app.exit(app.exec())