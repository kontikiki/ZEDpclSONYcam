using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using Valve.VR;

namespace Valve.VR
{
public class LeftCalibration : MonoBehaviour
{
    //public LeftShoulderMapping leftShoulder;
    public LeftArmMapping leftArm;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        printInitValue();
    }

    public void printInitValue(){
        Debug.Log("rot.x value of left arm still pose: "+leftArm.stillPose.rot.x);
         Debug.Log("rot.y value of left arm still pose: "+leftArm.stillPose.rot.y);
          Debug.Log("rot.z value of left arm still pose: "+leftArm.stillPose.rot.z);
           Debug.Log("rot.w value of left arm still pose: "+leftArm.stillPose.rot.w);

           Debug.Log("rot.x value of left arm T pose: "+leftArm.TPose.rot.x);
         Debug.Log("rot.y value of left arm T pose: "+leftArm.TPose.rot.y);
          Debug.Log("rot.z value of left arm T pose: "+leftArm.TPose.rot.z);
           Debug.Log("rot.w value of left arm T pose: "+leftArm.TPose.rot.w);
           

           Debug.Log("rot.x value of left arm forward pose: "+leftArm.forwardPose.rot.x);
         Debug.Log("rot.y value of left arm forward pose: "+leftArm.forwardPose.rot.y);
          Debug.Log("rot.z value of left arm forward pose: "+leftArm.forwardPose.rot.z);
           Debug.Log("rot.w value of left arm forward pose: "+leftArm.forwardPose.rot.w);
    }
}
}