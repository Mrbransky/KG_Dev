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
            if (value != _hasPlayedStartSound)
            {
                soundManager.SOUND_MAN.playSound("Play_Round_Start_Go", gameObject);

                if (CatMode)
                {
                    soundManager.SOUND_MAN.playSound("Play_cat", gameObject);
                }
            }

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
            if (countdown.text != ((int)timer).ToString() && !CatMode)
            {
                countdown.text = ((int)timer).ToString();

                soundManager.SOUND_MAN.playSound("Play_Round_Start", gameObject);
            }
			else if (CatMode)
			{
				switch (((int)timer))
				{
				case 3:
					if (countdown.text != "M")
					{
						countdown.text = "M";
						soundManager.SOUND_MAN.playSound("Play_Round_Start", gameObject);

                        if (CatMode)
                        {
                            soundManager.SOUND_MAN.playSound("Play_cat", gameObject);
                        }
					}
					break;
				case 2:
					if (countdown.text != "E")
					{
						countdown.text = "E";
						soundManager.SOUND_MAN.playSound("Play_Round_Start", gameObject);

                        if (CatMode)
                        {
                            soundManager.SOUND_MAN.playSound("Play_cat", gameObject);
                        }
					}
					break;
				case 1:
					if (countdown.text != "OW?")
					{
						countdown.text = "OW?";
						soundManager.SOUND_MAN.playSound("Play_Round_Start", gameObject);

                        if (CatMode)
                        {
                            soundManager.SOUND_MAN.playSound("Play_cat", gameObject);
                        }
					}
					break;
				default:
					if (countdown.text != "W")
					{
						countdown.text = "W";
						soundManager.SOUND_MAN.playSound("Play_Round_Start", gameObject);

                        if (CatMode)
                        {
                            soundManager.SOUND_MAN.playSound("Play_cat", gameObject);
                        }
					}
					break;
				}
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
                if (CatMode)
                {
                    soundManager.SOUND_MAN.playSound("Play_cat", gameObject);
                }
            }
            hasPlayedStartSound = true;
            gameObject.GetComponent<Image>().CrossFadeAlpha(0, .5f, false);
            countdown.GetComponent<Text>().CrossFadeAlpha(0, .5f, false);
            pauseGame.enabled = true;
            this.enabled = false;
        }
	}
}
