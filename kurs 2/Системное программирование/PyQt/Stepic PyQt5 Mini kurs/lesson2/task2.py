import sys

from PyQt6.QtCore import Qt
from PyQt6.QtWidgets import *

class Window(QMainWindow):
    def __init__(self):
        super().__init__()
        self.notes = list()
        self.setWindowTitle("Заметки тиктокера")
        self.initUI()
        self.show()

    def initUI(self):
        widget = QWidget()
        self.setCentralWidget(widget)

        header = QLabel("Заметки тиктокера", self)
        header.setStyleSheet("font-size:20px;")

        text_edit = QTextEdit(self)
        combo_box = QComboBox(self)
        tags = ["Лайфхак","Факты","Рецепты","Юмор"]
        combo_box.addItems(tags)

        button = QPushButton("Сохранить идею", self)
        button.clicked.connect(self.add)

        self.text_edit = text_edit
        self.combo_box = combo_box

        vbox_notes = QVBoxLayout()
        for note in self.notes:
            text, tag = note
            vbox_notes.addWidget(QLabel(text, self))
            label_tag = QLabel(tag, self)
            label_tag.setStyleSheet("font-size:20px;")
            vbox_notes.addWidget(label_tag)

        hbox = QHBoxLayout(self)
        hbox.addWidget(text_edit)
        hbox.addWidget(combo_box)
        main_box = QVBoxLayout(self)
        main_box.addWidget(header)
        main_box.addLayout(hbox)
        main_box.addWidget(button)
        main_box.setAlignment(button, Qt.AlignmentFlag.AlignHCenter)
        main_box.addLayout(vbox_notes)
        widget.setLayout(main_box)

    def add(self):
        text = self.text_edit.toPlainText()
        tag = self.combo_box.currentText()
        self.notes.append((text, tag))
        self.text_edit.clear()
        self.initUI()

app = QApplication(sys.argv)
window = Window()
sys.exit(app.exec())