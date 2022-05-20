import sys
import threading
import time
import argparse
from PyQt5.QtWidgets import QApplication

from src.dxl_control import DXLController
from src.hmd_read import HMDHeadPoseRead
from src.ui import UI

def str2bool(v):
    if v.lower() in ('yes', 'True', 'true', 't', 'y', '1'):
        return True
    elif v.lower() in ('no', 'False', 'false', 'f', 'n', '0'):
        return False
    else:
        raise argparse.ArgumentTypeError('Boolean value expected.')

def parse_args():
    parser = argparse.ArgumentParser(description='Augmentor')
    parser.add_argument('--unity_tcp_ip', '-ip', type=str,
                        default='127.0.0.1',
                        help='Unity TCP/IP, default: localhost(127.0.0.1).')
    parser.add_argument('--unity_tcp_port', type=int,
                        default=8092,
                        help='Unity TCP/IP port.')
    parser.add_argument('--unity_buffer_size', type=int,
                        default=1024,
                        help='Unity buffer size.')
    parser.add_argument('--dxl_port', type=str, 
                        default='COM4', 
                        help='Dynamixel U2D2 usb port.')
    parser.add_argument('--dxl_baudrate', '-bd', type=int, 
                        default=57600,
                        help='Dynamixel baudrate.')
    parser.add_argument('--dxl_protocol', type=float, 
                        default=2.0,
                        help='Dynamixel protocol version.')
    parser.add_argument('--dxl_series', type=str, 
                        default="X_SERIES",
                        help='Dynamixel series, default: X_SERIES.')
    parser.add_argument('--dxl_param', type=str,
                        default="DXL_SET_PARAM",
                        help='Dynamixel set parameter')
    return parser.parse_args()

HMD_POSE, TAR_POSE, CUR_POSE = [], [] ,[]
INIT_DXL_POSE, INIT_HMD_POSE = [], []
INIT_OFFSET = []

                                   # ID 1, ID 2
# DXL_SET_PARAM = {"VELOCITY_I_GAIN": [1920, 1500],
#                  "VELOCITY_P_GAIN": [ 100,  100],
#                  "POSITION_D_GAIN": [ 0,   30],
#                  "POSITION_I_GAIN": [   1,    1],
#                  "POSITION_P_GAIN": [ 100,   45],
#                  "HOME_POSE":       [2000, 1810]}

DXL_SET_PARAM = {"VELOCITY_I_GAIN": [1920, 1500],
                 "VELOCITY_P_GAIN": [ 100,  100],
                 "POSITION_D_GAIN": [   0,   30],
                 "POSITION_I_GAIN": [   1,    1],
                 "POSITION_P_GAIN": [ 100,   45],
                 "HOME_POSE":       [1910, 1935]}

CONTROL_FLAG = {"HMD_INIT": 0,
                "DXL_INIT": 0,
                "TCP_INIT": 0}

args = parse_args()
args.dxl_param = DXL_SET_PARAM
    
def main():
    """
    HELP
    """
    global args
    global HMD_POSE, TAR_POSE, CUR_POSE
    
    # app = QApplication(sys.argv)
    # ex = UI()
    # sys.exit(app.exec_())
    
    while True:
        try:
            if HMD_POSE[0] != 0 and HMD_POSE[1] != 0:
                sync_dxl_hmd()
                print("[INFO] Syncronized done between Dynamixel and HMD.")
                break
        except:
            continue
    while True:
        try:
            if HMD_POSE[0] != 0 and HMD_POSE[1] != 0:
                if CONTROL_FLAG["TCP_INIT"]:
                    print(f"[INFO] HMD: {int(HMD_POSE[0]):>4d} deg, {int(HMD_POSE[1]):>4d} deg, CURRENT_DXL: {int(CUR_POSE[0]+INIT_OFFSET[0]):>4d} deg, {int(-CUR_POSE[1]-INIT_OFFSET[1]+2*INIT_HMD_POSE[1]):>4d} deg, TARGET_DXL: {int(TAR_POSE[0]+INIT_OFFSET[0]):>4d} deg, {int(-TAR_POSE[1]-INIT_OFFSET[1]+2*INIT_HMD_POSE[1]):>4d} deg.")
                time.sleep(0.5)
        except:
            continue
      
