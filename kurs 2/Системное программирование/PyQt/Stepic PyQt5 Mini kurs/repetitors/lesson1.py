import sys
from PyQt6.QtWidgets import QApplication, QMainWindow, QLabel

class MainWindow(QMainWindow):
    def __init__(self):
        super().__init__()
        self.initializeUI()

    def initializeUI(self):
        self.setGeometry(300, 300, 300, 300)
        self.setWindowTitle("The Window")
        self.setUpMainWindow()
        self.show()

    def setUpMainWindow(self):
        label = QLabel("Надпись", self)
        label.move(10, 10)

app = QApplication(sys.argv)
window = MainWindow()
sys.exit(app.exec())
