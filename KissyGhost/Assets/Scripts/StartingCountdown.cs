using UnityEngine;
using UnityEngine.UI;

public class StartingCountdown : MonoBehaviour 
{
    public HeartZoomTransition _HeartZoomTransition;
    public Text countdown;
    public PauseGame pauseGame;
    public bool CatMode = false;
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
                if (!CatMode)
                {
                    countdown.text = ((int)timer).ToString();
                }
                else
                {
                    switch (((int)timer))
                    {
                        case 3:
                            countdown.text = "M";
                            break;
                        case 2:
                            countdown.text = "E";
                            break;
                        case 1:
                            countdown.text = "O";
                            break;
                        default:
                            countdown.text = "W";
                            break;
                    }
                }

                soundManager.SOUND_MAN.playSound("Play_Round_Start", gameObject);
            }
        }
        else
        {
            Time.timeScale = 1;
            if (!CatMode)
            {
                countdown.text = "Go!";
            }
            else
            {
                countdown.text = "Meow!";
            }
            hasPlayedStartSound = true;
            gameObject.GetComponent<Image>().CrossFadeAlpha(0, .5f, false);
            countdown.GetComponent<Text>().CrossFadeAlpha(0, .5f, false);
            pauseGame.enabled = true;
            this.enabled = false;
        }
	}
}
