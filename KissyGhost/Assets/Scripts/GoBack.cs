﻿using UnityEngine;
using System.Collections;

public class GoBack : MonoBehaviour {

    public HeartZoomTransition _HeartZoomTransition;

    public int sceneToGoBackToo = 0;
	void Update ()
    {
	    if (_HeartZoomTransition.enabled)
        {
            return;
        }

		if (Input.GetKeyDown (KeyCode.Space) || Input.GetKey("joystick button 1"))
        {
            _HeartZoomTransition.enabled = true;
            _HeartZoomTransition.StartHeartZoomIn(sceneToGoBackToo);
        }

#if !UNITY_EDITOR && !UNITY_WEBGL && !UNITY_WEBPLAYER
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _HeartZoomTransition.enabled = true;
            _HeartZoomTransition.StartHeartZoomIn(-1);
        }
#endif
    }
}
