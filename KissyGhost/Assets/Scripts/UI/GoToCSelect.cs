using UnityEngine;
using System.Collections;

public class GoToCSelect : MonoBehaviour {

	void Update () {

        if (Input.GetKey("joystick button 0"))
        {
            Application.LoadLevel(2);
        }
	
	}
}
