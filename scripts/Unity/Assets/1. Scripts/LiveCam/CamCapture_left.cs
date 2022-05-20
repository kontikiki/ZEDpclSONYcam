using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamCapture_left : MonoBehaviour
{
    int currentCamIndex = 0;

    public WebCamTexture tex;
    public RawImage display;
   public RawImage dis2;
    public Text startStopText;
    
    public void SwapCam_clicked()
    {
        if (WebCamTexture.devices.Length > 0)
        {
    
        WebCamDevice[] devices = WebCamTexture.devices;
        for( int i = 0 ; i < devices.Length ; i++ )
            Debug.Log(devices[i].name);

            // If tex is not null:
            // stop the webcam
            // start the webcam

            if (tex != null) // Stop the Camera
        {
            StopWebcam();
            startStopText.text ="camera stop";
            
        }
        else // Start the Camera
        {
            startStopText.text ="camera start";
        }
        StartCam();
            
        }
    }

    void StartCam()
    {
            
        WebCamDevice device = WebCamTexture.devices[currentCamIndex];
        Debug.Log("Device name: "+device.name);
            tex = new WebCamTexture(device.name,3840,2160);
            
            startStopText.text=string.Format("webcam texture pixel height: {0:F2}, width: {1:F2}",tex.requestedHeight,tex.requestedWidth);
            //tex.requestedHeight=3840;
            //tex.requestedWidth=2160;
            display.texture = tex;
            dis2.texture=tex;
            if(!tex.videoVerticallyMirrored)
            {
                tex.Play();
            }
    }


    private void StopWebcam()
    {
        display.texture = null;
        dis2.texture=null;
        tex.Stop();
        tex = null;
    }

    void Start()
    {
        
        SwapCam_clicked();
        
    }

    void Update()
    {
           
    }
}