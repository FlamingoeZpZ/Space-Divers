using UnityEngine;
using System.Collections;
 
//https://forum.unity.com/threads/how-can-i-display-fps-on-android-device.386250/
public class MobileUtilsScript : MonoBehaviour {
 
    private int framesPerSec;
    private readonly float frequency = 1.0f;
    private string fps;
 
 
 
    void Start(){
        StartCoroutine(FPS());
    }
 
    private IEnumerator FPS() {
        for(;;){
            // Capture frame-per-second
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;
           
            // Display it
 
            fps = $"FPS: {Mathf.RoundToInt(frameCount / timeSpan)}";
        }
    }
 
 
    void OnGUI(){
        GUI.Label(new Rect(Screen.width - 100,10,150,20), fps);
    }
}