using UnityEngine;
using System.Collections;

public class CreditInput : MonoBehaviour 
{
    public HeartZoomTransition heartZoomTrans;

	void Update () 
    {
	    if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey("joystick button 1"))
        {
            if (heartZoomTrans.enabled == false)
            {
                soundManager.SOUND_MAN.playSound("Play_MenuDown", gameObject);
                heartZoomTrans.enabled = true;
                heartZoomTrans.StartHeartZoomIn(Application.loadedLevel - 5);
            }
        }
	}
}
