using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using Valve.VR;

namespace Valve.VR
{
public class RightShoulderMapping : MonoBehaviour
{
    // Start is called before the first frame update
    public enum EIndex
        {
            None = -1,
            Hmd = (int)OpenVR.k_unTrackedDeviceIndex_Hmd,
            Device1,
            Device2,
            Device3,
           
            Device4,
            Device5,
            Device6,
            Device7,
            Device8,
            Device9,
            Device10,
            Device11,
            Device12,
            Device13,
            Device14,
            Device15,
            Device16
            
        };

    public EIndex index;

        [Tooltip("If not set, relative to parent")]
        public Transform origin;

        private float timeElapsed,timeElapsed2=0;
        int i=0;
        private TrackedDevicePose_t[] poses;
       
        public bool isValid { get; private set; }

        SteamVR_Utils.RigidTransform pose;

        public SteamVR_Utils.RigidTransform stillPose;
        public SteamVR_Utils.RigidTransform TPose;
        public SteamVR_Utils.RigidTransform forwardPose;

        float avg_pos_x;
    float avg_pos_y;
    float avg_pos_z;

    float avg_rot_x;
    float avg_rot_y;
    float avg_rot_z;
    float avg_rot_w;

    bool stillFlag=false;
    bool TFlag=false;
    bool forwardFlag=false;

        public GameObject rightShoulder1;
        public GameObject rightShoulder2;
        public GameObject rightShoulder3;

           public GameObject cam;

           void Start()
        {
           
        }

        private void OnNewPoses(TrackedDevicePose_t[] poses)
        {

            if (index == EIndex.None)
                return;

            i = (int)index;
          
            isValid = false;
            if (poses.Length <= i)
                return;

            if (!poses[i].bDeviceIsConnected)
                return;

            if (!poses[i].bPoseIsValid)
                return;

            isValid = true;
            this.poses=poses;

             timeElapsed=timeElapsed+Time.deltaTime;
             if(timeElapsed > 0.05f){
            pose = new SteamVR_Utils.RigidTransform(poses[i].mDeviceToAbsoluteTracking);

            Vector3 cali_pos=new Vector3(
                (pose.pos.x),
                (pose.pos.y),
                (pose.pos.z));

            Quaternion cali_rot=new Quaternion(
            
            (pose.rot.x),
            (pose.rot.y),
            (pose.rot.z),
            (pose.rot.w));
            
           // Debug.Log("cali_rot x:"+cali_rot.x);

             Vector3 eulerAng=cali_rot.eulerAngles;
             Quaternion processedXRotation=Quaternion.Euler(new Vector3(eulerAng.x,0,0));
             Quaternion processedYRotation=Quaternion.Euler(new Vector3(0,eulerAng.y,0));
             Quaternion processedZRotation=Quaternion.Euler(new Vector3(0,0,eulerAng.z));

             rightShoulder2.transform.localRotation=processedXRotation;
             rightShoulder1.transform.localRotation=processedYRotation;
             rightShoulder3.transform.localRotation=processedZRotation;
             
             timeElapsed=0;
             }

       }

        void Update()
       {
            RaycastHit hit;
        Vector3 forward=cam.transform.TransformDirection(Vector3.forward*1000);
        
       if(Physics.Raycast(cam.transform.position,forward,out hit))
        {

           if(stillFlag==false && hit.transform.tag=="Button2")
            {   
                timeElapsed2=timeElapsed2+Time.deltaTime;
                if(timeElapsed2>=2)
                //if(timeElapsed>=2 && trigger.GetStateDown(SteamVR_Input_Sources.Any))
                //if(timeElapsed>=2 && (Input.GetKeyDown(KeyCode.G)))
                {
                     Debug.Log("Hit init save!");
                    hit.transform.GetComponent<Button>().onClick.Invoke();
                    timeElapsed2=0;
                stillFlag=true;
                measureLS();
                
                Debug.Log("Still pose measurement end. Take a T-pose.");
                }            
            }
            else if(TFlag==false && hit.transform.tag=="Button2")
            {timeElapsed2=timeElapsed2+Time.deltaTime;
                if(timeElapsed2>=2)
                //if(timeElapsed>=2 && trigger.GetStateDown(SteamVR_Input_Sources.Any))
                //if(timeElapsed>=2 && (Input.GetKeyDown(KeyCode.G)))
                {
                     Debug.Log("Hit init save!");
                    hit.transform.GetComponent<Button>().onClick.Invoke();
                    timeElapsed2=0;
                TFlag=true;
                measureLS();
                Debug.Log("T pose measurement end.Take a forward-pose.");
                }
            }
            else if(forwardFlag==false && hit.transform.tag=="Button2")
            {
                timeElapsed2=timeElapsed2+Time.deltaTime;
                if(timeElapsed2>=2)
                //if(timeElapsed>=2 && trigger.GetStateDown(SteamVR_Input_Sources.Any))
                //if(timeElapsed>=2 && (Input.GetKeyDown(KeyCode.G)))
                {
                     Debug.Log("Hit init save!");
                    hit.transform.GetComponent<Button>().onClick.Invoke();
                    timeElapsed2=0;
                forwardFlag=true;
                measureLS();
                Debug.Log("Forward pose measurement of RIGHTSHOULDER end.");
                }
            }       
        
         }
         }

