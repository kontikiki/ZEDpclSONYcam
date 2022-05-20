#define DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEngine.Object;
using UnityEngine.UI;
using Unity.Collections;

using System;
using System.Runtime.InteropServices;
using System.IO;
using static System.IO.File;
using System.Text;
using System.Threading;
//using System.Drawing;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;


[ExecuteInEditMode]
public class CameraDepthOn : MonoBehaviour
{

//    public int resWidth;
//    public int resHeight;
    public Material material;
    public RenderTexture renderTexture;

#if DEBUG
    /* for debugging  */
    public Material material2;
    public Material material3;
    public RenderTexture renderTexture2;
#endif

    /* for saving  */
    Color color;


    string fullPath=""; 
    string folderPath="Assets/rawdata/"+DateTime.Now.ToString("yyyyMMdd")+"/";
   string formatPath=".txt";
   
    string pngFullPath="";    
    string pngFolderPath="Assets/png/"+DateTime.Now.ToString("yyyyMMdd")+"/";
    string pngFormatPath=".png";

     string depthFilePath="Depth";
    string colorFilePath="Color";

    int depthFlag=0;
    static int depth_cnt=0;
    static int color_cnt=0;

    Texture2D ToTexture2D (RenderTexture rTex) { 
        Texture2D tex = new Texture2D (rTex.width, rTex.height, TextureFormat.RGBA32, false); 
        RenderTexture.active = rTex;
        tex.ReadPixels (new Rect (0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex; 
        }


    public void SaveImageData(RenderTexture renderTexture)
    {  
        /* copy pixel from render texture to texture2D */
        Texture2D originTex=new Texture2D(renderTexture.width,renderTexture.height,TextureFormat.RGBA32,false);
        
        originTex=ToTexture2D(renderTexture);
        
        /* getting byte array for png image */
        
        //NativeArray<byte> imageBytes = new NativeArray<byte>(originTex.GetRawTextureData(), Allocator.Temp);
        //var bytes = ImageConversion.EncodeNativeArrayToPNG(imageBytes, originTex.graphicsFormat, (uint)originTex.width, (uint)originTex.height);
        byte[] imageBytes=originTex.GetRawTextureData();
        byte[] bytes=ImageConversion.EncodeArrayToPNG(imageBytes, originTex.graphicsFormat, (uint)originTex.width, (uint)originTex.height);
       
     BinaryWriter writer;

        if(depthFlag==1){

            fullPath=folderPath+depthFilePath+depth_cnt.ToString()+formatPath;

            while((new FileInfo(fullPath)).Exists)
            {
                depth_cnt++;
                fullPath=folderPath+depthFilePath+depth_cnt.ToString()+formatPath;
                
            } 
            pngFullPath=pngFolderPath+depthFilePath+depth_cnt.ToString()+pngFormatPath;
        }else{

            fullPath=folderPath+colorFilePath+color_cnt.ToString()+formatPath;

            while((new FileInfo(fullPath)).Exists)
            {
                color_cnt++;
                fullPath=folderPath+colorFilePath+color_cnt.ToString()+formatPath;
                
            }
            pngFullPath=pngFolderPath+colorFilePath+color_cnt.ToString()+pngFormatPath;
        }

        writer = new BinaryWriter(File.Open(fullPath, FileMode.Create));
        writer.Write(imageBytes);
        writer.Close();
        File.WriteAllBytes(pngFullPath, bytes);

 //these codes below are for debugging... (draw image from binary data)
#if DEBUG
        Texture2D copyTex=new Texture2D(renderTexture.width,renderTexture.height,TextureFormat.RGBA32,false);
        FileInfo fileInfo=new FileInfo(fullPath);
        byte[] value=new byte[imageBytes.Length];
        if(fileInfo.Exists)
        {
            Debug.Log("File exists!");
            BinaryReader reader=new BinaryReader(File.Open(fullPath,FileMode.Open));
            reader.Read(value,0,(int)imageBytes.Length);
            reader.Close();
        }      

        copyTex.LoadRawTextureData(value);
        copyTex.Apply();

        if(depthFlag==1)
            material2.mainTexture=copyTex;
        else
            material3.mainTexture=copyTex;
#endif
    }//end of SaveImageData

    void Start(){
    }

    void Update(){
        if(Input.GetKey(KeyCode.C)) // Input.GetKey("space")
        {
            Debug.Log("C key is pushed!");
            depthFlag=1;
            SaveImageData(renderTexture);

            depthFlag=0;
            SaveImageData(renderTexture2);
            
            depthFlag=0;
            
        }
        Resources.UnloadUnusedAssets(); 
    } 


    void OnEnable()
    {
        Camera pCamera = GetComponent<Camera>();
        if (pCamera == null) return;
//            Debug.Log("Depth On");
        pCamera.depthTextureMode = DepthTextureMode.DepthNormals;
        
    }

    void OnDisable()
    {
        Camera pCamera = GetComponent<Camera>();
        if (pCamera == null) return;
//              Debug.Log("Depth Off");
         pCamera.depthTextureMode = DepthTextureMode.None;
        
    }
    
}