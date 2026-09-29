import sys
import numpy as np
from PyQt6.QtWidgets import (QApplication, QMainWindow, QWidget, QVBoxLayout,
                             QHBoxLayout, QSlider, QLabel, QGridLayout)
from PyQt6.QtCore import Qt
from PyQt6.QtGui import QPainter, QPen, QColor, QFont


class PlotWidget(QWidget):
    def __init__(self, parent=None):
        super().__init__(parent)
        self.w1 = 1.0
        self.w2 = 1.0
        self.w0 = 0.0
        self.setMinimumSize(500, 500)

    def set_weights(self, w1, w2, w0):
        self.w1 = w1
        self.w2 = w2
        self.w0 = w0
        self.update()

    def paintEvent(self, event):
        painter = QPainter(self)
        painter.setRenderHint(QPainter.RenderHint.Antialiasing)

        # Рисуем белый фон
        painter.fillRect(self.rect(), QColor(255, 255, 255))

        # Отступы для осей
        margin = 50
        width = self.width() - 2 * margin
        height = self.height() - 2 * margin

        # Центр графика
        cx = margin + width // 2
        cy = margin + height // 2

        # Масштаб: от -5 до 5 по обеим осям
        scale = min(width, height) / 10

        # Рисуем сетку и оси
        painter.setPen(QPen(QColor(200, 200, 200), 1, Qt.PenStyle.DashLine))
        for i in range(-5, 6):
            if i == 0:
                continue
            # Вертикальная линия
            x = cx + i * scale
            painter.drawLine(x, margin, x, self.height() - margin)
            # Горизонтальная линия
            y = cy - i * scale
            painter.drawLine(margin, y, self.width() - margin, y)

        # Рисуем оси
        painter.setPen(QPen(QColor(0, 0, 0), 2))
        painter.drawLine(margin, cy, self.width() - margin, cy)  # Ось X
        painter.drawLine(cx, margin, cx, self.height() - margin)  # Ось Y

        # Подписи осей
        painter.setPen(QPen(QColor(0, 0, 0), 1))
        font = QFont("Arial", 10)
        painter.setFont(font)
        painter.drawText(self.width() - margin - 20, cy - 10, "x1")
        painter.drawText(cx + 10, margin + 20, "x2")

        # Подписи делений
        for i in range(-5, 6):
            if i == 0:
                continue
            painter.drawText(cx + i * scale - 5, cy + 20, str(i))
            painter.drawText(cx - 25, cy - i * scale + 5, str(i))
        painter.drawText(cx - 5, cy + 20, "0")

        # Рисуем случайные точки двух классов
        np.random.seed(42)
        # Класс 1 (красные) - вокруг (2, 2)
        points1 = np.random.randn(30, 2) * 0.8 + np.array([2, 2])
        # Класс 2 (синие) - вокруг (-2, -2)
        points2 = np.random.randn(30, 2) * 0.8 + np.array([-2, -2])

        for x, y in points1:
            px = cx + x * scale
            py = cy - y * scale
            painter.setPen(QPen(QColor(255, 0, 0), 2))
            painter.setBrush(QColor(255, 0, 0))
            painter.drawEllipse(int(px - 4), int(py - 4), 8, 8)

        for x, y in points2:
            px = cx + x * scale
            py = cy - y * scale
            painter.setPen(QPen(QColor(0, 0, 255), 2))
            painter.setBrush(QColor(0, 0, 255))
            painter.drawEllipse(int(px - 4), int(py - 4), 8, 8)

        # Рисуем разделяющую линию: w1*x1 + w2*x2 + w0 = 0
        painter.setPen(QPen(QColor(0, 255, 0), 3))

        if abs(self.w2) < 1e-6:
            # Вертикальная линия
            if abs(self.w1) > 1e-6:
                x1 = -self.w0 / self.w1
                px = cx + x1 * scale
                painter.drawLine(px, margin, px, self.height() - margin)
        else:
            # Обычная линия
            x1_min = -5
            x1_max = 5
            x2_min = -(self.w1 * x1_min + self.w0) / self.w2
            x2_max = -(self.w1 * x1_max + self.w0) / self.w2

            p1_x = -5
            p1_y = -(self.w1 * p1_x + self.w0) / self.w2
            p2_x = 5
            p2_y = -(self.w1 * p2_x + self.w0) / self.w2

            if p1_y < -5:
                p1_y = -5
                p1_x = -(self.w2 * p1_y + self.w0) / self.w1 if abs(self.w1) > 1e-6 else -5
            if p1_y > 5:
                p1_y = 5
                p1_x = -(self.w2 * p1_y + self.w0) / self.w1 if abs(self.w1) > 1e-6 else 5
            if p2_y < -5:
                p2_y = -5
                p2_x = -(self.w2 * p2_y + self.w0) / self.w1 if abs(self.w1) > 1e-6 else -5
            if p2_y > 5:
                p2_y = 5
                p2_x = -(self.w2 * p2_y + self.w0) / self.w1 if abs(self.w1) > 1e-6 else 5

            px1 = cx + p1_x * scale
            py1 = cy - p1_y * scale
            px2 = cx + p2_x * scale
            py2 = cy - p2_y * scale
            painter.drawLine(int(px1), int(py1), int(px2), int(py2))

        # Показываем текущие значения весов на графике
        painter.setPen(QPen(QColor(0, 0, 0), 1))
        painter.setFont(QFont("Arial", 12))
        info = f"w1 = {self.w1:.2f},  w2 = {self.w2:.2f},  w0 = {self.w0:.2f}"
        painter.drawText(10, 30, info)


