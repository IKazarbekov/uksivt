from PyQt5.QtWidgets import QMainWindow, QTextEdit, QAction
from PyQt5.QtGui import QKeySequence

class MyWindow(QMainWindow):


    def __init__(self):
        super().__init__()
        self.textEdit = QTextEdit()
        self.setCentralWidget(self.textEdit)

        # action close window
        action_exit = QAction("Выйти", self)
        action_exit.triggered.connect(self.close)
        action_exit.setShortcut(QKeySequence.Close)
        self.action_exit = action_exit
        self.addAction(action_exit)

        # action save file
        def save_file():
            with open("file.txt", 'w') as file:
                file.write(self.textEdit.toPlainText())
        action_save = QAction("Сохранить", self)
        action_save.triggered.connect(save_file)
        action_save.setShortcut(QKeySequence.Save)
        self.action_save = action_save
        self.addAction(action_save)

        # tool bar


        def open_file():
            pass


