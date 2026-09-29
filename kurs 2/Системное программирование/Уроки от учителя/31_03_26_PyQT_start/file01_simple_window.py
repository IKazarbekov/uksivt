from PyQt6.QtWidgets import QApplication, QWidget, QPushButton, QMainWindow
import sys

app = QApplication(sys.argv)
window = QPushButton("Pressme")# this one button in window or empty window - QWidget(), QMainWindow()
window.show()
app.exec()
