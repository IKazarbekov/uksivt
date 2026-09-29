import sys

from PyQt6.QtCore import Qt
from PyQt6.QtWidgets import QApplication, QMainWindow, QLabel, QVBoxLayout, QWidget, QPushButton, QRadioButton, QButtonGroup

class Window(QMainWindow):
    def __init__(self):
        super().__init__()
        self.initUI()

    def initUI(self):
        self.setGeometry(300, 300, 800, 500)
        self.setWindowTitle("Опрос удовлетворённости")
        self.setUpMainWindow()
        self.show()

    def setUpMainWindow(self):
        widget = QWidget()
        self.setCentralWidget(widget)

        header = QLabel("Опрос удовлетворенности", self)
        header.setStyleSheet("font-size:20px;")
        header.setFixedHeight(60)
        sub_header = QLabel("Выберите вариант", self)
        sub_header.setStyleSheet("font-size:20px;")
        sub_header.setFixedHeight(40)

        vbox = QVBoxLayout()
        vbox.addWidget(header)
        vbox.addWidget(sub_header)

        radio_group = QButtonGroup(self)
        radio_names = ["Хорошо","Нормально","Плохо"]
        radio_group.buttonClicked.connect(self.check)
        for radio_name in radio_names:
            radio_button = QRadioButton(radio_name, self)
            radio_group.addButton(radio_button)
            vbox.addWidget(radio_button)

        self.button = QPushButton("Отправить")
        self.button.setFixedWidth(100)
        self.button.setFixedHeight(60)
        self.button.setEnabled(False)
        vbox.addWidget(self.button)

        vbox.setAlignment(self.button, Qt.AlignmentFlag.AlignHCenter)
        widget.setLayout(vbox)

    def check(self, button):
        print(button.text())
        self.button.setEnabled(True)

app = QApplication(sys.argv)
window = Window()
sys.exit(app.exec())