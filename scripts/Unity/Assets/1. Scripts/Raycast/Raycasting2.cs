using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Raycasting2 : MonoBehaviour
{
    public GameObject cam;

    public GameObject homeMenu;

    public GameObject opening;

    public GameObject openingText;
    


    public Image pointer;

   // public Toggle toggle1=null;


    RaycastHit hit;

   // public GameObject sphere;
   // public GameObject vertual3D;

   public Toggle toggle2;

   public Toggle toggle1;

    float timeElapsed;
    bool isReal=false;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        reticle();    
    }

    void reticle()
    {
        RaycastHit hit;
        Vector3 forward=cam.transform.TransformDirection(Vector3.forward*1000);
        
        if(OVRInput.Get(OVRInput.Button.Start,OVRInput.Controller.LTouch))
        //if(Input.GetKeyDown(KeyCode.H))
            {
                homeMenu.SetActive(true);
                openingText.SetActive(false);
                //opening.SetActive(false);
            }

       if(Physics.Raycast(cam.transform.position,forward,out hit))
        //if(Physics.Raycast(ray,out hit, 50f))
        {
            
            if(hit.transform.tag=="ViewTransitionBtn")
            {
                
                pointer.fillAmount=timeElapsed/2;
                timeElapsed=timeElapsed+Time.deltaTime;
                if(timeElapsed>=2 && (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger,OVRInput.Controller.LTouch)!=0))
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
                if(timeElapsed>=2&& (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger,OVRInput.Controller.LTouch)!=0))
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
                if(timeElapsed>=2&& (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger,OVRInput.Controller.LTouch)!=0))
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
                if(timeElapsed>=2&&(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger,OVRInput.Controller.LTouch)!=0))
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
                if(timeElapsed>=2&& (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger,OVRInput.Controller.LTouch)!=0))
                {
                     Debug.Log("VirtualSpace");
                    GetComponent<Rigidbody>().useGravity=true;
                    hit.transform.GetComponent<Button>().onClick.Invoke();
                    timeElapsed=0;
                }
            }
            else if(hit.transform.tag=="DepthViewBtn")
            {
                if(isReal==false){
                    
                    pointer.fillAmount=timeElapsed/2;
                    timeElapsed=timeElapsed+Time.deltaTime;
                    if(timeElapsed>=2&& (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger,OVRInput.Controller.LTouch)!=0))
                    {
                        Debug.Log("DepthView");
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
                if(timeElapsed>=2&& (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger,OVRInput.Controller.LTouch)!=0))
                {
                    Debug.Log("RealView");
                    GetComponent<Rigidbody>().useGravity=false;
                    hit.transform.GetComponent<Button>().onClick.Invoke();
                    timeElapsed=0;
                }
            }
            else if(hit.transform.tag=="HomeBtn")
            {
               
                isReal=true;
                pointer.fillAmount=timeElapsed/2;
                timeElapsed=timeElapsed+Time.deltaTime;
                if(timeElapsed>=2&& (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger,OVRInput.Controller.LTouch)!=0))
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
                if(timeElapsed>=2&& (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger,OVRInput.Controller.LTouch)!=0))
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
                if(timeElapsed>=2&& (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger,OVRInput.Controller.LTouch)!=0))
                {
                    Debug.Log("check object2");
                    GetComponent<Rigidbody>().useGravity=false;
                    toggle2.isOn=!(toggle2.isOn);
                    //hit.transform.GetComponent<Toggle>().onValueChanged.AddListener(delegate{task(toggle2.isOn);});
                    timeElapsed=0;
                }
            }

            else
            {
                   // pointer.fillAmount=timeElapsed/2;
                   // timeElapsed=timeElapsed-Time.deltaTime;
                    timeElapsed=0;
                    pointer.fillAmount=timeElapsed/2;
                   // if(timeElapsed<=0) timeElapsed=0;
            }   
        
        }
        Debug.DrawRay(cam.transform.position,forward,Color.cyan);
    
    }
}