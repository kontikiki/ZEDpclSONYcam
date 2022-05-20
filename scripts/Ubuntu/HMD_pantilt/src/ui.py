import sys
from PyQt5.QtWidgets import QApplication, QWidget, QPushButton
from PyQt5.QtGui import QIcon
from PyQt5.QtCore import QCoreApplication

class UI(QWidget):
    def __init__(self):
        super().__init__()
        
        self.init()
        
    def init(self):
        btn = QPushButton('Quit', self)
        btn.move(50, 50)
        btn.resize(btn.sizeHint())
        btn.clicked.connect(QCoreApplication.instance().quit)
        
        
        self.setWindowTitle("KETI DXL Control")
        self.setWindowIcon(QIcon('./src/keti_icon.png'))
        self.setGeometry(300, 300, 300, 200)
        # self.resize(400, 200)
        self.show()
        
if __name__ == '__main__':
    app = QApplication(sys.argv)
    ex = UI()
    sys.exit(app.exec_())