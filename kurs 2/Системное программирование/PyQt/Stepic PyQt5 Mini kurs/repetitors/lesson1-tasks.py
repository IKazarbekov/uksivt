import sys

from PyQt6.QtWidgets import QApplication, QMainWindow, QPushButton, QLabel, QLineEdit

class Window(QMainWindow):
    def __init__(self):
        super().__init__()
        self.count = 0
        self.setGeometry(300, 300, 300, 300)
        self.initUI()
        self.show()

    def initUI(self):
        button_red = QPushButton("RED", self)
        button_green = QPushButton("GREEN", self)
        button_blue = QPushButton("BLUE", self)
        button_red.clicked.connect(self.red)
        button_green.clicked.connect(self.green)
        button_blue.clicked.connect(self.blue)
        button_red.move(0, 100)
        button_green.move(100, 100)
        button_blue.move(200, 100)
        self.label_color = QLabel(self)
        self.label_color.setText("color")
        self.label_color.setGeometry(100, 40, 100, 60)
        self.label_color.setStyleSheet("font-size:20px; font-weight:bold; padding:10px;")

        self.label_count = QLabel(str(self.count), self)
        self.label_count.setGeometry(0, 0, 100, 70)
        self.label_count.setStyleSheet("font-size:20px; font-weight:bold; padding:10px;")
        self.label_count_ = QLabel(str(10 - self.count), self)
        self.label_count_.setGeometry(100, 0, 100, 70)
        self.label_count_.setStyleSheet("font-size:20px; font-weight:bold; padding:10px;")
        self.button_count_plus = QPushButton("+", self)
        self.button_count_plus.clicked.connect(self.plus)

        self.line1 = QLineEdit(self)
        self.line1.move(0, 150)
        self.line2 = QLineEdit(self)
        self.line2.move(200, 150)
        self.labelre = QLabel(self)
        self.labelre.move(300, 0)
        self.button_resol = QPushButton("=", self)
        self.button_resol.move(300, 200)
        self.button_resol.clicked.connect(self.resolution)

    def resolution(self):
        try:
            self.labelre.setText(str((int(self.line1.text()) + int(self.line2.text()))))
        except:
            self.labelre.setText("Error")

    def plus(self):
        self.count += 1
        self.label_count.setText(str(self.count))
        self.label_count_.setText(str(str(10 - self.count)))
        if self.count >= 10:
            self.button_count_plus.setEnabled(False)

    def red(self):
        self.setStyleSheet("background-color: red")
        self.label_color.setText("red")

    def green(self):
        self.setStyleSheet("background-color: green")
        self.label_color.setText("green")

    def blue(self):
        self.setStyleSheet("background-color: blue")
        self.label_color.setText("blue")

app = QApplication(sys.argv)
window = Window()
sys.exit(app.exec())