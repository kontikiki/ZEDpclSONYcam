import os

if os.name == 'nt':
    import msvcrt
    def getch():
        return msvcrt.getch().decode()
else:
    import sys, tty, termios
    fd = sys.stdin.fileno()
    old_settings = termios.tcgetattr(fd)
    def getch():
        try:
            tty.setraw(sys.stdin.fileno())
            ch = sys.stdin.read(1)
        finally:
            termios.tcsetattr(fd, termios.TCSADRAIN, old_settings)
        return ch

from dynamixel_sdk import * # Uses Dynamixel SDK library

def DXL_config(MY_DXL):
    # Control table address
    config = dict()
    if MY_DXL == 'X_SERIES' or MY_DXL == 'MX_SERIES':
        config["ADDR_TORQUE_ENABLE"] = 64 
        config["ADDR_GOAL_POSITION"] = 116
        config["ADDR_PRESENT_POSITION"] = 132
        config["ADDR_VELOCITY_I_GAIN"] = 76
        config["ADDR_VELOCITY_P_GAIN"] = 78
        config["ADDR_POSITION_D_GAIN"] = 80
        config["ADDR_POSITION_I_GAIN"] = 82
        config["ADDR_POSITION_P_GAIN"] = 84
        # config["DXL_MINIMUM_POSITION_VALUE"] = 0
        # config["DXL_MAXIMUM_POSITION_VALUE"] = 4095
    elif MY_DXL == 'PRO_SERIES':
        config["ADDR_TORQUE_ENABLE"] = 562 
        config["ADDR_GOAL_POSITION"] = 596
        config["ADDR_PRESENT_POSITION"] = 611
        # config["DXL_MINIMUM_POSITION_VALUE"] = -150000
        # config["DXL_MAXIMUM_POSITION_VALUE"] = 150000
    elif MY_DXL == 'P_SERIES' or MY_DXL == 'PRO_A_SERIES':
        config["ADDR_TORQUE_ENABLE"] = 512 
        config["ADDR_GOAL_POSITION"] = 564
        config["ADDR_PRESENT_POSITION"] = 580
        # config["DXL_MINIMUM_POSITION_VALUE"] = -150000
        # config["DXL_MAXIMUM_POSITION_VALUE"] = 150000
    elif MY_DXL == 'XL320':
        config["ADDR_TORQUE_ENABLE"] = 24
        config["ADDR_GOAL_POSITION"] = 30
        config["ADDR_PRESENT_POSITION"] = 37
        # config["DXL_MINIMUM_POSITION_VALUE"] = 0
        # config["DXL_MAXIMUM_POSITION_VALUE"] = 1023
    return config

