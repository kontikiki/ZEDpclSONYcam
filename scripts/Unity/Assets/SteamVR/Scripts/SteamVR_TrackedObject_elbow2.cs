//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: For controlling in-game objects with tracked devices.
//
//=============================================================================

using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Valve.VR;

namespace Valve.VR
{
    public class SteamVR_TrackedObject_elbow2 : MonoBehaviour
    {
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

        private float timeElapsed=0;
        int i=0;
        private TrackedDevicePose_t[] poses;
       // SteamVR_Utils.RigidTransform init_pose;

        public bool isValid { get; private set; }

        SteamVR_Utils.RigidTransform pose;
        SteamVR_Utils.RigidTransform init_pose;

        private void OnNewPoses(TrackedDevicePose_t[] poses)
        {

            if (index == EIndex.None)
                return;

            i = (int)index;
            Debug.Log("index : "+i);
            isValid = false;
            if (poses.Length <= i)
                return;

            if (!poses[i].bDeviceIsConnected)
                return;

            if (!poses[i].bPoseIsValid)
                return;

            isValid = true;
            this.poses=poses;
       }

       void Update()
       {
            timeElapsed=timeElapsed+Time.deltaTime;
             if(timeElapsed > 0.05f){
            pose = new SteamVR_Utils.RigidTransform(poses[i].mDeviceToAbsoluteTracking);

            Vector3 cali_pos=new Vector3(
                (pose.pos.x-init_pose.pos.x),
                (pose.pos.y-init_pose.pos.y),
                (pose.pos.z-init_pose.pos.z));

            Quaternion cali_rot=new Quaternion(
            (pose.rot.x-init_pose.rot.x),
            (pose.rot.y-init_pose.rot.y),
            (pose.rot.z-init_pose.rot.z),
            (pose.rot.w-init_pose.rot.w));
/*
            if (origin != null)
            {
            //    transform.position = origin.transform.TransformPoint(cali_pos);
            //    transform.rotation = origin.rotation* cali_rot;
            //Vector3 eulerAng=(origin.rotation*cali_rot).eulerAngles;
            Vector3 eulerAng=cali_rot.eulerAngles;
            Quaternion processedXRotation=Quaternion.Euler(new Vector3(eulerAng.x,0,0));
                 Debug.Log("processed Rotation X : "+processedXRotation.x);
                  transform.localRotation=processedXRotation;
            }
           else
           {
 */              
                 Vector3 eulerAng=cali_rot.eulerAngles;
                // Vector3 initEulerAng=(init_pose.rot).eulerAngles;
                // Vector3 caliAng=eulerAng-initEulerAng;
               Quaternion processedXRotation=Quaternion.Euler(new Vector3(eulerAng.x,0,0));
                 Debug.Log("processed Rotation X : "+processedXRotation.x);
                // Debug.Log("cnt: "+cnt);
                
                //transform.localPosition = pose.pos;
               // transform.localRotation = pose.rot;
                transform.localRotation=processedXRotation;
//            }
            timeElapsed=0;
            
          
       }
       }
       
   void Start()
    {
            init_pose=new SteamVR_Utils.RigidTransform(poses[i].mDeviceToAbsoluteTracking);

            Debug.Log("init_pose rotation : "+init_pose.rot.x);
            Invoke("Update",1);
    }

        SteamVR_Events.Action newPosesAction;

        SteamVR_TrackedObject_elbow2()
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