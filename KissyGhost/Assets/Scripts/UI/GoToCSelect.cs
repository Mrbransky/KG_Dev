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

        if (Input.GetKey("joystick button 0"))
        {
            _HeartZoomTransition.enabled = true;
            _HeartZoomTransition.StartHeartZoomIn(2);
        }
	
	}
}