def control_dxl():
    global CUR_POSE, INIT_DXL_POSE
    
    dxl = DXLController(id=[1, 2], param=DXL_SET_PARAM, args=args)
    print("[INFO] Dynamixel initialization complete.")
    print(f"[INFO] Press the Unity [Play] button")
    INIT_DXL_POSE = [encoder2deg(dxl.init_pose[0]), encoder2deg(dxl.init_pose[1])]
    
    while True:
        try:
            dxl1_pose = dxl.read_dxl(id=1)
            dxl2_pose = dxl.read_dxl(id=2)
            
            # if dxl1_pose != 0 and dxl2_pose != 0:
            CUR_POSE = [encoder2deg(dxl1_pose), encoder2deg(dxl2_pose)]
        
            if HMD_POSE[0] != 0 and HMD_POSE[1] != 0:
                cal_pose()
                
                dxl1_target_pose = deg2encoder(TAR_POSE[0])
                dxl2_target_pose = deg2encoder(TAR_POSE[1])
                
                if dxl1_target_pose >= 3092:
                    dxl1_target_pose = 3092
                elif dxl1_target_pose <= 1527:
                    dxl1_target_pose = 1527
                
                if dxl2_target_pose >= 3600:
                    dxl2_target_pose = 3600
                elif dxl2_target_pose <= 100:
                    dxl2_target_pose = 100
                
                dxl.write_dxl(id=1, target_pose=dxl1_target_pose)
                dxl.write_dxl(id=2, target_pose=dxl2_target_pose)
        except:
            continue
    
    dxl.portHander.closePort()

def read_hmd():
    global HMD_POSE, INIT_HMD_POSE
    hmd = HMDHeadPoseRead(args)

    while True:
        hmd.socket_init()
        conn, addr = hmd.socket.accept()
        
        CONTROL_FLAG["TCP_INIT"] = 1
        print(f"[INFO] Unity TCP/IP connection ON, address: {addr}.")
        
        while True:
            try:
                hmd.read_socket(conn)
                if hmd.pose[0] != 0 and hmd.pose[1] != 0:
                    INIT_HMD_POSE = hmd.pose
                    break
            except:
                break
        while True:
            try:
                hmd.read_socket(conn)
                HMD_POSE = hmd.pose
            except:
                break
        conn.close()
        
        CONTROL_FLAG["TCP_INIT"] = 0
        print(f"[INFO] Unity TCP/IP connection OFF, address: {addr}.")
        print(f"[INFO] Press the Unity [Play] button")
    
def sync_dxl_hmd():
    global INIT_OFFSET
    
    INIT_OFFSET = [INIT_HMD_POSE[0] - INIT_DXL_POSE[0], INIT_HMD_POSE[1] - INIT_DXL_POSE[1]]

def cal_pose():
    global TAR_POSE
    
    TAR_POSE = [HMD_POSE[0] - INIT_OFFSET[0], - HMD_POSE[1] - INIT_OFFSET[1] + 2 * INIT_HMD_POSE[1]]
    
def deg2encoder(deg):
    encoder = deg * (4096 / 360)
    return int(encoder)

def encoder2deg(encoder):
    deg = encoder * (360 / 4096)
    return deg

if __name__ == '__main__':
    args.dxl_port = input("[INFO] Enter the port of U2D2: ").upper()
    
    print(f"[INFO] Dynamixel U2D2 port [{args.dxl_port}] start.")
    
    main_thread = threading.Thread(target=main)
    main_thread.start()
    
    tcp_thread = threading.Thread(target=read_hmd)
    tcp_thread.start()
    
    dxl_thread = threading.Thread(target=control_dxl)
    dxl_thread.start()