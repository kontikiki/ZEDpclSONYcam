using System;
using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient.MessageTypes.Sensor;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;


namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]
    public class PointCloudSubscriber_lidar : UnitySubscriber<MessageTypes.Sensor.PointCloud2>
    {
        private byte[] byteArray;
        private bool isMessageReceived = false;
        bool readyToProcessMessage = true;
        public int size_byte;

        public int size;


        public Vector3[] pcl;
        public Color[] pcl_color;

        int width;
        int height;
        int row_step;
        int point_step;

        protected override void Start()
        {
            base.Start();
        }

        public void Update()
        {

            if (isMessageReceived)
            {
                
                PointCloudRendering();
                isMessageReceived = false;
            }


        }

        protected override void ReceiveMessage(PointCloud2 message)
        {

            if(isMessageReceived!=true){
                size_byte = message.data.GetLength(0);
                byteArray = new byte[size_byte];
            byteArray = message.data;


            width = (int)message.width;
            height = (int)message.height;
            row_step = (int)message.row_step;
            point_step = (int)message.point_step;

            size = size_byte / point_step;
            isMessageReceived = true;
            }
        }

        //点群の座標を変換
        void PointCloudRendering()
        {
            
            pcl = new Vector3[size];
            pcl_color = new Color[size];

            int x_posi;
            int y_posi;
            int z_posi;

            float x;
            float y;
            float z;
            float position_max=100.0f;
            float position_min=-100.0f;

            int rgb_posi;
            int rgb_max = 255;

            float r;
            float g;
            float b;

            //この部分でbyte型をfloatに変換         
            for (int n = 0; n < size; n++)
            {
                x_posi = n * point_step + 0;
                y_posi = n * point_step + 4;
                z_posi = n * point_step + 8;

                x = BitConverter.ToSingle(byteArray, x_posi); 
                y = BitConverter.ToSingle(byteArray, y_posi);
                z = BitConverter.ToSingle(byteArray, z_posi);

                if(x>position_max||y>position_max||z>position_max)
                    continue;
                else if(x<position_min ||y<position_min||z<position_min)
                    continue;

                rgb_posi = n * point_step + 16;
                b = byteArray[rgb_posi + 0];
                g = byteArray[rgb_posi + 1];
                r = byteArray[rgb_posi + 2];

                /*
                Debug.Log("r="+r);
                Debug.Log("g="+g);
                Debug.Log("b="+b);
                */
                r = r / rgb_max;
                g = g / rgb_max;
                b = b / rgb_max;

                
                //Debug.Log("Num: "+n);
                //Debug.Log("Pcl Num : "+pcl.GetLength(0));
                pcl[n] = new Vector3(x, z, y);
                pcl_color[n] = new Color(r, g, b);
                //Debug.Log("pcl["+n+"].x: "+pcl[n].x);
                //Debug.Log("pcl[n].y: "+pcl[n].y);
                //Debug.Log("pcl[n].z: "+pcl[n].z);

            }
        }

        public Vector3[] GetPCL()
        {
            return pcl;
        }

        public Color[] GetPCLColor()
        {
            return pcl_color;
        }
    }
}
