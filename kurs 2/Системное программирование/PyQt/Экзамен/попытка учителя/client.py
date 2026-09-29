import sys
import socket
import json
from PyQt6.QtWidgets import (
    QApplication, QWidget, QVBoxLayout, QLineEdit, QPushButton,
    QTableWidget, QTableWidgetItem, QComboBox, QLabel
)
from PyQt6.QtCore import QThread, pyqtSignal

HOST, PORT = '127.0.0.1', 8888


class SocketWorker(QThread):
    response_received = pyqtSignal(dict) # Сигнал для передачи ответа

    def __init__(self, sock, request):
        super().__init__()
        self.sock = sock
        self.request = request

    def run(self):
        try:
            self.sock.sendall(json.dumps(self.request).encode())
            response = self.sock.recv(4096)
            self.response_received.emit(json.loads(response.decode()))
        except Exception as e:
            self.response_received.emit({"error": str(e)})

class StudentClient(QWidget):
    def __init__(self):
        super().__init__()
        self.setWindowTitle("Клиент - Управление студентами")
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.sock.connect((HOST, PORT))
        self.layout = QVBoxLayout(self)
        self.name_input = QLineEdit()
        self.name_input.setPlaceholderText("Имя")
        self.grades_input = QLineEdit()
        self.grades_input.setPlaceholderText("Оценки через запятую")
        self.add_btn = QPushButton("Добавить")
        self.filter_box = QComboBox()
        self.filter_box.addItems(["Все", "Средний ≥ 4", "Есть 5"])
        self.stats_label = QLabel()
        self.table = QTableWidget(0, 3)
        self.table.setHorizontalHeaderLabels(["Имя", "Оценки", "Средний"])
        self.layout.addWidget(self.name_input)
        self.layout.addWidget(self.grades_input)
        self.layout.addWidget(self.add_btn)
        self.layout.addWidget(self.filter_box)
        self.layout.addWidget(self.table)
        self.layout.addWidget(self.stats_label)
        self.add_btn.clicked.connect(self.add_student)
        self.filter_box.currentIndexChanged.connect(self.load_students)
        self.load_students()

    def send_request(self, command, payload=None):
        request = {"command": command, "payload": payload}
        self.worker = SocketWorker(self.sock, request)
        self.worker.response_received.connect(self.handle_response)
        self.worker.start()

    def add_student(self):
        name = self.name_input.text().strip()
        grades_text = self.grades_input.text().strip()
        if not name or not grades_text:
            return
        try:
            grades = list(map(int, grades_text.split(',')))
        except ValueError:
            return
        self.send_request("PUT", {"name": name, "grades": grades})
        self.name_input.clear()
        self.grades_input.clear()

    def load_students(self):
        self.send_request("GET-SORT")

    def handle_response(self, response):
        if "students" in response:
            students = response["students"]
            filter_type = self.filter_box.currentText()
            if filter_type == "Средний ≥ 4":
                students = [s for s in students if s["avg"] >= 4]
            elif filter_type == "Есть 5":
                students = [s for s in students if 5 in s["grades"]]

            self.table.setRowCount(0)
            for s in students:
                row = self.table.rowCount()
                self.table.insertRow(row)
                self.table.setItem(row, 0, QTableWidgetItem(s["name"]))
                self.table.setItem(row, 1, QTableWidgetItem(','.join(map(str, s["grades"]))))
                self.table.setItem(row, 2, QTableWidgetItem(f"{s['avg']:.2f}"))

            if students:
                avgs = [s["avg"] for s in students]
                self.stats_label.setText(
                    f"Студентов: {len(students)} | Мин: {min(avgs):.2f} | Макс: {max(avgs):.2f} | Средний: {sum(avgs)/len(avgs):.2f}"
                )
            else:
                self.stats_label.setText("Нет данных")
        elif "error" in response:
            self.stats_label.setText("Ошибка: " + response["error"])


if __name__ == "__main__":
    app = QApplication(sys.argv)
    win = StudentClient()
    win.show()
    sys.exit(app.exec())