from PyQt6.QtWidgets import QMainWindow, QApplication, QTextEdit
import sys

# created objects
app = QApplication(sys.argv)
window = QMainWindow()
textEdit = QTextEdit()

# settings
window.setCentralWidget(textEdit)
window.create

# visible and run window
window.show()
app.exec()