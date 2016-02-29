using UnityEngine;
using System.Collections;

public class ScrollCredits : MonoBehaviour {

    public float scrollSpeed;
    float startingYposition;
    float Yposition;

    void Awake()
    {
        startingYposition = GetComponent<RectTransform>().localPosition.y;
        this.GetComponent<RectTransform>().localPosition = new Vector3(0, startingYposition, 0);
    }
	
	void Update () 
    {
        Yposition += scrollSpeed * Time.deltaTime;
        this.GetComponent<RectTransform>().localPosition = new Vector3(0, startingYposition + Yposition, 0);

        if (GetComponent<RectTransform>().localPosition.y >= 1950)
        {
            Yposition = 0;
            GetComponent<RectTransform>().localPosition = new Vector3(0, startingYposition, 0);
        }
	}
}
