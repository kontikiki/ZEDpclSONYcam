using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildController : MonoBehaviour
{
    
    //public Rigidbody cameraRigid;
    public Rigidbody parentRigid;
    public GameObject Robot;
    public GameObject LeftCamera;
    public GameObject camera2;
    public Transform cameraTransform;
    public Transform cameraTransform2;
    // Start is called before the first frame update
    void Start()
    {
        if(SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
        {
            Debug.Log("depth texture possible!!");
        }

        Robot=GameObject.Find("dyros_tocabi_description");

        cameraTransform=LeftCamera.transform;
        cameraTransform2=camera2.transform;
        //transform.position=new Vector3(5,2,-10);
        //transform.rotation=Quaternion.Euler(new Vector3(0,0,0));
        //this.cameraTransform.localPosition=new Vector3(0,0.35,0);
        //LeftCamera.transform.localRotation=Quaternion.Euler(new Vector3(0,0,0));
        //camera2.transform.localRotation=Quaternion.Euler(new Vector3(0,0,0));
        cameraTransform2.localRotation=cameraTransform.localRotation;

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
        
        if(Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(LeftCamera.transform.forward*5*Time.deltaTime);
            camera2.transform.localRotation=LeftCamera.transform.localRotation;
          //camera2.transform.localRotation=Quaternion.Euler(new Vector3(0,0,0));
          
            //Debug.Log("forward camera local rotation: "+this.cameraTransform.localRotation);
        }

        if(Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(LeftCamera.transform.forward*(-5)*Time.deltaTime);
           camera2.transform.localRotation=LeftCamera.transform.localRotation;
           // camera2.transform.localRotation=Quaternion.Euler(new Vector3(0,0,0));
            
             //Debug.Log("backward camera local rotation: "+this.cameraTransform.localRotation);
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
             transform.Translate(LeftCamera.transform.right*(-5)*Time.deltaTime);
             camera2.transform.localRotation=LeftCamera.transform.localRotation;
             //camera2.transform.localRotation=Quaternion.Euler(new Vector3(0,0,0));
             
              //Debug.Log("left camera local rotation: "+this.cameraTransform.localRotation);
           
        }

        if(Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(LeftCamera.transform.right*5*Time.deltaTime);
            camera2.transform.localRotation=LeftCamera.transform.localRotation;
            //camera2.transform.localRotation=Quaternion.Euler(new Vector3(0,0,0));
           
             //Debug.Log("right camera local rotation: "+this.cameraTransform.localRotation);
        }


    //Rotate
        //pan
        if(Input.GetKey(KeyCode.LeftAlt)){
            Robot.transform.Rotate(new Vector3(0,30,0)*Time.deltaTime);
            /*
            cameraTransform.Rotate(new Vector3(0,30,0)*Time.deltaTime);
            cameraTransform2.Rotate(new Vector3(0,30,0)*Time.deltaTime);
            camera2.transform.localRotation=Quaternion.Euler(new Vector3(0,0,0));
            */
            //Debug.Log("pan_camera local rotation: "+this.cameraTransform.localRotation);
        }

        
        else if(Input.GetKey(KeyCode.LeftControl)){
            Robot.transform.Rotate(new Vector3(0,-30,0)*Time.deltaTime);
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
            cameraTransform2.Rotate(new Vector3(30,0,0)*Time.deltaTime);
            camera2.transform.localRotation=Quaternion.Euler(new Vector3(0,0,0));
            
            //Debug.Log("tilt_camera local rotation: "+this.cameraTransform.localRotation);
        } 

      
        else if(Input.GetKey(KeyCode.LeftShift)){
   
            cameraTransform.Rotate(new Vector3(-30,0,0)*Time.deltaTime);
            cameraTransform2.Rotate(new Vector3(-30,0,0)*Time.deltaTime);
            camera2.transform.localRotation=Quaternion.Euler(new Vector3(0,0,0));
           
            //Debug.Log("tilt_camera local rotation: "+this.cameraTransform.localRotation);
        }
        
        //yaw
        if(Input.GetKey(KeyCode.A))
        {
             cameraTransform.Rotate(new Vector3(0,0,30)*Time.deltaTime);
             cameraTransform2.Rotate(new Vector3(0,0,30)*Time.deltaTime); 
             camera2.transform.localRotation=Quaternion.Euler(new Vector3(0,0,0));
            
             //Debug.Log("yaw_camera local rotation: "+this.cameraTransform.localRotation);
        }
        else if(Input.GetKey(KeyCode.Z))
        {
         
             cameraTransform.Rotate(new Vector3(0,0,-30)*Time.deltaTime);
             cameraTransform2.Rotate(new Vector3(0,0,-30)*Time.deltaTime);
             camera2.transform.localRotation=Quaternion.Euler(new Vector3(0,0,0));
            
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
