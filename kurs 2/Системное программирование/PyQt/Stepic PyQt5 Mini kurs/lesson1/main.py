import sys
from PyQt6.QtWidgets import QApplication, QWidget, QLabel, QPushButton, QLineEdit
from PyQt6.QtGui import QPixmap


class MainWindow(QWidget):
    def __init__(self):
        super().__init__()
        self.count = 0
        self.initializeUI()

    def initializeUI(self):
        self.setGeometry(300, 300, 400, 400)
        self.setWindowTitle("Заголовок окна")
        self.setUpMainWindow()
        self.show()

    def setUpMainWindow(self):
        image = "icon.png"
        with open(image):
            image_label = QLabel(self)
            image_label.setPixmap(QPixmap(image))
            image_label.setGeometry(0, 0, 250, 200)

        self.line_edit = QLineEdit(self)
        self.line_edit.move(100, 250)
        self.buttonPlus = QPushButton("plus to count", self)
        self.buttonPlus.move(20, 250)
        self.buttonPlus.clicked.connect(self.save)

        self.label_count = QLabel(str(self.count), self)
        self.label_count.setGeometry(30, 300, 80, 60)
        self.label_count.setStyleSheet("font-size: 40px; padding: 10px;")
        button = QPushButton("plus", self)
        button.clicked.connect(self.plus)
        button.move(100, 300)

    def save(self):
        self.count = int(self.line_edit.text())
        self.label_count.setText(str(self.count))

    def plus(self):
        self.count += 1
        self.label_count.setText(str(self.count))

app = QApplication(sys.argv)
window = MainWindow()
sys.exit(app.exec())