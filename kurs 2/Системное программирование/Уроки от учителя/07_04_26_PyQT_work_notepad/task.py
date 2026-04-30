import sys
from PyQt5.QtWidgets import QMainWindow, QApplication, QTextEdit, QAction, QLabel, QFileDialog, QMessageBox
from PyQt5.QtGui import QKeySequence, QIcon


class TextEditor(QMainWindow):
    def __init__(self):
        super().__init__()

        self.setWindowTitle("Мой блокнот")
        self.setGeometry(100,100,400,300)
        self.setCentralWidget(QTextEdit())
        self.centralWidget().textChanged.connect(self.updateStatusBar)

        self.currentFilePath = None

        action_new = QAction("Новый", self)
        action_open = QAction("Открыть", self)
        action_save = QAction("Сохранить", self)
        action_save_as = QAction("Сохранить как", self)
        action_undo = QAction("Отменить", self)
        action_redo = QAction("Повторить", self)
        action_cut = QAction("Вырезать", self)
        action_copy = QAction("Копировать", self)
        action_paste = QAction("Вставить", self)
        action_cut.setEnabled(False)
        action_copy.setEnabled(False)
        action_undo.setEnabled(False)
        action_redo.setEnabled(False)
        self.centralWidget().undoAvailable.connect(action_undo.setEnabled)
        self.centralWidget().redoAvailable.connect(action_redo.setEnabled)
        self.centralWidget().copyAvailable.connect(action_cut.setEnabled)
        self.centralWidget().copyAvailable.connect(action_copy.setEnabled)
        action_new.triggered.connect(self.new_document)
        action_paste.triggered.connect(self.centralWidget().paste)
        action_copy.triggered.connect(self.centralWidget().copy)
        action_redo.triggered.connect(self.centralWidget().redo)
        action_cut.triggered.connect(self.centralWidget().cut)
        action_undo.triggered.connect(self.centralWidget().undo)
        action_open.triggered.connect(self.openFile)
        action_save.triggered.connect(self.saveFile)
        action_save_as.triggered.connect(self.saveFileAs)
        action_new.setShortcut(QKeySequence.New)
        action_open.setShortcut('Ctrl+O')
        action_save.setShortcut(QKeySequence.Save)
        action_save_as.setShortcut(QKeySequence.SaveAs)
        action_undo.setShortcut(QKeySequence.Undo)
        action_redo.setShortcut(QKeySequence.Redo)
        action_paste.setShortcut(QKeySequence.Paste)
        action_copy.setShortcut(QKeySequence.Copy)
        action_cut.setShortcut(QKeySequence.Cut)
        self.action_new = action_new
        self.action_save = action_save
        self.action_save_as = action_save_as
        self.action_open = action_open
        self.action_copy = action_copy
        self.action_paste = action_paste
        self.action_undo = action_undo
        self.action_redo = action_redo
        self.action_cut = action_cut
        self.createMenuBar()
        self.createToolBar()
        self.createStatusBar()

    def createMenuBar(self):
        menubar = self.menuBar()

        file_menu = menubar.addMenu("Файл")
        file_menu.addAction(self.action_new)
        file_menu.addAction(self.action_open)
        file_menu.addAction(self.action_save)
        file_menu.addAction(self.action_save_as)

        edit_menu = menubar.addMenu("Правка")
        edit_menu.addAction(self.action_undo)
        edit_menu.addAction(self.action_redo)
        edit_menu.addSeparator()
        edit_menu.addAction(self.action_copy)
        edit_menu.addAction(self.action_paste)
        edit_menu.addAction(self.action_cut)

    def createToolBar(self):
        tool_bar = self.addToolBar("Инструменты")

        tool_bar.addAction(self.action_new)
        tool_bar.addAction(self.action_open)
        tool_bar.addAction(self.action_save)

    def createStatusBar(self):
        label = QLabel("Символов 0")
        self.label = label

        self.statusBar().addWidget(label)

    def updateStatusBar(self):
        self.label.setText(f"Символов: {len(self.centralWidget().toPlainText())}")

    def new_document(self):
        if self.centralWidget().document().isModified():
            answer = QMessageBox.question(self,
                                          "Файл не сохранён",
                                          "У вас есть не сохранённые изменения, хотите их сохранить?",
                                          QMessageBox.Save | QMessageBox.No | QMessageBox.Cancel)
        else:
            answer = QMessageBox.No
        match answer:
            case QMessageBox.Save:
                self.saveFile()
            case QMessageBox.Cancel:
                return
            case QMessageBox.No:
                pass
            case _:
                return
        self.centralWidget().clear()
        self.currentFilePath = None

    def openFile(self):
        path, type = QFileDialog.getOpenFileName(
            self,
            "Открыть файл",
            "",
            "Текстовый файл(*.txt);;Все файлы(*.*)"
        )
        if path:
            try:
                with open(path, 'r', encoding='utf-8') as file:
                    text = file.read()
                    self.centralWidget().setText(text)
                    self.currentFilePath = path
                    self.centralWidget().document().setModified(False)
            except Exception as e:
                QMessageBox.warning(self, "Ошибка", f"Не удалось найти файл{str(e)}")

    def saveFileAs(self):
        path, type = QFileDialog.getSaveFileName(
            self,
            "Сохранить файл",
            "",
            "Текстовый файл(*.txt);;Все файлы(*.*)"
        )
        if path:
            try:
                with open(path, 'w', encoding='utf-8') as file:
                    file.write(self.centralWidget().toPlainText())
                    self.currentFilePath = path
                    self.centralWidget().document().setModified(False)
                    return True
            except Exception as e:
                QMessageBox.warning(self, "Ошибка", f"Не удалось сохранить файл {str(e)}")
                return False
        return False

    def saveFile(self):
        if self.currentFilePath is None:
            return self.saveFileAs()
        try:
            with open(self.currentFilePath, 'w', encoding='utf-8') as file:
                file.write(self.centralWidget().toPlainText())
                self.centralWidget().document().setModified(False)
                return True
        except Exception as e:
            QMessageBox.warning(self, "Ошибка", f"Не удалось сохранить файл {str(e)}")
            return False

    def closeEvent(self, event):
        if self.centralWidget().document().isModified():
            reply = QMessageBox.question(
                self,
                "Не сохранённые изменения",
                "Текст был изменён. Сохранить изменения",
                QMessageBox.Save | QMessageBox.Discard | QMessageBox.Cancel
            )
            match reply:
                case QMessageBox.Save:
                    if self.saveFile():
                        event.accept()
                    else:
                        event.ignore()
                case QMessageBox.Discard:
                    event.accept()
                case QMessageBox.Cancel:
                    event.ignore()
                case _:
                    event.accept()

if __name__ == '__main__':
    app = QApplication(sys.argv)
    window = TextEditor()
    window.show()
    app.exec()