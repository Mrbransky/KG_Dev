using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
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
                Scene scene = SceneManager.GetActiveScene();
                soundManager.SOUND_MAN.playSound("Play_MenuDown", gameObject);
                heartZoomTrans.enabled = true;
                heartZoomTrans.StartHeartZoomIn(scene.buildIndex - 5);
            }
        }
	}
}
