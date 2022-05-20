using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient.MessageTypes.Sensor;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System;

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]

public class PointCloudSubscriber_big : UnitySubscriber<MessageTypes.Sensor.PointCloud2>
{
    // Start is called before the first frame update
        private byte[] byteArray;
        private bool isMessageReceived = false;
        //bool readyToProcessMessage = true;
        private int size_byte;
        private int size;


        private Vector3[] pcl;
        private Color[] pcl_color;

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

        protected async override void ReceiveMessage(PointCloud2 message)
        {
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
            float position_max=10.0f;
            float position_min=-10.0f;

            int rgb_posi;
            int rgb_max = 255;

            float r;
            float g;
            float b;

                   
            for (int n = 0; n < size; n++)
            {
                x_posi = n * point_step + 0;
                y_posi = n * point_step + 4;
                z_posi = n * point_step + 8;

                
                x = BitConverter.ToSingle(byteArray, x_posi);
                y = BitConverter.ToSingle(byteArray, y_posi);
                z = BitConverter.ToSingle(byteArray, z_posi);
                
                /*
                if(x>position_max)
                    x=position_max;
                if(y>position_max)
                    y=position_max;
                if(z>position_max)
                    z=position_max;

                if(x<position_min)
                    x=position_min;
                if(y<position_min)
                    y=position_min;
                if(z<position_min)
                    z=position_min;
                */

            
                if(x>position_max||y>position_max||z>position_max)
                    continue;
                else if(x<position_min ||y<position_min||z<position_min)
                    continue;
                

                rgb_posi = n * point_step + 12;
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
