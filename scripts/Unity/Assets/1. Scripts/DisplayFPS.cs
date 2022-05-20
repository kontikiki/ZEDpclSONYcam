using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayFPS : MonoBehaviour
{
    [SerializeField]
        Text text;

        float frames=0f;
        float timeElap=0f;
        float frametime=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        frames++;
        timeElap+=Time.unscaledDeltaTime;
        if(timeElap>1f){
            frametime=timeElap/(float)frames;
            timeElap-=1f;
            //UpdateText();
             text.text=string.Format(
            "FPS:{0}, FrameTime : {1:F2} ms",
            frames, frametime * 1000.0f);
            frames=0;
        }
    }

    void UpdateText(){
       
        
    }
}
