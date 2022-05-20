#!/usr/bin/env python
#-*- coding:utf-8 -*- 

import rospy
from geometry_msgs.msg import PoseStamped
from dynamixel_workbench_msgs.srv import DynamixelCommand, DynamixelCommandRequest
from scipy.spatial.transform import Rotation

def quaternion_to_euler_scipy(x, y, z, w):
    rot = Rotation.from_quat((x, y, z, w))
    rot_euler = rot.as_euler('xyz', degrees=True)
    
    return rot_euler

def orientation(pre, cur):
    if pre > cur:
        if pre >= 30 and cur < 0:
            tmp = cur - pre + 360
            tmp *= 11.38
            return tmp
        else:
            tmp = cur - pre
            tmp *= 11.38
            return tmp
    elif pre < cur:
        if pre < 0 and cur >= 30:
            tmp = cur - pre - 360
            tmp *= 11.38
            return tmp
        else:
            tmp = cur - pre 
            tmp *= 11.38
            return tmp                                    
    else:
        return 0.0

trigger = 0
order = 0

def callback(data):
    global trigger, acc_id1, acc_id2, pre_id1, pre_id2, order

    rx = data.pose.orientation.x
    ry = data.pose.orientation.y
    rz = data.pose.orientation.z
    rw = data.pose.orientation.w

    _, pitch, roll = quaternion_to_euler_scipy(rx, ry, rz, rw)    
    
    if trigger:
        acc_id2 += orientation(pre_id2, roll)                                                           # id 1 : tilt / id 2 : pan
        acc_id1 += orientation(pre_id1, pitch)                                                          # roll = pan / pitch = tilt
        if order % 5 == 0:                                                                              # limit id 1 : -60 ~ 90 (1227.2 ~ 2592.8) / id 2 : -180 ~ 180 (-113.4 ~ 3983.4)
            command = rospy.ServiceProxy('/dynamixel_workbench/dynamixel_command', DynamixelCommand)    
            if acc_id1 >= 1227.2 and acc_id1 <= 2592.8:
                resp1 = command(DynamixelCommandRequest("", 1, "Goal_Position", acc_id1))
            if acc_id2 >= -113.4 and acc_id2 <= 3983.4:
                resp2 = command(DynamixelCommandRequest("", 2, "Goal_Position", acc_id2))
        order += 1
        print("[position] id_1: " +  str(acc_id1) + " / id_2: " + str(acc_id2))
    else:
        acc_id2 = 1935.0
        acc_id1 = 1910.0
        trigger = 1

    pre_id2 = roll
    pre_id1 = pitch

def subscriber():
    rospy.init_node('topic_transform', anonymous=True)
    rospy.Subscriber('/hmd/head/odom', PoseStamped, callback)
    rospy.spin()

def init_dynamixel():
    rospy.wait_for_service('/dynamixel_workbench/dynamixel_command')
    command = rospy.ServiceProxy('/dynamixel_workbench/dynamixel_command', DynamixelCommand)
    resp1 = command(DynamixelCommandRequest("", 1, "Goal_Position", 1910))
    resp2 = command(DynamixelCommandRequest("", 2, "Goal_Position", 1935))
    
    print("[position] id_1: 1910 / id_2: 1935")
    
    subscriber()

if __name__ == "__main__":
    try:
        init_dynamixel()
    except rospy.ROSInterruptException:
        pass

'''
#### publish ros message angle(radians) ####

import math

from std_msgs.msg import Header
from trajectory_msgs.msg import JointTrajectory, JointTrajectoryPoint

def orientation(pre, cur):
    if pre > cur:
        if pre >= 30 and cur < 0:
            tmp = cur - pre + 360
            return math.radians(tmp)
        else:
            tmp = cur - pre
            return math.radians(tmp)
    elif pre < cur:
        if pre < 0 and cur >= 30:
            tmp = cur - pre - 360
            return math.radians(tmp)
        else:
            tmp = cur - pre 
            return math.radians(tmp)
    else:
        return math.radians(0)

trigger = 0

def callback(data):
    global trigger, acc_pan, acc_tilt, pre_pan, pre_tilt

    rx = data.pose.orientation.x
    ry = data.pose.orientation.y
    rz = data.pose.orientation.z
    rw = data.pose.orientation.w

    _, pitch, roll = quaternion_to_euler_scipy(rx, ry, rz, rw)

    jt = JointTrajectory()
    jt.header = Header()
    jt.joint_names = ['pan', 'tilt']  # id 1 : tilt / id 2 : pan
    jtp = JointTrajectoryPoint()
    jtp.time_from_start = rospy.Duration(0.25)
    
    if trigger:
        acc_pan += orientation(pre_pan, roll)
        acc_tilt += orientation(pre_tilt, pitch)
        jtp.positions = [acc_pan, acc_tilt] # roll = pan / pitch = tilt
    else:
        acc_pan = 0.0
        acc_tilt = 0.0
        jtp.positions = [acc_pan, acc_tilt]
        trigger = 1
    
    jt.points.append(jtp)

    pre_pan = roll
    pre_tilt = pitch
    
    publisher(jt)

def publisher(data):
    pub = rospy.Publisher('/dynamixel_workbench/joint_trajectory', JointTrajectory, queue_size=10)
    rate = rospy.Rate(10)
    pub.publish(data)

    rospy.loginfo(data)

#############################################
'''

