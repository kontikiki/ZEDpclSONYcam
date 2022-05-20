using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Runtime.InteropServices;
using System.IO;
using static System.IO.File;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
//using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading.Tasks;

public class LeftThread : MonoBehaviour
{
    public int BUFF_SIZE;
   //public int BUFF_SIZE = 1555200;    //960*540
    //public int BUFF_SIZE = 3110400;
    //public int BUFF_SIZE=2764800;   //1280*720
   // public int BUFF_SIZE=6220800; //1920*1080

    //public int BUFF_SIZE=7188912; //2064*1161
    public int imgWidth = 960;
    public int imgHeight = 540;

     Byte[] imagebyte=null;
    public int m_Port = 8000;
    private List<TcpClient> m_Clients = new List<TcpClient>(new TcpClient[0]);
    private Thread m_ThrdtcpListener=null;
    TcpListener m_TcpListener=null;
    TcpClient m_Client=null;
    private bool image_tri = false;

    //public GameObject sphere;
   public Material material;
 //public RenderTexture img;

    static public string printbytearray(byte[] bytes){
        return string.Join(", ", bytes);
    }  
    
public void ByteArrayToImage(byte[] bytes)
    {
        Texture2D _img_=new Texture2D(imgWidth, imgHeight, TextureFormat.RGB24, false);
        _img_.LoadRawTextureData(bytes);
        _img_.Apply();
        Texture img_ = _img_;//width, height 
        RenderTexture img= new RenderTexture(imgWidth, imgHeight, 24);
        
        Graphics.Blit(img_,img);

        RenderTexture.active = img;
        material.mainTexture=img;
      
        Debug.Log("Write to left material image");
       
    }
    void Start()
    {
        BUFF_SIZE = imgWidth * imgHeight * 3;

        
        Debug.Log("Server left start");
        m_ThrdtcpListener = new Thread(new ThreadStart(ListenForIncommingRequests));
        m_ThrdtcpListener.IsBackground = false;
        m_ThrdtcpListener.Start();
    }

    void Update()   
    {
        if(image_tri == true){
            
            ByteArrayToImage(imagebyte);
           
            Debug.Log("left Image");
            image_tri=false;
            
        }
         Resources.UnloadUnusedAssets();
       
    }

    void OnApplicationQuit()
    {
        m_ThrdtcpListener.Abort();

        if (m_TcpListener != null)
        {
            m_TcpListener.Stop();
            m_TcpListener = null;
        }
    }

    void ListenForIncommingRequests()
    {
        m_TcpListener = new TcpListener(IPAddress.Any, m_Port);
        m_TcpListener.Start();
        ThreadPool.QueueUserWorkItem(ListenerWorker, null);
    }

    void ListenerWorker(object token)
    {
        while (m_TcpListener != null)
        {
            m_Client = m_TcpListener.AcceptTcpClient();
            m_Clients.Add(m_Client);
            ThreadPool.QueueUserWorkItem(HandleClientWorker, m_Client);
        }
    }
    void HandleClientWorker(object token)
    {
        using (var client = token as TcpClient)
        using (var stream = client.GetStream())
        {
            
            int length = 0;
            int total = 0;
            
            int t=0;
            int sub=0;
            
            Byte[] bytes= new Byte[BUFF_SIZE];
            Byte[] subBytes=new Byte[BUFF_SIZE];
           imagebyte = new Byte[BUFF_SIZE];
            while((length = stream.Read(bytes,sub,bytes.Length-sub))!=0)
           // while((length = stream.Read(bytes,0,bytes.Length))!=0)
            {
                
                sub=0;
                if((t=total+length)>=BUFF_SIZE)
                {
                    sub=t-BUFF_SIZE;
                    Array.Copy(bytes,length-sub,subBytes,0,sub);
                    length=length-sub;
                    Debug.Log("left sub : ");
                    Debug.Log(sub);
                }
                
               Array.Copy(bytes, 0, imagebyte, total, length);
               total += length;

                 if(total == BUFF_SIZE){
                   Array.Copy(subBytes,0, bytes, 0, sub);
                    total=sub;
                    image_tri = true;
                    Debug.Log("right image_tri set TRUE.");                 
                }
            }
            
            if (m_Client == null)
            {
                return;
            }
             OnApplicationQuit();
             Debug.Log("left thread quit");
        }
    }
}
