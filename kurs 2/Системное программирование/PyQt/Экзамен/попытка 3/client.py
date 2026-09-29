from socket import socket, AF_INET, SOCK_STREAM
import json
import sys

from PyQt6.QtWidgets import QMainWindow, QWidget, QVBoxLayout, QStackedWidget, QLabel, QTableWidget, QPushButton, \
    QApplication, QMessageBox, QTableWidgetItem, QSpinBox, QLineEdit, QComboBox
from PyQt6.QtCore import pyqtSignal, QThread

HOST, PORT, ENCODING = '127.0.0.1', 8080, "UTF-8"
s = socket(AF_INET, SOCK_STREAM)

class ClientWorker(QThread):
    response_received = pyqtSignal(dict)

    def __init__(self, sock, request):
        super().__init__()
        self.sock = sock
        self.request = request

    def run(self):
        try:
            self.sock.sendall(json.dumps(self.request).encode(ENCODING))
            answer = json.loads(self.sock.recv(1024).decode(ENCODING))
            self.response_received.emit(answer)
        except Exception as e:
            self.response_received.emit({"error": str(e)})

class MainWindow(QMainWindow):
    def __init__(self):
        super().__init__()

        self._students = []

        self._sock = socket(AF_INET, SOCK_STREAM)
        self._connect()
        self._initUI()
        self._get_all_data()
        self.show()

    def closeEvent(self, event):
        self._sock.close()

    def _connect(self):
        try:
            self._sock.connect((HOST, PORT))
        except ConnectionRefusedError:
            QMessageBox.information(self, "Ошибка подключения", "Не удаётся подключится к серверу, убедитель что сервер запущен и запуститте прилоежние еще раз")
            exit()

    def _send_request(self, command: str, payload = None):
        self.worker = ClientWorker(self._sock, {"command": command, "payload": payload})
        self.worker.response_received.connect(self._handle_response)
        self.worker.start()

    def _handle_response(self, response):
        if "students" in response:
            students = response["students"]
            if self.combo.currentIndex() == 1:
                students = list(filter(lambda st: st["avg"] >= 4, students))
            elif self.combo.currentIndex() == 2:
                students = list(filter(lambda st: 5 in st["grades"], students))
            self.table.setRowCount(len(students))
            for i, student in enumerate(students):
                self.table.setItem(i, 0, QTableWidgetItem(student["name"]))
                self.table.setItem(i, 1, QTableWidgetItem(str(student["grades"])))
                self.table.setCellWidget(i, 2, QSpinBox(value = student["avg"]))
            max_avg = max(students, key=lambda x: x["avg"])["avg"]
            min_avg = min(students, key=lambda x: x["avg"])["avg"]
            sr_avg = sum([s["avg"] for s in students]) / len(students)
            status = f"Кол-во: {len(students)}; Максимальный балл: {max_avg}; Минимальный балл: {min_avg}; Средний балл: {sr_avg}"
            self.statusBar().showMessage(status)
        else:
            QMessageBox.information(self, "Ошибка получения данных", "Ошибка получения данных из сервера. Вероятно на сервере, что-то не так.")

    def _get_all_data(self):
        print("Отправил запрос GET")
        self._send_request("GET")

    def _put_student(self):
        if self.line_name.text() == "":
            QMessageBox.warning(self, "Ошибка заполнения", "Заполните имя ученика")
        print("Отправил запрос PUT")
        student = {"name":self.line_name.text(), "grades":list(map(int, self.line_grades.text().split(",")))}
        self._send_request("PUT", student)
        self.line_name.setText("")
        self.line_grades.setText("")
        self.stacked.setCurrentIndex(0)

    def _initUI(self):
        self.screen1 = QWidget()
        self.screen2 = QWidget()
        self.vbox = QVBoxLayout()

        self.label1 = QLabel("Ученики")
        self.combo = QComboBox()
        self.combo.addItem("Все студенты")
        self.combo.addItem("Средний балл > 4")
        self.combo.addItem("Имеет пятёрку")
        self.combo.currentIndexChanged.connect(self._get_all_data)
        self.table = QTableWidget(0, 3)
        self.table.setHorizontalHeaderLabels(["Имя","Оценки","Средний балл"])
        self.button_update = QPushButton("Обновить данные")
        self.button_update.clicked.connect(self._get_all_data)
        self.button_put = QPushButton("добавить ученика")
        self.button_put.clicked.connect(lambda:self.stacked.setCurrentIndex(1))

        self.label2 = QLabel("Добавление ученика", self)
        self.label_name = QLabel("Имя", self)
        self.line_name = QLineEdit(self)
        self.label_grades = QLabel("Оценки(через запятую)", self)
        self.line_grades = QLineEdit(self)
        self.button_put2 = QPushButton("Добавить", self)
        self.button_put2.clicked.connect(lambda:self._put_student())

        self.button_list = QPushButton("Список учеников")
        self.button_list.clicked.connect(lambda:self.stacked.setCurrentIndex(0))

        self.screen1.setLayout(self.vbox)
        self.vbox2 = QVBoxLayout()
        self.screen2.setLayout(self.vbox2)
        self.stacked = QStackedWidget()
        self.stacked.addWidget(self.screen1)
        self.stacked.addWidget(self.screen2)
        self.setCentralWidget(self.stacked)

        self.vbox.addWidget(self.label1)
        self.vbox.addWidget(self.combo)
        self.vbox.addWidget(self.table)
        self.vbox.addWidget(self.button_update)
        self.vbox.addWidget(self.button_put)

        self.vbox2.addWidget(self.label2)
        self.vbox2.addWidget(self.label_name)
        self.vbox2.addWidget(self.line_name)
        self.vbox2.addWidget(self.label_grades)
        self.vbox2.addWidget(self.line_grades)
        self.vbox2.addWidget(self.button_put2)
        self.vbox2.addWidget(self.button_list)

app = QApplication(sys.argv)
window = MainWindow()
app.exit(app.exec())
