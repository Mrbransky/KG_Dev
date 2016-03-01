using UnityEngine;
using System.Collections;

public class GoBack : MonoBehaviour {

    public HeartZoomTransition _HeartZoomTransition;
	
	void Update ()
    {
	    if (_HeartZoomTransition.enabled)
        {
            return;
        }

		if (Input.GetKeyDown (KeyCode.Escape) || Input.GetKey("joystick button 1"))
        {
            _HeartZoomTransition.StartHeartZoomIn(0);
		}

	}
}
