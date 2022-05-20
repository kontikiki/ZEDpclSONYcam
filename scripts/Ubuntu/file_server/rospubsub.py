#!/usr/bin/env python

# Siemens AG, 2018
# Author: Berkay Alp Cakal (berkay_alp.cakal.ct@siemens.com)
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
# <http://www.apache.org/licenses/LICENSE-2.0>.
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

import rospy
import numpy
from geometry_msgs.msg import PoseStamped
from Xlib import display
from Xlib.ext import randr

def callback(data):
    rospy.loginfo(rospy.get_caller_id()+"I heard %f",data.pose.position.x)
    pub=rospy.Publisher('odom2',PoseStamped,queue_size=10)
    rospy.init_node('HmdToPoseStamped',anonymous=True)
    rate=rospy.Rate(10)
    pub.publish(data)
  
def HmdToPoseStamped():
    rospy.init_node('HmdToPoseStamped', anonymous = True)
    rospy.Subscriber("odom",PoseStamped,callback)
    rospy.spin()

if __name__ == '__main__':
	HmdToPoseStamped()
