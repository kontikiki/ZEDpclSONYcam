using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Valve.VR;

public class ButtonManager3 : MonoBehaviour
{
    /*
    public GameObject homeMenu;
    public GameObject viewMenu;

     //public GameObject openingText;

    public GameObject detectionMenu;
    public GameObject sensorMenu;
    public GameObject settingsMenu;

    public GameObject virtual3D;
    public GameObject depth;
*/

/*
    public GameObject toggle1;

    public GameObject toggle2;
*/

     public GameObject robotView;

    public GameObject rtabmap;

    public GameObject pointcloud;
  

    public GameObject originCamera;

    public GameObject camView;

    public GameObject viewButton;

    public GameObject pointcloudView;

    public GameObject rtabmapView;

    public GameObject Mapping;

    public LeftArmMapping leftArm;
    public LeftShoulderMapping leftShoulder;
    public LeftCalibration leftCal;

    bool rtabmap_flag=false;

    bool pointcloud_flag=false;
    bool realCamera_flag=false;
    bool robotState_flag=false;

    //bool depthView_flag=false;
     //bool virtual3D_flag=false;
    
    void Start()
    {
    originCamera.SetActive(true);
    //originCamera.SetActive(false);
    camView.SetActive(false);
    //camView.SetActive(true);



 //pointcloud.SetActive(true);
 //       pointcloudView.SetActive(true);
    }
/*
    public void viewTransition()
    {
        homeMenu.SetActive(false);
        viewMenu.SetActive(true);
        Debug.Log("btn view transition");
    }

    public void ObjDetection()
    {
        homeMenu.SetActive(false);
        detectionMenu.SetActive(true);
    }

    public void SensorTracking()
    {
        homeMenu.SetActive(false);
        sensorMenu.SetActive(true);
    }

    public void Settings()
    {
        homeMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void quit(){
        homeMenu.SetActive(false);
        viewMenu.SetActive(false);
        detectionMenu.SetActive(false);
        sensorMenu.SetActive(false);
        settingsMenu.SetActive(false);

    }
*/

    //SonyAction cam Activation
    public void goReal()
    {
        //viewMenu.SetActive(false);
        if(realCamera_flag==false){
            camView.SetActive(true);
            viewButton.SetActive(false);
            originCamera.SetActive(false);
        realCamera_flag=true;
        }else if(realCamera_flag==true){
            camView.SetActive(false);
             originCamera.SetActive(true);
        realCamera_flag=false;
        }
    }

/*
    public void goRobot(){
        viewMenu.SetActive(false);
        if(robotState_flag==false){
            robotView.SetActive(true);
            robotState_flag=true;
        }else if(robotState_flag==true)
        {
            robotView.SetActive(false);
            robotState_flag=false;
        }
    }

    public void goVR()
    {
        viewMenu.SetActive(false);
        if(virtual3D_flag==false){
            virtual3D.SetActive(true);
            virtual3D_flag=true;
        }else if(virtual3D_flag==true)
        {
            virtual3D.SetActive(false);
            virtual3D_flag=false;
        }
    }

    public void goDepth()
    {
        viewMenu.SetActive(false);
        if(depthView_flag==false){
            depth.SetActive(true);
            depthView_flag=true;
        }else if(depthView_flag==true)
        {
            depth.SetActive(false);
            depthView_flag=false;
        }
    }

    public void goPointCloud()
    {
        viewMenu.SetActive(false);

        if(pointcloud_flag==false){
        //originCamera.enabled=false;
        viewMenu.SetActive(false);

        pointcloud.SetActive(true);
        //pointcloudView.SetActive(true);
        //originCamera.SetActive(false);
        
        pointcloud_flag=true;
            
        }
        else if(pointcloud_flag==true){
            //originCamera.enabled=true;
            
            pointcloud.SetActive(false);
        //    pointcloudView.SetActive(false);
            //originCamera.SetActive(true);
             pointcloud_flag=false;
        }
    }

    public void goHome()
    {
        homeMenu.SetActive(true);
        viewMenu.SetActive(false);
        detectionMenu.SetActive(false);
        sensorMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void goToggle1(){
        //Debug.Log("Toggle1 !");
    }

    public void goToggle2(){
         //Debug.Log("Toggle2 !");
    }
    
*/
    public void goCalibSave(){
        Mapping.SetActive(true);
    }

    public void reset()
    {
        leftArm.stillFlag=false;
        leftArm.TFlag=false;
        leftArm.forwardFlag=false;

        leftShoulder.stillFlag=false;
        leftShoulder.TFlag=false;
        leftShoulder.forwardFlag=false;

        leftCal.caliFlag=false;
    }
}
