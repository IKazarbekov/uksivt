from socket import socket, AF_INET, SOCK_STREAM
from PyQt6.QtWidgets import QMainWindow, QApplication, QTableWidget, QFormLayout, QSpinBox, QWidget, QVBoxLayout, \
    QLabel, \
    QTableWidgetItem, QPushButton, QStackedWidget, QLineEdit
import json
import sys
from sharing_car import Sharing_car

CODING = 'utf-8'

class MainWindow(QMainWindow):
    def __init__(self):
        super().__init__()

        # data
        self._cars = list[Sharing_car]
        self._get_data()

        # initialization
        self._initUI()
        self.setWindowTitle("Автомобили - просмотр и изменение существующих")
        self.setGeometry(300, 300, 700, 400)
        self.setMinimumSize(400, 400)
        self.show()

    def _initUI(self):
        # Screen 1 - View and edit cars
        # Labels
        label = QLabel("Автомобили", self)
        label.setStyleSheet("font-size: 40px;")

        # Table

        table = QTableWidget(len(self._cars), 4)
        table.setStyleSheet("font-size: 20px;")
        table.setHorizontalHeaderLabels(["Модель","Марка","Стоимость аренды","Средняя стоимость аренды"])
        for i, car in enumerate(self._cars):
            table.setItem(i, 0 , QTableWidgetItem(car.model))
            table.setItem(i, 1 , QTableWidgetItem(car.mark))
            table.setCellWidget(i, 2 , QSpinBox(value = car.price))
            table.setCellWidget(i, 3 , QSpinBox(value = car.sr_price))

        # Buttons
        button_update = QPushButton("Получить из сервера", self)
        button_update.setStyleSheet("font-size: 20px;")
        button_update.clicked.connect(lambda: (self._get_data(), self._initUI()))

        button_put = QPushButton("Загрузить на сервер", self)
        button_put.setStyleSheet("font-size: 20px;")
        button_put.clicked.connect(self._put_data)

        button_to_add = QPushButton("Добавить автомобиль", self)
        button_to_add.setStyleSheet("font-size: 20px;")
        button_to_add.clicked.connect(lambda: stacked.setCurrentIndex(1))

        # Screen2 - Add car.py
        # Header
        label2 = QLabel("Добавление автомобиля")
        label2.setStyleSheet("font-size: 40px;")
        button_to_table = QPushButton("Вернутся к таблице")
        button_to_table.clicked.connect(lambda: self.stacked.setCurrentIndex(0))

        # Inputs
        line_model = QLineEdit()
        line_mark = QLineEdit()
        spin_price = QSpinBox()

        # Button add
        button_add = QPushButton("Добавить")
        button_add.clicked.connect(lambda:
                                   (self._cars.append(Sharing_car(line_model.text(),
                                                                  line_mark.text(),
                                                                  spin_price.value(),
                                                                  spin_price.value())),
                                                      self._initUI(),
                                                      self._put_data(),
                                                      ))

        # Boxes and stacked
        screen_1 = QWidget()
        screen_2 = QWidget()
        screen_2.setStyleSheet("font-size: 20px;")
        stacked = QStackedWidget()
        stacked.addWidget(screen_1)
        stacked.addWidget(screen_2)
        self.setCentralWidget(stacked)

        vbox = QVBoxLayout()
        vbox.addWidget(label)
        vbox.addWidget(table)
        vbox.addWidget(button_update)
        vbox.addWidget(button_put)
        vbox.addWidget(button_to_add)
        screen_1.setLayout(vbox)

        form = QFormLayout()
        form.addRow("Модель:", line_model)
        form.addRow("Марка:", line_mark)
        form.addRow("Цена:", spin_price)
        vbox2 = QVBoxLayout()
        vbox2.addWidget(label2)
        vbox2.addWidget(button_to_table)
        vbox2.addLayout(form)
        vbox2.addWidget(button_add)
        screen_2.setLayout(vbox2)

    def _get_data(self):
        with socket(AF_INET, SOCK_STREAM) as s:
            s.connect(('localhost', 8080))
            s.sendall(json.dumps({"command":"GET"}).encode(CODING))
            answer_json = s.recv(1024).decode(CODING)
            dict_cars = json.loads(answer_json)
            self._cars = [Sharing_car(**car) for car in dict_cars]

    def _put_data(self):
        t = self.table
        # insert data in list from table
        self._cars.clear()
        for i in range(t.rowCount()):
            car = Sharing_car(t.item(i, 0).text(),
                              t.item(i, 1).text(),
                              t.cellWidget(i, 2).value(),
                              t.cellWidget(i, 3).value())
            self._cars.append(car)

        with socket(AF_INET, SOCK_STREAM) as s:
            s.connect(('localhost', 8080))
            dict_message = {'command': 'PUT', 'data': self._cars}
            json_message = json.dumps(dict_message, default=vars)
            s.sendall(json_message.encode(CODING))

app = QApplication(sys.argv)
window = MainWindow()
app.exit(app.exec())
