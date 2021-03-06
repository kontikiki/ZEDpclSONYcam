using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamButton : MonoBehaviour
{
    int currentCamIndex = 0;

    WebCamTexture tex;
    public RawImage display;
    public Text startStopText;
    public void SwapCam_clicked()
    {
        if (WebCamTexture.devices.Length > 0)
        {
            currentCamIndex += 1;
            currentCamIndex %= WebCamTexture.devices.Length;
            // If tex is not null:
            // stop the webcam
            // start the webcam
            if (tex != null)
            {
                StopWebcam();
                StartStopCam_Clicked();
            }
        }
    }
    public void StartStopCam_Clicked()
    {
        if (tex != null) // Stop the Camera
        {
            StopWebcam();
            startStopText.text = "Start Camera";
        }
        else // Start the Camera
        {
            WebCamDevice device = WebCamTexture.devices[currentCamIndex];
            tex = new WebCamTexture(device.name);
            display.texture = tex;
            tex.Play();
            startStopText.text = "Stop Camera";
        }
    }
    private void StopWebcam()
    {
        display.texture = null;
        tex.Stop();
        tex = null;
    }
}