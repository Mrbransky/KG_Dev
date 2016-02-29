using UnityEngine;
using System.Collections;

public class GoToCSelect : MonoBehaviour
{
    public HeartZoomTransition _HeartZoomTransition;

    void Awake()
    {
        Time.timeScale = 1;
    }

	void Update ()
    {
        if (_HeartZoomTransition.enabled)
        {
            return;
        }

        if (Input.GetKeyDown("joystick button 0"))
        {
            _HeartZoomTransition.enabled = true;
            _HeartZoomTransition.StartHeartZoomIn(2);
            soundManager.SOUND_MAN.playSound("Play_MenuConfirm", gameObject);
        }
#if UNITY_EDITOR
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            _HeartZoomTransition.enabled = true;
            _HeartZoomTransition.StartHeartZoomIn(2);
            soundManager.SOUND_MAN.playSound("Play_MenuConfirm", gameObject);
        }
#endif
	}
}
