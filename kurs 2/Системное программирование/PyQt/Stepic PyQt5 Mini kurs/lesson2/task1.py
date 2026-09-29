import sys

from PyQt6.QtCore import Qt
from PyQt6.QtWidgets import QApplication, QMainWindow, QLabel, QSpinBox, QHBoxLayout, QWidget, QPushButton, \
    QRadioButton, QButtonGroup, QVBoxLayout


class Window(QMainWindow):
    def __init__(self):
        super().__init__()
        self.names = ["10% (Чёрная пятница)","20% (Распродажа)","30% (Промокод)"]
        self.initUI()

    def initUI(self):
        self.setGeometry(300, 300, 800, 500)
        self.setWindowTitle("Калькулятор скидок")
        self.setUpMainWindow()
        self.show()

    def setUpMainWindow(self):
        widget = QWidget()
        self.setCentralWidget(widget)
        vbox = QVBoxLayout()

        header = QLabel("Калькулятор скидок")
        header.setStyleSheet("font-size:30px;")
        header.setFixedHeight(50)
        spinBox = QSpinBox()
        spinBox.setMaximum(10000)
        spinBox.setStyleSheet("font-size:20px;")
        spinBox.setFixedHeight(50)
        spinBox.setFixedWidth(300)
        self.spinBox = spinBox
        vbox.addWidget(header)
        vbox.addWidget(spinBox)

        group = QButtonGroup(self)
        self.group = group
        group.buttonClicked.connect(self.enables_button)
        for name in self.names:
            radio_button = QRadioButton(name)
            radio_button.setStyleSheet("font-size:20px;")
            group.addButton(radio_button)
            vbox.addWidget(radio_button)

        button = QPushButton("Рассчитать")
        self.button = button
        button.setStyleSheet("font-size:20px;")
        button.setFixedWidth(200)
        button.setEnabled(False)
        button.clicked.connect(self.solution)
        self.label_result = QLabel()
        self.label_result.setStyleSheet("font-size:20px;")
        vbox.addWidget(button)
        vbox.addWidget(self.label_result)
        vbox.setAlignment(button, Qt.AlignmentFlag.AlignHCenter)
        vbox.setAlignment(self.label_result, Qt.AlignmentFlag.AlignHCenter)

        widget.setLayout(vbox)

    def enables_button(self, button):
        self.button.setEnabled(True)

    def solution(self):
        skidka = self.group.checkedButton().text()
        int_skidka = int(skidka[:2])
        price = self.spinBox.value()
        self.label_result.setText("Цена = " + str(price * ((100 - int_skidka) / 100)))

app = QApplication(sys.argv)
window = Window()
sys.exit(app.exec())