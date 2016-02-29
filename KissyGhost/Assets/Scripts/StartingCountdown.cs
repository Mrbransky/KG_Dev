using UnityEngine;
using UnityEngine.UI;

public class StartingCountdown : MonoBehaviour 
{
    public HeartZoomTransition _HeartZoomTransition;
    public Text countdown;

    private bool hasPlayedStartSound
    {
        get{return _hasPlayedStartSound;}
        set
        {
            if(value != _hasPlayedStartSound)
                soundManager.SOUND_MAN.playSound("Play_Round_Start_Go", gameObject);

            _hasPlayedStartSound = value;
        }
    }

    private bool _hasPlayedStartSound;

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
            if (countdown.text != ((int)timer).ToString())
            {
                countdown.text = ((int)timer).ToString();
                soundManager.SOUND_MAN.playSound("Play_Round_Start", gameObject);
            }
        }
        else
        {
            Time.timeScale = 1;
            countdown.text = "Go!";
            hasPlayedStartSound = true;
            gameObject.GetComponent<Image>().CrossFadeAlpha(0, .5f, false);
            countdown.GetComponent<Text>().CrossFadeAlpha(0, .5f, false);
        }
	}
}
