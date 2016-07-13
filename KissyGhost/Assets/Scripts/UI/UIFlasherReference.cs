using UnityEngine;
using System.Collections;

public class UIFlasherReference : MonoBehaviour
{
    private float fadeDuration = 3.0f;
    private float timeSinceFade = 0;
    private bool isTransparent = false;

    public float FadeDuration
    {
        get { return fadeDuration; }
    }

    public float TimeSinceFade
    {
        get { return timeSinceFade; }
    }

    public bool IsTransparent
    {
        get { return isTransparent; }
    }

    public float TimeDelta
    {
        get { return (timeSinceFade / fadeDuration); }
    }
    
	void Update ()
    {
        timeSinceFade += Time.deltaTime;

        if (timeSinceFade > FadeDuration)
        {
            isTransparent = !isTransparent;
            timeSinceFade = 0;
        }
    }
}
