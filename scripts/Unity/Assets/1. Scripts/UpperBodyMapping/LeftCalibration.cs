    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.XR;
    using UnityEngine.UI;
    using Valve.VR;

    //using MathNet.Numerics.LinearAlgebra;
    //using MathNet.Numerics.LinearAlgebra.Double;

    public class LeftCalibration : MonoBehaviour
    {
        //public LeftShoulderMapping leftShoulder;
        public LeftArmMapping leftArm;
        
        public bool caliFlag=false; 
        public Text detailText;
        //public RightShoulderMapping rightShoulder;
        //public RightArmMapping rightArm;

        Matrix4x4 m_Still=Matrix4x4.identity;
        Matrix4x4 m_T=Matrix4x4.identity;
        Matrix4x4 m_Forward=Matrix4x4.identity;
        
        Matrix4x4 forward_Inverse=Matrix4x4.identity;

        public Vector3 stillTranslation;
        public Quaternion stillRotation;
        public Vector3 stillScale;

        public Vector3 TTranslation;
        public Quaternion TRotation;
        public Vector3 TScale;

        public Vector3 forwardTranslation;
        public Quaternion forwardRotation;
        public Vector3 forwardScale;



        // Start is called before the first frame update
        void Start()
        {
            
        
        }

        // Update is called once per frame
        void Update()
        {
            if(leftArm.stillFlag && caliFlag==false)
            {
                stillTranslation=new Vector3(leftArm.stillPose.pos.x,leftArm.stillPose.pos.y,leftArm.stillPose.pos.z);
                
                Vector3 trans_euler=(new Quaternion(leftArm.stillPose.rot.x,leftArm.stillPose.rot.y,leftArm.stillPose.rot.z,leftArm.stillPose.rot.w)).eulerAngles;
                stillRotation=Quaternion.Euler(trans_euler);
                //stillRotation=new Quaternion(leftArm.stillPose.rot.x,leftArm.stillPose.rot.y,leftArm.stillPose.rot.z,leftArm.stillPose.rot.w);
                //Debug.Log("stillRotation.x : "+stillRotation.x);
                //forwardRotation=new Quaternion(leftArm.forwardPose.rot.x,leftArm.forwardPose.rot.y,leftArm.forwardPose.rot.z,leftArm.forwardPose.rot.w);
                stillScale = new Vector3(1, 1, 1);
                
                m_Still.SetTRS(stillTranslation,stillRotation,stillScale);
                printInitValue();
            }
            else if(leftArm.TFlag && caliFlag==false)
            {
                TTranslation=new Vector3(leftArm.TPose.pos.x,leftArm.TPose.pos.y,leftArm.TPose.pos.z);
                //Debug.Log("leftArm.forwardPose.rot.x: "+leftArm.forwardPose.rot.x);
                Vector3 trans_euler=(new Quaternion(leftArm.TPose.rot.x,leftArm.TPose.rot.y,leftArm.TPose.rot.z,leftArm.TPose.rot.w)).eulerAngles;
                TRotation=Quaternion.Euler(trans_euler);
                //TRotation=new Quaternion(leftArm.TPose.rot.x,leftArm.TPose.rot.y,leftArm.TPose.rot.z,leftArm.TPose.rot.w);
                //Debug.Log("forwardRotation.x : "+forwardRotation.x);
                //forwardRotation=new Quaternion(leftArm.forwardPose.rot.x,leftArm.forwardPose.rot.y,leftArm.forwardPose.rot.z,leftArm.forwardPose.rot.w);
                TScale = new Vector3(1, 1, 1);
                
                m_T.SetTRS(TTranslation,TRotation,TScale);
                printInitValue();
            }
            else if(leftArm.forwardFlag && caliFlag==false)
            {
                
                forwardTranslation=new Vector3(leftArm.forwardPose.pos.x,leftArm.forwardPose.pos.y,leftArm.forwardPose.pos.z);
                //Debug.Log("leftArm.forwardPose.rot.x: "+leftArm.forwardPose.rot.x);
                Vector3 trans_euler=(new Quaternion(leftArm.forwardPose.rot.x,leftArm.forwardPose.rot.y,leftArm.forwardPose.rot.z,leftArm.forwardPose.rot.w)).eulerAngles;
                forwardRotation=Quaternion.Euler(trans_euler);
                //forwardRotation=new Quaternion(leftArm.forwardPose.rot.x,leftArm.forwardPose.rot.y,leftArm.forwardPose.rot.z,leftArm.forwardPose.rot.w);
                //Debug.Log("forwardRotation.x : "+forwardRotation.x);
                //forwardRotation=new Quaternion(leftArm.forwardPose.rot.x,leftArm.forwardPose.rot.y,leftArm.forwardPose.rot.z,leftArm.forwardPose.rot.w);
                forwardScale = new Vector3(1, 1, 1);
                
                m_Forward.SetTRS(forwardTranslation,forwardRotation,forwardScale);
                printInitValue();
                caliFlag=true;
            }
        }        
    

    public void printInitValue(){
/*
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
*/
            forward_Inverse=m_Forward.inverse;

            for(int i=0;i<4;i++)
            {
                
                Debug.Log(i+"-row of Forward_pose ORIGINAL matrix");
                for(int j=0;j<4;j++)
                {
                    Vector4 a=m_Forward.GetRow(i);
                    
                    
                    Debug.Log(" "+a[j]);
                }
                Debug.Log("\n");
            }

    /*
            for(int i=0;i<4;i++)
            {
                Debug.Log(i+"-row of Forward_pose INVERSE matrix");
                for(int j=0;j<4;j++)
                {
                    Vector4 b=forward_Inverse.GetRow(i);
                    
                    Debug.Log(" "+b[j]);
                }
                Debug.Log("\n");
            }
    */     
        }
    }

