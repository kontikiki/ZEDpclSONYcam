using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Valve.VR;

public class ChildController_pointcloud : MonoBehaviour
{
    
    //public Rigidbody cameraRigid;
    public Rigidbody parentRigid;
    public GameObject originCamera;

    public Transform cameraTransform;

    public SteamVR_Action_Boolean forward;
    public SteamVR_Action_Boolean backward;
    public SteamVR_Action_Boolean left;
    public SteamVR_Action_Boolean right;
    

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform=originCamera.transform;
        //transform.position=new Vector3(5,2,-10);
        //transform.rotation=Quaternion.Euler(new Vector3(0,0,0));
        //this.cameraTransform.localPosition=new Vector3(0,0.35,0);
        //LeftCamera.transform.localRotation=Quaternion.Euler(new Vector3(0,0,0));
        //camera2.transform.localRotation=Quaternion.Euler(new Vector3(0,0,0));

    }

    // Update is called once per frame
    void Update()
    {
   
/*  
//using parent Transform
       
        //Translate

        if(Input.GetKey(KeyCode.UpArrow))
          {
            transform.Translate(new Vector3(0,0,5)*Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0,0,-5)*Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
              transform.Translate(new Vector3(-5,0,0)*Time.deltaTime);
           
        }

        if(Input.GetKey(KeyCode.RightArrow))
        {
             transform.Translate(new Vector3(5,0,0)*Time.deltaTime);
          
        }


        //pan
        if(Input.GetKey(KeyCode.LeftAlt)){
            transform.Rotate(new Vector3(0,60,0)*Time.deltaTime);
             //Debug.Log("pan camera local rotation: "+this.cameraTransform.localRotation);
        }

        
        else if(Input.GetKey(KeyCode.LeftControl)){
            transform.Rotate(new Vector3(0,-60,0)*Time.deltaTime);
            // Debug.Log("pan camera local rotation: "+this.cameraTransform.localRotation);
        }

        //tilt
        if(Input.GetKey(KeyCode.X)){
    
            transform.Rotate(new Vector3(60,0,0)*Time.deltaTime);
            // Debug.Log("tilt camera local rotation: "+this.cameraTransform.localRotation);
        } 

      
        else if(Input.GetKey(KeyCode.LeftShift)){
   
            transform.Rotate(new Vector3(-60,0,0)*Time.deltaTime);
            // Debug.Log("tilt camera local rotation: "+this.cameraTransform.localRotation);
        }
        
        //yaw
        if(Input.GetKey(KeyCode.A))
        {
             transform.Rotate(new Vector3(0,0,60)*Time.deltaTime); 
            //  Debug.Log("yaw camera local rotation: "+this.cameraTransform.localRotation);
        }
        else if(Input.GetKey(KeyCode.Z))
        {
         
             transform.Rotate(new Vector3(0,0,-60)*Time.deltaTime);
            //  Debug.Log("yaw camera local rotation: "+this.cameraTransform.localRotation);
        }
 */



//using camera Transform
    
    //translate
        
        //if(Input.GetKey(KeyCode.UpArrow))
        if(forward.GetState(SteamVR_Input_Sources.Any))
        {
            transform.Translate(originCamera.transform.right*(-5)*Time.deltaTime);

          //camera2.transform.localRotation=Quaternion.Euler(new Vector3(0,0,0));
          
            //Debug.Log("forward camera local rotation: "+this.cameraTransform.localRotation);
        }

        //if(Input.GetKey(KeyCode.DownArrow))
        if(backward.GetState(SteamVR_Input_Sources.Any))
        {
            transform.Translate(originCamera.transform.right*(5)*Time.deltaTime);
           
           // camera2.transform.localRotation=Quaternion.Euler(new Vector3(0,0,0));
            
             //Debug.Log("backward camera local rotation: "+this.cameraTransform.localRotation);
        }

        //if(Input.GetKey(KeyCode.LeftArrow))
        if(left.GetState(SteamVR_Input_Sources.Any))
        {
             transform.Translate(originCamera.transform.forward*(-5)*Time.deltaTime);
             
             //camera2.transform.localRotation=Quaternion.Euler(new Vector3(0,0,0));
             
              //Debug.Log("left camera local rotation: "+this.cameraTransform.localRotation);
           
        }

        //if(Input.GetKey(KeyCode.RightArrow))
        if(right.GetState(SteamVR_Input_Sources.Any))
        {
            transform.Translate(originCamera.transform.forward*5*Time.deltaTime);
            
            //camera2.transform.localRotation=Quaternion.Euler(new Vector3(0,0,0));
           
             //Debug.Log("right camera local rotation: "+this.cameraTransform.localRotation);
        }


    //Rotate
        //pan
        if(Input.GetKey(KeyCode.LeftAlt)){
            cameraTransform.Rotate(new Vector3(0,30,0)*Time.deltaTime);
            /*
            cameraTransform.Rotate(new Vector3(0,30,0)*Time.deltaTime);
            cameraTransform2.Rotate(new Vector3(0,30,0)*Time.deltaTime);
            camera2.transform.localRotation=Quaternion.Euler(new Vector3(0,0,0));
            */
            //Debug.Log("pan_camera local rotation: "+this.cameraTransform.localRotation);
        }

        
        else if(Input.GetKey(KeyCode.LeftControl)){
            cameraTransform.Rotate(new Vector3(0,-30,0)*Time.deltaTime);
            /*
            cameraTransform.Rotate(new Vector3(0,-30,0)*Time.deltaTime);
            cameraTransform2.Rotate(new Vector3(0,-30,0)*Time.deltaTime);
            camera2.transform.localRotation=Quaternion.Euler(new Vector3(0,0,0));
            */
            //Debug.Log("pan_camera local rotation: "+this.cameraTransform.localRotation);
        }

        //tilt
        if(Input.GetKey(KeyCode.X)){
    
            cameraTransform.Rotate(new Vector3(30,0,0)*Time.deltaTime);
            
            
            //Debug.Log("tilt_camera local rotation: "+this.cameraTransform.localRotation);
        } 

      
        else if(Input.GetKey(KeyCode.LeftShift)){
   
            cameraTransform.Rotate(new Vector3(-30,0,0)*Time.deltaTime);
            
           
            //Debug.Log("tilt_camera local rotation: "+this.cameraTransform.localRotation);
        }
        
        //yaw
        if(Input.GetKey(KeyCode.A))
        {
             cameraTransform.Rotate(new Vector3(0,0,30)*Time.deltaTime);
             
            
             //Debug.Log("yaw_camera local rotation: "+this.cameraTransform.localRotation);
        }
        else if(Input.GetKey(KeyCode.Z))
        {
         
             cameraTransform.Rotate(new Vector3(0,0,-30)*Time.deltaTime);
            
             //Debug.Log("yaw_camera local rotation: "+this.cameraTransform.localRotation);
        }

 

        //jump 
  
        if(Input.GetKeyDown(KeyCode.Space))
        {
            this.parentRigid.AddForce(transform.up*680*1);
           //transform.Translate(transform.up*50*Time.deltaTime);
        }
        
    }

}
