using UnityEngine;
using System.Collections;

public class GoBack : MonoBehaviour {
	
	
	void Update () {
	
		if (Input.GetKeyDown (KeyCode.Escape) || Input.GetKey("joystick button 1")) {
			Application.LoadLevel(0);
		}

	}
}
