import sys

from PyQt6.QtWidgets import QApplication, QMainWindow, QLabel, QPushButton

class Window(QMainWindow):
    def __init__(self):
        super().__init__()
        self.clicks = -1
        self.initUI()

    def initUI(self):
        self.setGeometry(300, 300, 900, 700)

        QLabel("Кол-во нажатий: ", self).move(10, 10)
        self.first_label = QLabel(self)
        QLabel("Осталось: ", self).move(10, 50)
        self.second_label = QLabel(self)

        self.first_label.move(140, 10)
        self.second_label.move(70, 50)

        self.button = QPushButton("+", self)
        self.button.clicked.connect(self.click)
        self.button.move(150, 30)

        self.click()
        self.show()

    def click(self):
        self.clicks += 1

        self.first_label.setText(str(self.clicks))
        self.second_label.setText(str(10 - self.clicks))

        if self.clicks >= 10:
            self.button.setEnabled(False)

app = QApplication(sys.argv)
window = Window()
sys.exit(app.exec())
