using UnityEngine;
using System.Collections;

public class HideMouse : MonoBehaviour {

    public float timeDelay;
    float Timer;
    bool isNotHidden = true;

    void Start()
    {
        Timer = timeDelay;
    }
	void Update () {

        if (isNotHidden)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                HideTheMouse();
            }
        }

        if (Input.GetAxis("Mouse X")!=0)
        {
            UnhideTheMouse();
        }
        if (Input.GetAxis("Mouse Y")!=0)
        {
            UnhideTheMouse();
        }

    }

    void HideTheMouse()
    {
        Cursor.visible = false;
        isNotHidden = false;
    }
    void UnhideTheMouse()
    {
        Cursor.visible = true;
        Timer = timeDelay;
        isNotHidden = true;
    }
}
