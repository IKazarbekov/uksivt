from my_window import MyWindow
from PyQt5.QtWidgets import QApplication
import sys

# create objects
app = QApplication(sys.argv)
window = MyWindow()

# settings


# show objects
window.show()
app.exec()