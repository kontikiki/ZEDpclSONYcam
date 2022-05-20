using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamCapture_right : MonoBehaviour
{
    int currentCamIndex = 0;

    public WebCamTexture tex;
    public RawImage display;
    public Text startStopText;
    
    public void SwapCam_clicked()
    {
        if (WebCamTexture.devices.Length > 0)
        {

            WebCamDevice[] devices = WebCamTexture.devices;
        for( int i = 0 ; i < devices.Length ; i++ )
            Debug.Log(devices[i].name);
            //Debug.Log("WebCamTexture.devices.Length:"+WebCamTexture.devices.Length);
           // currentCamIndex += 1;
            //Debug.Log("currentCamIndex:"+currentCamIndex);
           // currentCamIndex %= WebCamTexture.devices.Length;
            
            //Debug.Log("currentCamIndex:"+currentCamIndex);
            
            // If tex is not null:
            // stop the webcam
            // start the webcam

            if (tex != null) // Stop the Camera
        {
            StopWebcam();
            startStopText.text = "Camera stop";
            
        }
        else // Start the Camera
        {
            startStopText.text = "Camera start";
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
            display.texture = tex;
            if(tex.videoVerticallyMirrored)
            {
                tex.Play();
            }
            
    }


    private void StopWebcam()
    {
        display.texture = null;
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