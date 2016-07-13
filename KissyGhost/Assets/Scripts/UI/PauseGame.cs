using UnityEngine;
using System.Collections;
public class PauseGame : MonoBehaviour {

    bool isPaused = false;
    public GameObject pauseScreen;
	void Update () {


        if (Input.GetKeyDown("joystick button 7"))
        {
            if (isPaused == false)
            {
                pauseScreen.SetActive(true);
                Time.timeScale = 0;
                isPaused = true;
            }
            else
            {
                pauseScreen.SetActive(false);
                Time.timeScale = 1;
                isPaused = false;
            }
        }

	}
}
