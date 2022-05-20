import socket
import numpy as np
from scipy import signal

class HMDHeadPoseRead():
    def __init__(self, args):
        self.args = args
        
        self.pose = []
        
        # self.socket_init()
                       
    def socket_init(self):
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        self.socket.bind((self.args.unity_tcp_ip, self.args.unity_tcp_port))
        self.socket.listen(1)
        # print(f"[INFO] Succeeded to initialize unity tcp socket.")
        
    def read_socket(self, conn):
        data = conn.recv(self.args.unity_buffer_size)
        data = data.decode('utf-8')
        
        data = data.split("@")
        data = data[-2].split(',')
        
        try:
            self.pose = np.array([data[0], data[1]], dtype=np.float16)
        except:
            pass