        public void measureLS()
    {  
        int k=0;
        while(k<10){

            timeElapsed2=timeElapsed2+Time.deltaTime;
       //     Debug.Log("present timeElapsed : "+timeElapsed2);
            
            if(timeElapsed2>150)
            {
                pose=new SteamVR_Utils.RigidTransform(poses[i].mDeviceToAbsoluteTracking);

            avg_pos_x+=pose.pos.x;
            avg_pos_y+=pose.pos.y;
            avg_pos_z+=pose.pos.z;

            avg_rot_x+=pose.rot.x;
            avg_rot_y+=pose.rot.y;
            avg_rot_z+=pose.rot.z;
            avg_rot_w+=pose.rot.w;
            timeElapsed2=0;
            //Debug.Log("x : "+avg_rot_x);
           // Debug.Log("k= "+k);
            k++;
            }
        }

       avg_pos_x=avg_pos_x/10;
       avg_pos_y=avg_pos_y/10;
       avg_pos_z=avg_pos_z/10;
       avg_rot_x=avg_rot_x/10;
       avg_rot_y=avg_rot_y/10;
       avg_rot_z=avg_rot_z/10;
       avg_rot_w=avg_rot_w/10; 
       Debug.Log("avg_rot_x :"+avg_rot_x);
       Debug.Log("avg_rot_y :"+avg_rot_y);
       Debug.Log("avg_rot_z :"+avg_rot_z);
       Debug.Log("avg_rot_w :"+avg_rot_w);

       if(stillFlag==true&&TFlag==false)
       {
       stillPose=new SteamVR_Utils.RigidTransform(new Vector3(avg_pos_x,avg_pos_y,avg_pos_z),new Quaternion(avg_rot_x,avg_rot_y,avg_rot_z,avg_rot_w));
       }
       else if(TFlag==true && forwardFlag==false)
       {
           TPose=new SteamVR_Utils.RigidTransform(new Vector3(avg_pos_x,avg_pos_y,avg_pos_z),new Quaternion(avg_rot_x,avg_rot_y,avg_rot_z,avg_rot_w));
       }
       else if(forwardFlag==true)
       {
           forwardPose=new SteamVR_Utils.RigidTransform(new Vector3(avg_pos_x,avg_pos_y,avg_pos_z),new Quaternion(avg_rot_x,avg_rot_y,avg_rot_z,avg_rot_w));

       }

    }
        

        SteamVR_Events.Action newPosesAction;

        RightShoulderMapping()
        {
            newPosesAction = SteamVR_Events.NewPosesAction(OnNewPoses);
        }

        private void Awake()
        {
            OnEnable();
        }

        void OnEnable()
        {
            var render = SteamVR_Render.instance;
            if (render == null)
            {
                enabled = false;
                return;
            }

            newPosesAction.enabled = true;
        }

        void OnDisable()
        {
            newPosesAction.enabled = false;
            isValid = false;
        }

        public void SetDeviceIndex(int index)
        {
            if (System.Enum.IsDefined(typeof(EIndex), index))
                this.index = (EIndex)index;
        }
}
}
