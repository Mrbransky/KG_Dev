using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CloudMovement : MonoBehaviour
{
    Image image;

    public float LeftBound, RightBound;
    public float Speed;

    float Xposition
    {
        get { return image.transform.localPosition.x; }
    }

    float Yposition
    {
        get { return image.transform.localPosition.y; }
    }

    float Zposition
    {
        get { return image.transform.localPosition.z; }
    }

	void Start ()
    {
        image = GetComponent<Image>();
	}
	
	void Update ()
    {
        if (Xposition >= LeftBound)
            image.transform.localPosition += new Vector3(-Speed * 10 * Time.deltaTime, 0, 0);

        else
            image.transform.localPosition = new Vector3(RightBound, Yposition, Zposition);
    }
}
