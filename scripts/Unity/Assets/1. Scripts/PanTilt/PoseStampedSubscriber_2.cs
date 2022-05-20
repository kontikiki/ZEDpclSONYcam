/*
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace RosSharp.RosBridgeClient
{
    public class PoseStampedSubscriber_2 : UnitySubscriber<MessageTypes.Geometry.PoseStamped>
    {
        //public Transform PublishedTransform;

        public GameObject robot_head;    
        public GameObject robot_neck;

        public GameObject right_point_cloud;
        public GameObject right_rtabmapImage;
        public GameObject right_robotImage;

        public GameObject left_point_cloud;
        public GameObject left_rtabmapImage;
        public GameObject left_robotImage;

        public GameObject Rtabmap;
      
        public GameObject camView;

        public GameObject originCamera;

        public GameObject viewButton;

        public GameObject Mapping;

        public GameObject RobotCamera;

        private Vector3 position;
        private Quaternion rotation;
        private bool isMessageReceived=false;

        //for toggle
        private bool isTilt=false;
        private bool isReturn=false;

        private float timeElapsed=0;

        protected override void Start()
        {
			base.Start();
		}
		
        private void Update()
        {
            
            if (isMessageReceived){
                //timeElapsed=timeElapsed+Time.deltaTime;
                ProcessMessage();
                ToggleUI();
            }
        }

        protected override void ReceiveMessage(MessageTypes.Geometry.PoseStamped message)
        {
            position = GetPosition(message).Ros2Unity();
            rotation = GetRotation(message).Ros2Unity();

            isMessageReceived = true;
        }

        private void ProcessMessage()
        {
            //PublishedTransform.position = position;
            Vector3 eulerAng=rotation.eulerAngles;
            //PublishedTransform.rotation = rotation;

            Quaternion processedXRotation=Quaternion.Euler(new Vector3(eulerAng.x,0,0));
        Quaternion processedYRotation=Quaternion.Euler(new Vector3(0,eulerAng.y,0));

        robot_head.transform.localRotation=processedXRotation;
        robot_neck.transform.localRotation=processedYRotation;
    //    Debug.Log("rotation.x : "+rotation.x);
    //    Debug.Log("rotation.z : "+rotation.x);
    
    //for toggle scene
    //for point cloud
        if(rotation.x<-0.5 && isTilt==false)
            {
                 timeElapsed=timeElapsed+Time.deltaTime;
                if(timeElapsed>3.0){
                    isTilt=true;
                    timeElapsed=0;
                }

            }else if(rotation.x<-0.5 && isTilt==true)
            {
                 timeElapsed=timeElapsed+Time.deltaTime;
                if(timeElapsed>3.0){
                    isTilt=false;
                    timeElapsed=0;
                }
            }
        
    //for returning to home scene
        if(rotation.z>0.2 && isReturn==false)
        {
             timeElapsed=timeElapsed+Time.deltaTime;
                if(timeElapsed>3.0){
                    isReturn=true;
                    timeElapsed=0;
                }

        }
        /*
        else if(rotation.z>0.2 && isReturn==true)
        {
            timeElapsed=timeElapsed+Time.deltaTime;
                if(timeElapsed>3.0){
                    isReturn=false;
                    timeElapsed=0;
                }
        }
        */
        isMessageReceived=false;
        }

        private Vector3 GetPosition(MessageTypes.Geometry.PoseStamped message)
        {
            return new Vector3(
                (float)message.pose.position.x,
                (float)message.pose.position.y,
                (float)message.pose.position.z);
        }

        private Quaternion GetRotation(MessageTypes.Geometry.PoseStamped message)
        {
            return new Quaternion(
                (float)message.pose.orientation.x,
                (float)message.pose.orientation.y,
                (float)message.pose.orientation.z,
                (float)message.pose.orientation.w);
        }

        private void ToggleUI()
        {
            if(isTilt){
                right_point_cloud.SetActive(true);
                right_rtabmapImage.SetActive(true);
                right_robotImage.SetActive(true);
                left_point_cloud.SetActive(true);
                left_rtabmapImage.SetActive(true);
                left_robotImage.SetActive(true);
                Rtabmap.SetActive(true);
            }else if(!isTilt){
                
                right_point_cloud.SetActive(false);
                right_rtabmapImage.SetActive(false);
                right_robotImage.SetActive(false); 
                left_point_cloud.SetActive(false);
                left_rtabmapImage.SetActive(false);
                left_robotImage.SetActive(false);
                Rtabmap.SetActive(false);
            }

            
            if(isReturn){
                originCamera.SetActive(true);
                viewButton.SetActive(true);
                camView.SetActive(false);
                Rtabmap.SetActive(false);
                isReturn=false;
            
            }
            /*
            else if(!isReturn)
            {
                //originCamera.SetActive(false);
                ZEDView.SetActive(true);
                Rtabmap.SetActive(true);
               
            }
            */
            
        }
    }
}