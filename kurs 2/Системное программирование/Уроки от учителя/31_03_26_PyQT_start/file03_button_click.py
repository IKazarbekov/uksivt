from PyQt6.QtCore import QSize
from PyQt6.QtWidgets import QApplication, QWidget, QPushButton, QMainWindow
import sys

class MainWindow(QMainWindow):
    def __init__(self):
        super().__init__()
        self.setWindowTitle("Hello")
        button = QPushButton("Pussme")
        button.setCheckable(True)
        def feafsfe():
            print("Hehehe")
        button.clicked.connect(feafsfe)
        self.setFixedSize(QSize(300, 400))
        self.setCentralWidget(button)
app = QApplication(sys.argv)
window = MainWindow()
window.show()
app.exec()
