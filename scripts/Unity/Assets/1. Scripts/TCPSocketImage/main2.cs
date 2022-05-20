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

public class main2 : MonoBehaviour
{
    //public int BUFF_SIZE = 1555200;
    public int BUFF_SIZE = 3110400;
    //public int BUFF_SIZE;
    public int imgWidth = 1920;
    public int imgHeight = 540;
    public int m_Port = 8000;

    private TcpListener m_TcpListener;
    private List<TcpClient> m_Clients = new List<TcpClient>(new TcpClient[0]);
    private Thread m_ThrdtcpListener;
    private TcpClient m_Client;
    
    Byte[] imagebyte;
    
    private bool image_tri = false;

    //RawImage m_RawImage;
    //Select a Texture in the Inspector to change to

    public RawImage m_RawImage;
    public RawImage m_RawImage2;
    
    private Texture m_Texture;

    //public RenderTexture RenderTexture;
    //public Texture2D updateimage;
    //private Color32[] pixel32;
    //private GCHandle pixelHandle;
    //private IntPtr pixelPtr;

    static public string printbytearray(byte[] bytes){
        return string.Join(", ", bytes);
    }
   
    public Texture2D ByteArrayToImage(byte[] bytes)
    {
        //Debug.Log(printbytearray(bytes));
        Debug.Log("bytearraytoimage  " + bytes.Length);
        //Texture2D img_ = new Texture2D(960, 540, TextureFormat.RGB24, false); //width, height 
        Texture2D img_ = new Texture2D(imgWidth, imgHeight, TextureFormat.RGB24, false); //width, height 

        //img_.SetPixels32(pixel32);
        //Texture2D img_ = new Texture2D(960, 540, TextureFormat.BGRA32, false); //width, height 
        
        img_.LoadRawTextureData(bytes);
        img_.Apply();
        return img_;
    }

    void Start()
    {
        BUFF_SIZE = imgWidth * imgHeight * 3;

        //value init 
        imagebyte = new byte[BUFF_SIZE];
        
        //server
        Debug.Log("Server start");
        m_ThrdtcpListener = new Thread(new ThreadStart(ListenForIncommingRequests));
        m_ThrdtcpListener.IsBackground = true;
        m_ThrdtcpListener.Start();

        // showing defalut image 
       //m_RawImage = GetComponent<RawImage>();
      
        m_Texture = Resources.Load<Texture2D>("ski");
        m_RawImage.texture = m_Texture;
        m_RawImage2.texture=m_Texture;

    }

    void Update()
    {
        ///// 
        // for (int i = 0; i < m_Clients.Count; i++)
        // {
        //     if (!m_Clients[i].Connected)
        //         m_Clients.RemoveAt(i);

        //     else
        //         SendMessage(m_Clients[i], "--> "); //
        // }

        if(image_tri == true){
            m_Texture = ByteArrayToImage(imagebyte);
            m_RawImage.texture = m_Texture;
            m_RawImage2.texture=m_Texture;
            Debug.Log("get one Image");
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
           //imagebyte = new Byte[BUFF_SIZE];

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

 void SendMessage(object token, string message)
    {
        if (m_Client == null)
            return;

        //else
            //Debug.Log(m_Clients.Count);

        var client = token as TcpClient;
        {
            try
            {
                NetworkStream stream = client.GetStream();
                if (stream.CanWrite)
                {
                    byte[] serverMessageAsByteArray = Encoding.Default.GetBytes(message);
                    stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
                }
            }

            catch (SocketException ex)
            {
                Debug.Log(ex);
                return;
            }
        }
    }
    
}