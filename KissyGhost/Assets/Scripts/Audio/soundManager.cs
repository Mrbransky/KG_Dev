using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class soundManager : MonoBehaviour {
    static public soundManager SOUND_MAN;

	AkEvent theEvent;

    public bool MainMenuMusicPlaying;
    public bool GameHasEnded;
    public bool CatMode = false;

	// Use this for initialization

    void Awake()
    {
        if (SOUND_MAN == null)
            SOUND_MAN = this;

        else if (SOUND_MAN != this)
            Destroy(gameObject);
    }

	void Start () 
    {
        GameHasEnded = false;

		AkBankManager.LoadBank ("KissyGhostBank");
        if (!SOUND_MAN.MainMenuMusicPlaying)
        {
            playSound("Play_Music", gameObject);
            SOUND_MAN.MainMenuMusicPlaying = true;
        }
		/*uint busID;
		busID = AkSoundEngine.GetIDFromString ("toneBusParameter");
		AkSoundEngine.SetMixer ("toneBusParameter", busID);*/

        DontDestroyOnLoad(this.gameObject);
	}

	void Update () 
    {
        Scene scene = SceneManager.GetActiveScene();

#if UNITY_EDITOR || UNITY_WEBGL //|| UNITY_STANDALONE
        if (Input.GetKeyDown ("p")) {
			AkSoundEngine.StopAll ();
		}
#endif

        if ((scene.name == "MainScene" || scene.name == "MainScene_Cat") && !GameHasEnded)
        {
            switchVoid("MusicSwitch", "GameplayMusic", gameObject);
            MainMenuMusicPlaying = false;
        }

        if (scene.name == "MainMenu" || scene.name == "Instructions" && !MainMenuMusicPlaying)
        {
            switchVoid("MusicSwitch", "MenuMusic", gameObject);
            GameHasEnded = false;
            MainMenuMusicPlaying = true;
        }

        if (scene.name == "Animatic" && !GameHasEnded)
		{
			switchVoid("MusicSwitch", "Animatic", gameObject);
			MainMenuMusicPlaying = false;
		}

        //if (GameHasEnded && !WinMusicPlaying)
        //{
        //    if(HumansWonGame)
        //        switchVoid("MusicSwitch", "HumanWinMusic", gameObject);
        //    else
        //        switchVoid("MusicSwitch", "GhostWinMusic", gameObject);

        //    WinMusicPlaying = true;
        //}
	}

	public void playSound(string eventName, GameObject soundObject){
		AkSoundEngine.PostEvent (eventName, soundObject);
		/*AkSoundEngine.SetRTPCValue ("pitchParameter", pitchValue, soundObject);
		AkSoundEngine.SetRTPCValue ("flatSharpParameter", pitchFlatten, soundObject);*/

        if (CatMode && eventName != "Play_cat")
        {
            AkSoundEngine.PostEvent("Play_cat", soundObject);
        }
	}
	
	public void stopSound(string eventName, GameObject soundObject, int fadeOut){
		uint eventID;
		eventID = AkSoundEngine.GetIDFromString (eventName);
		AkSoundEngine.ExecuteActionOnEvent (eventID, AkActionOnEventType.AkActionOnEventType_Stop, gameObject, fadeOut * 1000, AkCurveInterpolation.AkCurveInterpolation_Sine);
	}

	public void switchVoid(string switchGroup, string switchState, GameObject colObject){
		AkSoundEngine.SetSwitch (switchGroup, switchState, colObject);
	}

    public void PlayHumanWinMusic()
    {
        GameHasEnded = true;
        switchVoid("MusicSwitch", "HumanWinMusic", gameObject);
    }

    public void PlayGhostWinMusic()
    {
        GameHasEnded = true;
        switchVoid("MusicSwitch", "GhostWinMusic", gameObject);
    }

	public void attenParamSetUp(GameObject otherObj, string parameter){
		float posX = gameObject.transform.position.x - otherObj.transform.position.x;
		float posY = gameObject.transform.position.y - otherObj.transform.position.y;
		float displacement = Mathf.Sqrt(Mathf.Pow(posX, 2) + Mathf.Pow(posY, 2));
		
		AkSoundEngine.SetRTPCValue (parameter, displacement, otherObj);
		print (displacement);
	}
}
