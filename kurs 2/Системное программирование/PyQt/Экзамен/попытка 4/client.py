import sys

from PyQt6.QtCore import QThread, pyqtSignal
from PyQt6.QtWidgets import QApplication, QMainWindow, QMessageBox, QWidget, QStackedWidget, QLabel, QPushButton, \
    QVBoxLayout, QTableWidget, QComboBox, QLineEdit, QTableWidgetItem, QSpinBox
import json
import socket
HOST_PORT = ("127.0.0.1", 8080)

class SocketWorker(QThread):
    received = pyqtSignal(dict)

    def __init__(self, sock, request):
        super().__init__()
        self.sock = sock
        self.request = request

    def run(self):
        try:
            print("run worker socket")
            self.sock.sendall(json.dumps(self.request).encode())
            response = self.sock.recv(1024).decode()
            if not response:
                return
            answer = json.loads(response)
            self.received.emit(answer)
        except Exception as e:
            self.received.emit({"error":str(e)})

class MainWindow(QMainWindow):
    def __init__(self):
        super().__init__()

        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.sock.connect(HOST_PORT)


        self._initUI()

    def closeEvent(self, event):
        self.sock.close()

    def _initUI(self):
        self.screen1 = QWidget()
        self.vbox_list = QVBoxLayout()
        self.label1 = QLabel("Список студентов")
        self.table = QTableWidget(0, 3)
        self.table.setHorizontalHeaderLabels(["Имя","Оценки","Средний балл"])
        self.comboBox = QComboBox()
        self.comboBox.addItems(["Все студенты","Средний балл > 4>","Есть хотя бы одна 5",])
        self.comboBox.addItems(["Сотированный порядок","Обратный порядок","Смешанный порядок",])
        self.button_update = QPushButton("Обновить данные")
        self.button_update.clicked.connect(self._get_sort)
        self.button_to_put = QPushButton("Добавить студента")
        self.button_to_put.clicked.connect(lambda: self.stackedWidget.setCurrentIndex(1))
        self.vbox_list.addWidget(self.label1)
        self.vbox_list.addWidget(self.table)
        self.vbox_list.addWidget(self.comboBox)
        self.vbox_list.addWidget(self.button_update)
        self.vbox_list.addWidget(self.button_to_put)
        self.screen1.setLayout(self.vbox_list)

        self.screen2 = QWidget()
        self.vbox_put = QVBoxLayout()
        self.label2 = QLabel("Добавление студента")
        self.label_name = QLabel("Имя студента")
        self.line_name = QLineEdit()
        self.label_grades = QLabel("Оценки студента")
        self.line_grades = QLineEdit()
        self.button_put = QPushButton("Добавить студента")
        self.button_back = QPushButton("Назад к списку")
        self.button_back.clicked.connect(lambda: self.stackedWidget.setCurrentIndex(0))
        self.vbox_put.addWidget(self.label_name)
        self.vbox_put.addWidget(self.line_name)
        self.vbox_put.addWidget(self.label_grades)
        self.vbox_put.addWidget(self.line_grades)
        self.vbox_put.addWidget(self.button_put)
        self.vbox_put.addWidget(self.button_back)
        self.screen2.setLayout(self.vbox_put)

        self.stackedWidget = QStackedWidget()
        self.stackedWidget.addWidget(self.screen1)
        self.stackedWidget.addWidget(self.screen2)
        self.setCentralWidget(self.stackedWidget)

    def _send_request(self, request):
        print("_send_request")
        self.worker = SocketWorker(self.sock, request)
        self.worker.received.connect(self._handler)
        self.worker.start()

    def _handler(self, response):
        print("_handler")
        if "students" in response:
            students = response["students"]
            print(students)
            if self.comboBox.currentIndex() == 1:
                students = list(filter(lambda st: st["avg"] >= 4, students))
            elif self.comboBox.currentIndex() == 2:
                students = list(filter(lambda st: 5 in st["grades"], students))
            print("    sorted")

            self.table.setRowCount(len(students))
            for i, student in enumerate(students):
                self.table.setItem(i, 0, QTableWidgetItem(student["name"]))
                self.table.setItem(i, 1, QTableWidgetItem(student["grades"]))
                self.table.setCellWidget(i, 2, QSpinBox(value = student["avg"]))

            print("    table if fill")

    def _get_sort(self):
        self._send_request({"command":"GET-SORT"})

app = QApplication(sys.argv)
window = MainWindow()
window.show()
app.exit(app.exec())