class MainWindow(QMainWindow):
    def __init__(self):
        super().__init__()
        self.setWindowTitle("Визуализация разделяющей линии")
        self.setGeometry(100, 100, 700, 700)

        central_widget = QWidget()
        self.setCentralWidget(central_widget)

        layout = QVBoxLayout(central_widget)

        # Виджет с графиком
        self.plot = PlotWidget()
        layout.addWidget(self.plot)

        # Панель управления
        controls = QGridLayout()

        # Ползунок для w1
        controls.addWidget(QLabel("w1"), 0, 0)
        self.slider_w1 = QSlider(Qt.Orientation.Horizontal)
        self.slider_w1.setRange(-100, 100)
        self.slider_w1.setValue(10)
        self.slider_w1.valueChanged.connect(self.update_weights)
        controls.addWidget(self.slider_w1, 0, 1)
        self.label_w1 = QLabel("1.00")
        controls.addWidget(self.label_w1, 0, 2)

        # Ползунок для w2
        controls.addWidget(QLabel("w2"), 1, 0)
        self.slider_w2 = QSlider(Qt.Orientation.Horizontal)
        self.slider_w2.setRange(-100, 100)
        self.slider_w2.setValue(10)
        self.slider_w2.valueChanged.connect(self.update_weights)
        controls.addWidget(self.slider_w2, 1, 1)
        self.label_w2 = QLabel("1.00")
        controls.addWidget(self.label_w2, 1, 2)

        # Ползунок для w0
        controls.addWidget(QLabel("w0"), 2, 0)
        self.slider_w0 = QSlider(Qt.Orientation.Horizontal)
        self.slider_w0.setRange(-100, 100)
        self.slider_w0.setValue(0)
        self.slider_w0.valueChanged.connect(self.update_weights)
        controls.addWidget(self.slider_w0, 2, 1)
        self.label_w0 = QLabel("0.00")
        controls.addWidget(self.label_w0, 2, 2)

        layout.addLayout(controls)

        # Применяем начальные значения
        self.update_weights()

    def update_weights(self):
        w1 = self.slider_w1.value() / 10.0
        w2 = self.slider_w2.value() / 10.0
        w0 = self.slider_w0.value() / 10.0

        self.label_w1.setText(f"{w1:.2f}")
        self.label_w2.setText(f"{w2:.2f}")
        self.label_w0.setText(f"{w0:.2f}")

        self.plot.set_weights(w1, w2, w0)


if __name__ == "__main__":
    app = QApplication(sys.argv)
    window = MainWindow()
    window.show()
    sys.exit(app.exec())