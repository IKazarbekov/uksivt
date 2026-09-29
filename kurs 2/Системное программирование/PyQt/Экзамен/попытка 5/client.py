import json

from PyQt6.QtCore import QThread, pyqtSignal
from PyQt6.QtWidgets import QMainWindow, QWidget, QStackedWidget, QVBoxLayout, QLabel, QComboBox, QTableWidget, \
    QPushButton, QLineEdit, QApplication, QTableWidgetItem, QSpinBox
import sys, socket

HOST_PORT = ('127.0.0.1', 8080)

class SocketWorker(QThread):
    signal = pyqtSignal(dict)

    def __init__(self, sock, request):
        super().__init__()
        self.sock = sock
        self.request = request
        print("\tСоздан сокет процесс для отправки запроса: ", request)

    def run(self):
        try:
            print("Запущен процесс отправки запроса")
            self.sock.sendall(json.dumps(self.request).encode())
            print("Отправил запрос, жду ответ от сервера...")
            response = json.loads(self.sock.recv(1024).decode())
            print("Получил ответ, вызываю слушателя")
            self.signal.emit(response)
        except Exception as e:
            print("ОШИБКА СОКЕТА ПРОЦЕССА:", str(e))

class MainWindow(QMainWindow):
    def __init__(self):
        super().__init__()

        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

        self._connect()
        self._initUI()

    def closeEvent(self, event):
        self.sock.close()

    def _initUI(self):
        print("Инициализация интейфейса началась ...")
        self.screen1 = QWidget()
        self.vbox1 = QVBoxLayout()
        self.screen1.setLayout(self.vbox1)
        self.label1 = QLabel("Список студентов")
        self.comboBoxFilter = QComboBox()
        self.comboBoxFilter.addItems(["Все студенты","Средний балл более 4","Хотябы одна 5"])
        self.comboBoxSort = QComboBox()
        self.comboBoxSort.addItems(["Прямой порядок","Обратный порядок","Смешанный порядок"])
        self.table = QTableWidget(0, 3)
        self.table.setHorizontalHeaderLabels(["Имя","Оценки","Средний балл"])
        self.button_get = QPushButton("Обновить данные")
        self.button_get.clicked.connect(self._method_get)
        self.button_to_put = QPushButton("Добавтиь студента")
        self.button_to_put.clicked.connect(lambda: self.stackedWidget.setCurrentIndex(1))
        self.vbox1.addWidget(self.label1)
        self.vbox1.addWidget(self.comboBoxFilter)
        self.vbox1.addWidget(self.comboBoxSort)
        self.vbox1.addWidget(self.table)
        self.vbox1.addWidget(self.button_get)
        self.vbox1.addWidget(self.button_to_put)

        self.screen2 = QWidget()
        self.vbox2 = QVBoxLayout()
        self.screen2.setLayout(self.vbox2)
        self.label2 = QLabel("Добавление студента")
        self.label_name = QLabel("Имя")
        self.line_name = QLineEdit()
        self.label_grades = QLabel("Оценки")
        self.line_grades = QLineEdit()
        self.button_put = QPushButton("Добавить")
        self.button_put.clicked.connect(self._method_put)
        self.button_back = QPushButton("Назад к списку")
        self.button_back.clicked.connect(lambda: self.stackedWidget.setCurrentIndex(0))
        self.vbox2.addWidget(self.label_name)
        self.vbox2.addWidget(self.line_name)
        self.vbox2.addWidget(self.label_grades)
        self.vbox2.addWidget(self.line_grades)
        self.vbox2.addWidget(self.button_put)
        self.vbox2.addWidget(self.button_back)

        self.stackedWidget = QStackedWidget()
        self.stackedWidget.addWidget(self.screen1)
        self.stackedWidget.addWidget(self.screen2)
        self.setCentralWidget(self.stackedWidget)

        print("Инициализация интейфейса успешна завершена")

    def _method_get(self):
        match self.comboBoxSort.currentIndex():
            case 0:
                self._send_request({ "command" : "GET-SORT"})
            case 1:
                self._send_request({ "command" : "GET-REVERSE" })
            case 2:
                self._send_request({ "command" : "GET-SHUFFLE" })
            case _:
                print("Что то не так с comboBoxSort")

    def _method_put(self):
        payload = {"name": self.line_name.text(), "grades": list(map(int, self.line_grades.text().split(",")))}
        self._send_request({"command" : "PUT", "payload" : payload})

    def _connect(self):
        print("Пробую подключится к серверу ...")
        self.sock.connect(HOST_PORT)
        print("Подключение успешно завершенно")

    def _send_request(self, request):
        print(f"Хочу отправить запрос {request} серверу")
        self.worker = SocketWorker(self.sock, request)
        self.worker.signal.connect(self._handler_server)
        self.worker.start()

    def _handler_server(self, response):
        print("\tСлушатель сервера принял данные ...")
        if "students" in response:
            students = response["students"]
            print("Данные верные:", students)
            print("Выполняю фильтрацию данных")
            if self.comboBoxFilter.currentIndex() == 1:
                students = list(filter(lambda student: student["avg"] >= 4, students))
            elif self.comboBoxFilter.currentIndex() == 2:
                students = list(filter(lambda student: 5 in student["grades"], students))
            print("Выполнил фильтрацию на основе comboBoxFilter, вот отфильтрованный список: ", students)

            print("\tЗаполняю таблицу ...")
            self.table.setRowCount(len(students))
            for i, student in enumerate(students):
                self.table.setItem(i, 0, QTableWidgetItem(student["name"]))
                self.table.setItem(i, 1, QTableWidgetItem(str(student["grades"])))
                self.table.setCellWidget(i, 2, QSpinBox(value = int(student["avg"])))
            print("\tТаблица была успешно заполнена")

        else:
            print("\tОШИБКА СЛУШАТЕЛЬ НЕ ПОЛУЧИЛ НОРМАЛЬНЫЕ ДАННЫЕ")

app = QApplication(sys.argv)
window = MainWindow()
window.show()
app.exit(app.exec())