using UnityEngine;
using UnityEngine.UI;

public class StartingCountdown : MonoBehaviour 
{
    public HeartZoomTransition _HeartZoomTransition;
    public Text countdown;

    private float timer = 4;
    private bool downcounting = false;
    
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

        countdown.enabled = true;
        if(downcounting == false)
        timer -= Time.fixedDeltaTime;

        countdown.text = ((int)timer).ToString();

        if (timer <= 0)
        {
            Time.timeScale = 1;
            this.gameObject.GetComponent<Image>().CrossFadeAlpha(0f, .5f, false);
            countdown.GetComponent<Text>().CrossFadeAlpha(0f, 1.0f, false);
            downcounting = true;
        }
	}
}
