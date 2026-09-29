from PyQt6.QtCore import QSize
from PyQt6.QtWidgets import QApplication, QWidget, QPushButton, QMainWindow
import sys

class MainWindow(QMainWindow):
    def __init__(self):
        super().__init__()
        self.button_is_checked = True
        self.setWindowTitle("Hello")
        button = QPushButton("Pussme")
        button.setCheckable(True)
        button.clicked.connect(self.this_button_was_togdled)
        self.setFixedSize(QSize(300, 400))
        self.setCentralWidget(button)

    def this_button_was_togdled(self, checked):
        self.button_is_checked = checked
        print(self.button_is_checked)
app = QApplication(sys.argv)
window = MainWindow()
window.show()
app.exec()
