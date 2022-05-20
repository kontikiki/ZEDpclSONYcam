using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace UnityTutorial
{
    public class TCPSendPipe : MonoBehaviour
    {
        public Thread clientReceiveThread;
        public String Host = "localhost";
        public Int32 Port = 8092;

        TcpClient mySocket = null;
        NetworkStream theStream = null;
        StreamWriter theWriter = null;

        float roll;
        float pitch;
        float yaw;

        // Start is called before the first frame update
        void Start() {
            ConnectToTcpServer();
        }

        // Update is called once per frame
        void Update()
        {
            SendMessage();
        }

        private void ConnectToTcpServer() {
            try {
                clientReceiveThread = new Thread (new ThreadStart(SetupSocket));
                clientReceiveThread.IsBackground = true;
                clientReceiveThread.Start();
            }
            catch (Exception e) {
                Debug.Log("On client connect exception " + e); 		
            }
        }

        public void SetupSocket()
        {
            mySocket = new TcpClient();
            try
            {
                mySocket.Connect(Host, Port);
                theStream = mySocket.GetStream();
                theWriter = new StreamWriter(theStream);
                Byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes("yah!! it works@");
                mySocket.GetStream().Write(sendBytes, 0, sendBytes.Length);
                Debug.Log("socket is sent");
                // return true;
            }
            catch (Exception e)
            {
                Debug.Log("Socket error: " + e);
                // return false;
            }
        }

        public void SendMessage() {
            // if (mySocket == null) {
            //     return;
            // }
            Quaternion q = transform.rotation;
            Vector3 v = q.ToEulerAngles();
            String roll = (v.x * Mathf.Rad2Deg).ToString("0.00");
            String pitch = (v.y * Mathf.Rad2Deg).ToString("0.00");
            String yaw = (v.z * Mathf.Rad2Deg).ToString("0.00");

            String send_msg = roll + "," + pitch + "," + yaw+"@";

            try {
                theStream = mySocket.GetStream();
                theWriter = new StreamWriter(theStream);
                Byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(send_msg);
                mySocket.GetStream().Write(sendBytes, 0, sendBytes.Length);
                // Debug.Log(send_msg);
                Debug.Log(q);
                // return true;
            }
            catch (Exception e) {
                Debug.Log("Send Message error: " + e);
                // return false;
            }
        }

        private void OnApplicationQuit()
        {
            if (mySocket != null && mySocket.Connected)
                mySocket.Close();
        }
    }
}