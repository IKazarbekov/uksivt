import sys

from PyQt6.QtWidgets import QApplication, QMainWindow, QPushButton, QLabel


class Window(QMainWindow):
    def __init__(self):
        super().__init__()
        self.initUI()
        self.setGeometry(300, 300, 300, 200)
        self.show()

    def initUI(self):
        self.label = QLabel("По умолчанию", self)
        self.label.move(140,50)
        self.button_red = QPushButton("Red", self)
        self.button_red.move(10, 10)
        self.button_red.clicked.connect(self.changeColorOnRed)

        self.button_green = QPushButton("Green", self)
        self.button_green.move(10, 50)
        self.button_green.clicked.connect(self.changeColorOnGreen)

        self.button_blue = QPushButton("Blue", self)
        self.button_blue.move(10, 100)
        self.button_blue.clicked.connect(self.changeColorOnBlue)

    def changeColorOnRed(self):
        self.label.setText("Красный")
        self.setStyleSheet("background-color: red")

    def changeColorOnGreen(self):
        self.label.setText("Зеленый")
        self.setStyleSheet("background-color: green")

    def changeColorOnBlue(self):
        self.label.setText("Голубой")
        self.setStyleSheet("background-color: blue")

app = QApplication(sys.argv)
window = Window()
sys.exit(app.exec())