class DXLController():
    def __init__(self, id, param, args):
        self.args = args
        self.param = param
        
        self.config = DXL_config(self.args.dxl_series)
        
        self.TORQUE_ENABLE               = 1
        self.TORQUE_DISABLE              = 0
        
        self.init_pose = []
        
        try:
            self.init_u2d2()
        except:
            print(f"[ERROR] Port ERROR [{self.args.dxl_port}].")
            # print(f"[ERROR] ")
        
        try:
            [self.init_dxl(i) for i in id]
        except:
            print(f"[ERROR] Dynamixel initializtion is Fail")
            
        
    def init_u2d2(self):
        try:
            self.portHandler.closePort()
        except:
            pass
        
        self.portHandler = PortHandler(self.args.dxl_port)
        self.packetHandler = PacketHandler(self.args.dxl_protocol)
        
        if self.portHandler.openPort():
            print("[INFO] Succeeded to open the port")
        else:
            print("[ERROR] Failed to open the port")
            print("[ERROR] Press any key to terminate...")
            getch()
            quit()
        
        if self.portHandler.setBaudRate(self.args.dxl_baudrate):
            print("[INFO] Succeeded to change the baudrate")
        else:
            print("[ERROR] Failed to change the baudrate")
            print("[ERROR] Press any key to terminate...")
            getch()
            quit()
            
    def init_dxl(self, id):
        dxl_comm_result, dxl_error = self.packetHandler.write1ByteTxRx(self.portHandler, 
                                                                       id, 
                                                                       self.config["ADDR_TORQUE_ENABLE"], 
                                                                       self.TORQUE_ENABLE)
        # """
        dxl_comm_result, dxl_error = self.packetHandler.write4ByteTxRx(self.portHandler, 
                                                                        id, 
                                                                        self.config["ADDR_POSITION_P_GAIN"], 
                                                                        45)
        dxl_comm_result, dxl_error = self.packetHandler.write4ByteTxRx(self.portHandler, 
                                                                        id, 
                                                                        self.config["ADDR_POSITION_D_GAIN"], 
                                                                        1000)
        self.write_dxl(id, self.param["HOME_POSE"][id - 1])
        
        while True:
            if self.read_dxl(id) in range(self.param["HOME_POSE"][id - 1]-200, self.param["HOME_POSE"][id - 1]+200):
                time.sleep(1)
                # PID setting
                pid_gain_list = ["VELOCITY_I_GAIN", "VELOCITY_P_GAIN", "POSITION_D_GAIN", "POSITION_I_GAIN", "POSITION_P_GAIN"]
                for pid_gain in pid_gain_list:
                    addr_gain = self.config[f"ADDR_{pid_gain}"]
                    dxl_comm_result, dxl_error = self.packetHandler.write4ByteTxRx(self.portHandler, 
                                                                                id, 
                                                                                addr_gain, 
                                                                                self.param[pid_gain][id - 1])
                time.sleep(0.1)
                self.write_dxl(id, self.param["HOME_POSE"][id - 1])
                time.sleep(0.5)
                break
            else:
                continue
        # """
        
        # pid_gain_list = ["VELOCITY_I_GAIN", "VELOCITY_P_GAIN", "POSITION_D_GAIN", "POSITION_I_GAIN", "POSITION_P_GAIN"]
        # for pid_gain in pid_gain_list:
        #     addr_gain = self.config[f"ADDR_{pid_gain}"]
        #     dxl_comm_result, dxl_error = self.packetHandler.write4ByteTxRx(self.portHandler, 
        #                                                                 id, 
        #                                                                 addr_gain, 
        #                                                                 self.param[pid_gain][id - 1])
        # time.sleep(0.1)
            
        if dxl_comm_result != COMM_SUCCESS:
            print(f"[ERROR di1] {self.packetHandler.getTxRxResult(dxl_comm_result)}")
        elif dxl_error != 0:
            print(f"[ERROR di2] {self.packetHandler.getRxPacketError(dxl_error)}")
        else:
            print(f"[INFO] Dynamixel [ID: {id}] has been successfully connected.")
            
            self.init_pose.append(self.read_dxl(id))
    
    def read_dxl(self, id):
        dxl_present_position, dxl_comm_result, dxl_error = self.packetHandler.read4ByteTxRx(self.portHandler, 
                                                                                            id,
                                                                                            self.config["ADDR_PRESENT_POSITION"])
        if dxl_comm_result != COMM_SUCCESS:
            print(f"[ERROR dr1] {self.packetHandler.getTxRxResult(dxl_comm_result)}")
        elif dxl_error != 0:
            print(f"[ERROR dr2] {self.packetHandler.getRxPacketError(dxl_error)}")
        
        return dxl_present_position
    
    def write_dxl(self, id, target_pose):
        dxl_comm_result, dxl_error = self.packetHandler.write4ByteTxRx(self.portHandler, 
                                                                       id,
                                                                       self.config["ADDR_GOAL_POSITION"],
                                                                       target_pose)
        if dxl_comm_result != COMM_SUCCESS:
            print(f"[ERROR dw1] {self.packetHandler.getTxRxResult(dxl_comm_result)}")
        elif dxl_error != 0:
            print(f"[ERROR dw2] {self.packetHandler.getRxPacketError(dxl_error)}")
