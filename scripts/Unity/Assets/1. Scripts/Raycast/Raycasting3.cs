using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Valve.VR;

public class Raycasting3 : MonoBehaviour
{
    public GameObject cam;

    public GameObject homeMenu;

    public GameObject viewMenu;

    public GameObject opening;

    public GameObject openingText;
    
    public GameObject originCamera;

 public GameObject ZEDView;

 public GameObject RobotView;

 public GameObject LeftSideView;

public GameObject RightSideView;
 public GameObject TopView;

 public GameObject ForwardView;

 public SteamVR_Action_Boolean menu;
public SteamVR_Action_Boolean grip;
public SteamVR_Action_Boolean trigger;
public SteamVR_Action_Boolean center;

    public Image pointer;

   // public Toggle toggle1=null;

    RaycastHit hit;

   // public GameObject sphere;
   // public GameObject vertual3D;

   public Toggle toggle2;

   public Toggle toggle1;

    float timeElapsed;
    bool isReal=false;

    bool pointcloud_flag=false;

    bool forwardSwitch_flag=false;
      bool topSwitch_flag=false;
      bool camSwitch_flag=false;

    // Start is called before the first frame update
    void Start()
    {
         GetComponent<Rigidbody>().useGravity=false;
    }

    // Update is called once per frame
    void Update()
    {
        if(menu.GetStateDown(SteamVR_Input_Sources.Any))
            {
                homeMenu.SetActive(true);
                openingText.SetActive(false);
                opening.SetActive(false);
            }
        else if(grip.GetStateDown(SteamVR_Input_Sources.Any))
            {
                originCamera.SetActive(true);
                viewMenu.SetActive(true);
                ZEDView.SetActive(false);
                pointcloud_flag=false;

            }
            else if(trigger.GetStateDown(SteamVR_Input_Sources.Any)&&pointcloud_flag==true)
            {
                if(forwardSwitch_flag==false) 
                {
                        
                    LeftSideView.SetActive(false);
                    RightSideView.SetActive(false);
                    ForwardView.SetActive(true);
                    forwardSwitch_flag=true;
                }else if(forwardSwitch_flag==true) 
                {
                    if(camSwitch_flag==false)
                    {
                        
                        LeftSideView.SetActive(true);
                        RightSideView.SetActive(false);
                        camSwitch_flag=true;
                        }
                        else if(camSwitch_flag==true)
                        {
                            TopView.SetActive(false);
                        LeftSideView.SetActive(false);
                        RightSideView.SetActive(true);
                        camSwitch_flag=false;
                        }
                    ForwardView.SetActive(false);
                    forwardSwitch_flag=false;   
                }
            }
            else if(center.GetStateDown(SteamVR_Input_Sources.Any))
            {
                if(topSwitch_flag==false){
                    LeftSideView.SetActive(false);
                    RightSideView.SetActive(false);
                    ForwardView.SetActive(false);
                    TopView.SetActive(true);
                    topSwitch_flag=true;

                }else if(topSwitch_flag==true){
                    LeftSideView.SetActive(true);
                    ForwardView.SetActive(true);
                    TopView.SetActive(false);
                    topSwitch_flag=false;
                }
            }
       

        reticle();    
    }

