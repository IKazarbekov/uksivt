import sys

from PyQt6.QtCore import Qt
from PyQt6.QtWidgets import QApplication, QMainWindow, QLabel, QLineEdit, QHBoxLayout, QWidget, QPushButton, QRadioButton, QButtonGroup

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

        label = QLabel("Код подверждения: ")
        line_edit = QLineEdit()
        line_edit.textEdited.connect(self.enabled_button)
        self.button = QPushButton("отправить")
        self.button.setEnabled(False)

        hbox = QHBoxLayout()
        hbox.addWidget(label)
        hbox.addWidget(line_edit)
        hbox.addWidget(self.button)

        widget.setLayout(hbox)

    def enabled_button(self, text):
        self.button.setEnabled(len(text) > 0)

    def button_clicked(self):
        ...

app = QApplication(sys.argv)
window = Window()
sys.exit(app.exec())