import time
from socket import socket, AF_INET, SOCK_STREAM
from PyQt6.QtWidgets import QMainWindow, QApplication, QMessageBox, QWidget, QLabel, QTableWidget, QVBoxLayout, \
    QStackedWidget, QTableWidgetItem, QSpinBox, QPushButton
import sys
import json
from car import Sharing_car

class MainWindow(QMainWindow):
    def __init__(self):
        # data
        self._socket = None
        self._cars = list[Sharing_car]()

        # init
        super().__init__()
        self._connect()
        self._get_data()
        self._init_UI()
        self.show()

    def _init_UI(self):
        # SCREEN 1
        # header
        label = QLabel("Редактирование существующих автомобилей")

        # table
        count_cars = len(self._cars) if self._cars is not None else 0
        table = QTableWidget(count_cars, 4)
        table.setHorizontalHeaderLabels(["Модель","Марка","Цена","Средняя цена"])
        for i, car in enumerate(self._cars):
            table.setItem(i, 0, QTableWidgetItem(car.model))
            table.setItem(i, 1, QTableWidgetItem(car.mark))
            table.setCellWidget(i, 2, QSpinBox(value=car.price))
            table.setCellWidget(i, 3, QSpinBox(value=car.sr_price))

        # buttons
        button_push = QPushButton("отправить на сервер")
        button_push.clicked.connect(self._put_data)

        # BOXES
        screen1 = QWidget()
        vbox = QVBoxLayout()
        vbox.addWidget(label)
        vbox.addWidget(table)
        vbox.addWidget(button_push)
        screen1.setLayout(vbox)

        screen2 = QWidget()

        stacked = QStackedWidget()
        stacked.addWidget(screen1)
        stacked.addWidget(screen2)
        self.setCentralWidget(stacked)

        #links
        self._table = table

    def closeEvent(self, event):
        self._put_data()
        self.disconnect()

    def _get_data(self):
        json_message = json.dumps({"command":"GET"})
        self._socket.sendall(json_message.encode())
        json_cars = self._socket.recv(1024).decode()
        list_cars = json.loads(json_cars)
        self._cars.clear()
        for car in list_cars:
            self._cars.append(Sharing_car(**car))

    def _put_data(self):
        self._cars.clear()
        table = self._table
        for i in range(table.rowCount()):
            model = table.item(i, 0).text()
            mark = table.item(i, 1).text()
            price = table.cellWidget(i, 2).value()
            sr_price = table.cellWidget(i, 3).value()
            car = Sharing_car(model=model, mark=mark, price=price, sr_price=sr_price)
            self._cars.append(car)
        json_message = json.dumps({"command":"PUT", "cars":self._cars}, default=vars)
        self._socket.sendall(json_message.encode())

    def _connect(self):
        self._socket = socket(AF_INET, SOCK_STREAM)
        try:
            self._socket.connect(('127.0.0.1', 8080))
        except ConnectionRefusedError:
            QMessageBox.information(self, "wadaw", "Соединение с сервером не удалась")

    def _disconnect(self):
        self.socket.close()

app = QApplication(sys.argv)
window = MainWindow()
app.exit(app.exec())