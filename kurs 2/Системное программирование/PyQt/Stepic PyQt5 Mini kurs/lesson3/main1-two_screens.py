import sys

from PyQt6.QtWidgets import *

class Window(QMainWindow):
    def __init__(self):
        super().__init__()
        self.screen1 = QWidget()
        self.screen2 = QWidget()
        self.initUI()

    def initUI(self):
        self.setMinimumSize(800, 600)
        self.setWindowTitle("Started Layout")
        self.setUpMainWindow()
        self.show()

    def setUpMainWindow(self):
        mainWidget = QWidget()
        self.setCentralWidget(mainWidget)

        button1 = QPushButton("Start", self)
        button1.clicked.connect(self.goToScreen2)
        layout1 = QVBoxLayout(self.screen1)
        layout1.addWidget(button1)

        button2 = QPushButton("Exit", self)
        button2.clicked.connect(self.goToScreen1)
        layout2 = QVBoxLayout(self.screen2)
        layout2.addWidget(button2)

        self.stackedLayout = QStackedLayout()
        self.stackedLayout.addWidget(self.screen1)
        self.stackedLayout.addWidget(self.screen2)

        mainWidget.setLayout(self.stackedLayout)

    def goToScreen1(self):
        self.stackedLayout.setCurrentIndex(0)

    def goToScreen2(self):
        self.stackedLayout.setCurrentIndex(1)

app = QApplication(sys.argv)
window = Window()
sys.exit(app.exec())