    void reticle()
    {
        RaycastHit hit;
        Vector3 forward=cam.transform.TransformDirection(Vector3.forward*1000);
        
       if(Physics.Raycast(cam.transform.position,forward,out hit))
        {      
            if(hit.transform.tag=="ViewTransitionBtn")
            {    
                pointer.fillAmount=timeElapsed/2;
                timeElapsed=timeElapsed+Time.deltaTime;
                if(timeElapsed>=2 && trigger.GetStateDown(SteamVR_Input_Sources.Any))
                //if(timeElapsed>=2 && (Input.GetKeyDown(KeyCode.G)))
                {
                     Debug.Log("Hit View transition!");
                    hit.transform.GetComponent<Button>().onClick.Invoke();
                    timeElapsed=0;
                }
                
            }
            else if(hit.transform.tag=="ObjDetectionBtn")
            {
                 
                pointer.fillAmount=timeElapsed/2;
                timeElapsed=timeElapsed+Time.deltaTime;
                if(timeElapsed>=2&& trigger.GetStateDown(SteamVR_Input_Sources.Any))
                {
                    Debug.Log("ObjDetection!");
                    hit.transform.GetComponent<Button>().onClick.Invoke();
                    timeElapsed=0;
                }
            }
            else if(hit.transform.tag=="SensorTrackingBtn")
            {
                 
                pointer.fillAmount=timeElapsed/2;
                timeElapsed=timeElapsed+Time.deltaTime;
                if(timeElapsed>=2&&trigger.GetStateDown(SteamVR_Input_Sources.Any))
                {
                    Debug.Log("SensorTracking!");
                    hit.transform.GetComponent<Button>().onClick.Invoke();
                    timeElapsed=0;
                }
            }
            else if(hit.transform.tag=="QuitBtn")
            {
                
                pointer.fillAmount=timeElapsed/2;
                timeElapsed=timeElapsed+Time.deltaTime;
                if(timeElapsed>=2&&trigger.GetStateDown(SteamVR_Input_Sources.Any))
                {
                    Debug.Log("quit!");
                    hit.transform.GetComponent<Button>().onClick.Invoke();
                    timeElapsed=0;
                }
            }
            else if(hit.transform.tag=="VirtualSpaceBtn")
            
            {
               
                isReal=false;
                pointer.fillAmount=timeElapsed/2;
                timeElapsed=timeElapsed+Time.deltaTime;
                if(timeElapsed>=2&& trigger.GetStateDown(SteamVR_Input_Sources.Any))
                {
                     Debug.Log("VirtualSpace");
                    GetComponent<Rigidbody>().useGravity=true;
                    //GetComponent<Rigidbody>().useGravity=false;
                    hit.transform.GetComponent<Button>().onClick.Invoke();
                    timeElapsed=0;
                }
            }
            else if(hit.transform.tag=="DepthViewBtn")
            {
                if(isReal==false){
                    
                    pointer.fillAmount=timeElapsed/2;
                    timeElapsed=timeElapsed+Time.deltaTime;
                    if(timeElapsed>=2&& trigger.GetStateDown(SteamVR_Input_Sources.Any))
                    {
                        Debug.Log("DepthView");
                        hit.transform.GetComponent<Button>().onClick.Invoke();
                        timeElapsed=0;
                    }
                }
            }
            else if(hit.transform.tag=="RobotState")
            {
                if(isReal==false){
                    
                    pointer.fillAmount=timeElapsed/2;
                    timeElapsed=timeElapsed+Time.deltaTime;
                    if(timeElapsed>=2&& trigger.GetStateDown(SteamVR_Input_Sources.Any))
                    {
                        Debug.Log("RobotStateView");
                        hit.transform.GetComponent<Button>().onClick.Invoke();
                        timeElapsed=0;
                    }
                }
            }
            
            else if(hit.transform.tag=="RealViewBtn")
            {
                
                
                isReal=true;
                pointer.fillAmount=timeElapsed/2;
                timeElapsed=timeElapsed+Time.deltaTime;
                if(timeElapsed>=2&& trigger.GetStateDown(SteamVR_Input_Sources.Any))
                {
                    Debug.Log("RealView");
                    GetComponent<Rigidbody>().useGravity=false;
                    hit.transform.GetComponent<Button>().onClick.Invoke();
                    timeElapsed=0;
                }
            }
            else if(hit.transform.tag=="PointCloudViewBtn")
            {
                
                
                isReal=true;
                pointer.fillAmount=timeElapsed/2;
                timeElapsed=timeElapsed+Time.deltaTime;
                if(timeElapsed>=2&& trigger.GetStateDown(SteamVR_Input_Sources.Any))
                {
                    Debug.Log("PoinCloudView");
                    GetComponent<Rigidbody>().useGravity=false;
                    pointcloud_flag=true;
                    hit.transform.GetComponent<Button>().onClick.Invoke();
                    timeElapsed=0;
                }
            }
            else if(hit.transform.tag=="HomeBtn")
            {
               
                isReal=true;
                pointer.fillAmount=timeElapsed/2;
                timeElapsed=timeElapsed+Time.deltaTime;
                if(timeElapsed>=2&& trigger.GetStateDown(SteamVR_Input_Sources.Any))
                {
                     Debug.Log("Home!");
                    GetComponent<Rigidbody>().useGravity=false;
                    hit.transform.GetComponent<Button>().onClick.Invoke();
                    timeElapsed=0;
                }
            }
            else if(hit.transform.tag=="Toggle1")
            {
                
                isReal=true;
                pointer.fillAmount=timeElapsed/2;
                timeElapsed=timeElapsed+Time.deltaTime;
                if(timeElapsed>=2&& trigger.GetStateDown(SteamVR_Input_Sources.Any))
                {
                    Debug.Log("check object1");
                    GetComponent<Rigidbody>().useGravity=false;
                    toggle1.isOn=!(toggle1.isOn);
                    timeElapsed=0;
                }
            }
            else if(hit.transform.tag=="Toggle2")
            {
                
                isReal=true;
                pointer.fillAmount=timeElapsed/2;
                timeElapsed=timeElapsed+Time.deltaTime;
                if(timeElapsed>=2&& trigger.GetStateDown(SteamVR_Input_Sources.Any))
                {
                    Debug.Log("check object2");
                    GetComponent<Rigidbody>().useGravity=false;
                    toggle2.isOn=!(toggle2.isOn);
                    
                    timeElapsed=0;
                }
            }
            else
            {
                    timeElapsed=0;
                    pointer.fillAmount=timeElapsed/2;
            }   
        
        }
        //Debug.DrawRay(cam.transform.position,forward,Color.cyan);
    
    }
}
