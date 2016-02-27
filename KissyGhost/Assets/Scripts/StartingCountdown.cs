using UnityEngine;
using UnityEngine.UI;

public class StartingCountdown : MonoBehaviour 
{
    public HeartZoomTransition _HeartZoomTransition;
    public Text countdown;

    private float timer = 4;
    
    void Awake()
    {
        Time.timeScale = 0;
    }
	
	void Update ()
    {
        if (_HeartZoomTransition.enabled)
        {
            return;
        }

        if ((int)timer > 0)
        {
            timer -= Time.fixedDeltaTime;
            countdown.text = ((int)timer).ToString();
        }
        else
        {
            Time.timeScale = 1;
            countdown.text = "Go!";
            gameObject.GetComponent<Image>().CrossFadeAlpha(0, .5f, false);
            countdown.GetComponent<Text>().CrossFadeAlpha(0, 1.0f, false);
        }
	}
}
