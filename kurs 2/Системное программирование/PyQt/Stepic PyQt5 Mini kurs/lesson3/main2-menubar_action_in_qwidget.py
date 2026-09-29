import sys

from PyQt6.QtWidgets import *
from PyQt6.QtGui import *

class Window(QWidget):
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

        menuBar = QMenuBar(self)
        file_menu = menuBar.addMenu("Файл")
        file_menu.addAction(action)

        textEdit = QTextEdit(self)
        vbox = QVBoxLayout()
        vbox.addWidget(menuBar)
        vbox.addWidget(textEdit)
        self.setLayout(vbox)

app = QApplication(sys.argv)
window = Window()
sys.exit(app.exec())