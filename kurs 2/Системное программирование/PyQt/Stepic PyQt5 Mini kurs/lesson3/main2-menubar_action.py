import sys

from PyQt6.QtWidgets import *
from PyQt6.QtGui import *

class Window(QMainWindow):
    def __init__(self):
        super().__init__()
        self.initUI()

    def initUI(self):
        self.setMinimumSize(800, 600)
        self.setWindowTitle("Started Layout")
        self.setUpMainWindow()
        self.show()

    def setUpMainWindow(self):


        action = QAction("Выход",self)
        action.triggered.connect(self.close)
        action.setShortcut("CTRL+Q")

        file_menu = self.menuBar().addMenu("Файл")
        file_menu.addAction(action)

        widget = QWidget()
        self.setCentralWidget(widget)
        textEdit = QTextEdit(self)
        vbox = QVBoxLayout()
        vbox.addWidget(textEdit)
        widget.setLayout(vbox)

app = QApplication(sys.argv)
window = Window()
sys.exit(app.exec())