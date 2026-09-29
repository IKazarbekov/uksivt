import socket

from PyQt6.QtCore import QThread, pyqtSignal
from PyQt6.QtWidgets import QWidget, QVBoxLayout, QLabel, QTableWidget, QPushButton, QLineEdit, QStackedWidget, \
    QApplication, QMainWindow, QTableWidgetItem
import sys, json

class SocketWorker(QThread):
    signal = pyqtSignal(dict)

    def __init__(self, sock, request):
        super().__init__()
        self.sock = sock
        self.request = request

    def run(self):
        try:
            self.sock.sendall(json.dumps(self.request).encode())
            data = self.sock.recv(1024)
            response = json.loads(data.decode('utf-8'))
            self.signal.emit(response)
        except Exception as e:
            print(e)

class MainWindow(QMainWindow):
    def __init__(self):
        super().__init__()

        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.sock.connect(('localhost', 8080))

        self.setFixedSize(500, 300)
        self._initUI()

    def closeEvent(self, event):
        self.sock.close()

    def _initUI(self):
        self.screen1 = QWidget()
        self.vbox1 = QVBoxLayout()
        self.screen1.setLayout(self.vbox1)
        self.label1 = QLabel("Список медицинских кард")
        self.vbox1.addWidget(self.label1)
        self.table = QTableWidget(0, 4)
        self.table.setHorizontalHeaderLabels(["Номер",
                                              "Фамилия",
                                              "Посещения(числа этого месяца)",
                                              "Среднее время посещения" ])
        self.vbox1.addWidget(self.table)
        self.button_update = QPushButton("Обновить")
        self.button_update.clicked.connect(lambda: self._send_request({"command":"GET-SORT"}))
        self.button_to_put = QPushButton("Добавить карту")
        self.button_to_put.clicked.connect(lambda: self.stackedWidget.setCurrentIndex(1))
        self.vbox1.addWidget(self.button_update)
        self.vbox1.addWidget(self.button_to_put)

        self.screen2 = QWidget()
        self.vbox2 = QVBoxLayout()
        self.screen2.setLayout(self.vbox2)
        self.label2 = QLabel("Добавление карты")
        self.label_number = QLabel("Номер карты")
        self.label_family = QLabel("Фамилия владельца")
        self.label_visits = QLabel("Посещения к врачу")
        self.label_sr_time = QLabel("Среднее время посещения")
        self.line_number = QLineEdit()
        self.line_family = QLineEdit()
        self.line_visits = QLineEdit()
        self.line_sr_time = QLineEdit()
        self.button_put = QPushButton("Добавить")
        self.button_back = QPushButton("Назад")
        self.button_back.clicked.connect(lambda: self.stackedWidget.setCurrentIndex(0))
        self.vbox2.addWidget(self.label_number)
        self.vbox2.addWidget(self.line_number)
        self.vbox2.addWidget(self.label_family)
        self.vbox2.addWidget(self.line_family)
        self.vbox2.addWidget(self.label_visits)
        self.vbox2.addWidget(self.line_visits)
        self.vbox2.addWidget(self.label_sr_time)
        self.vbox2.addWidget(self.line_sr_time)
        self.vbox2.addWidget(self.button_put)
        self.vbox2.addWidget(self.button_back)

        self.stackedWidget = QStackedWidget()
        self.stackedWidget.addWidget(self.screen1)
        self.stackedWidget.addWidget(self.screen2)
        self.setCentralWidget(self.stackedWidget)

    def _send_request(self, request):
        self.worker = SocketWorker(self.sock, request)
        self.worker.signal.connect(self._server_handler)
        self.worker.start()

    def _server_handler(self, response):
        if "data" in response:
            cards_dict = response["data"]
            print(cards_dict)
            self.table.setRowCount(len(cards_dict))
            for i, card in enumerate(cards_dict):
                self.table.setItem(i, 0, QTableWidgetItem(card["nomber"]))
                self.table.setItem(i, 1, QTableWidgetItem(card["familia"]))
                self.table.setItem(i, 2, QTableWidgetItem(str(card["visits"])))
                self.table.setItem(i, 3, QTableWidgetItem(str(card["sr"])))

app = QApplication(sys.argv)
window = MainWindow()
window.show()
app.exit(app.exec())
