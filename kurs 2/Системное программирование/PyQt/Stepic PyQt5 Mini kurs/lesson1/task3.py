import sys

from PyQt6.QtWidgets import QApplication, QWidget, QLineEdit, QPushButton, QLabel


class Window(QWidget):
    def __init__(self):
        super().__init__()
        self.initUI()
        self.setGeometry(300, 300, 500, 200)

    def initUI(self):
        self.first_line_edit = QLineEdit(self)
        self.second_line_edit = QLineEdit(self)
        self.button = QPushButton("+", self)
        self.label = QLabel(self)

        self.first_line_edit.move(0, 10)
        self.button.move(100, 10)
        self.second_line_edit.move(200, 10)
        self.label.setGeometry(300, 20, 100, 50)

        self.button.clicked.connect(self.resolution)

    def resolution(self):
        try:
            one = int(self.first_line_edit.text())
            two = int(self.second_line_edit.text())
            summa = one + two
            self.label.setText(" = " + str(summa))
        except:
            self.label.setText("Введите корректные данные")

app = QApplication(sys.argv)
window = Window()
window.show()
sys.exit(app.exec())