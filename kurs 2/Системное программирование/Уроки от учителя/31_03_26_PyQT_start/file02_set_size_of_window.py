from PyQt6.QtCore import QSize
from PyQt6.QtWidgets import QApplication, QWidget, QPushButton, QMainWindow
import sys

class MainWindow(QMainWindow):
    def __init__(self):
        super().__init__()
        self.setWindowTitle("Hello")
        button = QPushButton("Pussme")
        #self.setFixedSize(QSize(300, 400)) # fixed size
        self.setMinimumSize(QSize(100, 100))
        self.setMaximumSize(QSize(1000, 1000))
        self.setCentralWidget(button)
app = QApplication(sys.argv)
window = MainWindow()
window.show()
app.exec()
