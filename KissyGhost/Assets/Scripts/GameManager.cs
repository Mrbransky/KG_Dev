using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class GameManager : MonoBehaviour {

	public GameObject heart_splosion;
	public GameObject heartZoom;
	public bool gameEnd = false;
	public GameObject gameWinner;
    public HeartZoomTransition _HeartZoomTransition;

    public AudioClip[] music;

    public List<GameObject> thingsToTurnOffAtGameEnd;
    public List<GameObject> thingsToTurnOnAtGameEnd;

    public int playerCount = 0;
    public List<GameObject> currentPlayers;
    public GameObject currentGhostPlayer;
    public GameObject ghostPrefab;
    private bool[] isPlayerReadyArray;
    private int ghostPlayerIndex = -1;

    void Start()
    {
        handleCharacterSelectData();
        initializePlayers();
    }

    private void handleCharacterSelectData()
    {
        GameObject characterSelectData = GameObject.FindGameObjectWithTag("CharacterSelectData");

        if (characterSelectData != null)
        {
            CharacterSelectData _CharacterSelectData = characterSelectData.GetComponent<CharacterSelectData>();
            
            isPlayerReadyArray = _CharacterSelectData.IsPlayerReadyArray;
            playerCount = _CharacterSelectData.PlayerCount;
            ghostPlayerIndex = _CharacterSelectData.GhostPlayerIndex;
            Destroy(characterSelectData);
        }
        else
        {
            Debug.LogError("CharacterSelectDemoController: Could not find game object with tag \"CharacterSelectData\"");
            gameObject.SetActive(false);
        }
    }

    private void initializePlayers()
    {
        currentPlayers = new List<GameObject>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        GameObject ghostPlayer = null;

        for (int i = 0; i < players.Length; ++i)
        {
            int playerReadyArrayIndex = players[i].GetComponent<Human>().playerNum - 1;

            if (!isPlayerReadyArray[playerReadyArrayIndex])
            {
                players[i].SetActive(false);
                Camera.main.GetComponent<NewCameraBehavior>().targets.Remove(players[i]);
            }
            else
            {
                if (playerReadyArrayIndex == ghostPlayerIndex)
                {
                    Debug.Log(playerReadyArrayIndex);

                    ghostPlayer = (GameObject)GameObject.Instantiate(ghostPrefab, players[i].transform.position, players[i].transform.rotation);
                    ghostPlayer.GetComponent<Ghost>().playerNum = players[i].GetComponent<Human>().playerNum;
                    ghostPlayer.gameObject.tag = "Ghost";
                    ghostPlayer.GetComponentInChildren<Text>().text = "P" + (ghostPlayerIndex + 1);
                    ghostPlayer.GetComponentInChildren<FadeOnTimeScale1>().timeScale = .45f;
                    Camera.main.gameObject.GetComponent<NewCameraBehavior>().targets.Remove(players[i]);
                    Destroy(players[i]);
                }
                else
                {
                    currentPlayers.Add(players[i]);
                }
            }
        }

        currentPlayers.Add(ghostPlayer);
        currentGhostPlayer = ghostPlayer;
        Camera.main.gameObject.GetComponent<NewCameraBehavior>().targets.Add(ghostPlayer);
    }
    
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        checkIsGameEnd();
	}

    public void OnHumanDead(GameObject obj)
    {
        currentPlayers.Remove(obj);
        --playerCount;

        if (playerCount == 1)
        {
            OnGhostWin();
        }
    }

    public void OnHumansWin()
    {
        gameEnd = true;

        VibrateAllHumans(.75f, 1, 1);

        AudioSource source = this.GetComponent<AudioSource>();
        source.Stop();
        source.clip = music[1];
        source.Play(); 
    }

    public void OnGhostWin()
    {
        gameEnd = true;
        int ghostPlayerNum = currentGhostPlayer.GetComponent<Ghost>().playerNum;

        StartCoroutine(InputMapper.Vibration(ghostPlayerNum, .75f, 1, 1));

        AudioSource source = this.GetComponent<AudioSource>();
        source.Stop();
        source.clip = music[0];
        source.Play(); 
    }

    private void checkIsGameEnd()
    {
        if (gameEnd)
        {
            heartZoom.SetActive(true);
            heartZoom.GetComponent<Animator>().SetBool("gameEndTrig", true);

            if (heartZoom.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                foreach (GameObject i in thingsToTurnOffAtGameEnd)
                {
                    i.SetActive(false);
                }
                foreach (GameObject i in thingsToTurnOnAtGameEnd)
                {
                    i.SetActive(true);
                }
                Time.timeScale = 0;

                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKey("joystick button 1"))
                {
                    Time.timeScale = 1;
                    _HeartZoomTransition.enabled = true;
                    _HeartZoomTransition.StartHeartZoomIn(0);
                }

                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey("joystick button 0"))
                {
                    Time.timeScale = 1;
                    _HeartZoomTransition.enabled = true;
                    _HeartZoomTransition.StartHeartZoomIn(1);
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        CeaseAllVibrations();
    }

    #region Vibration Methods
    void VibrateAllHumans(float timeAmt, float leftMotor, float rightMotor)
    {
        foreach (GameObject obj in currentPlayers)
        {
            if (obj.GetComponent<Human>())
            {
                int playerNum = obj.GetComponent<Human>().playerNum;
                StartCoroutine(InputMapper.Vibration(playerNum, timeAmt, leftMotor, rightMotor));
            }
        }
    }

    void CeaseVibrationHumans()
    {
        foreach(GameObject obj in currentPlayers)
        {
            if (obj.GetComponent<Human>())
            {
                int playerNum = obj.GetComponent<Human>().playerNum;
                GamePad.SetVibration((PlayerIndex)playerNum, 0, 0);
            }
        }
    }

    void CeaseVibrationGhost()
    {
        int ghostPlayerNum = currentGhostPlayer.GetComponent<Ghost>().playerNum;
        GamePad.SetVibration((PlayerIndex)ghostPlayerNum, 0, 0);
    }

    void CeaseAllVibrations()
    {
        for (int i = 0; i < playerCount; i++)
            GamePad.SetVibration((PlayerIndex)i, 0, 0);
    }
    #endregion

    //private void oldWinCondition()
    //{
    //    if (gameEnd == false)
    //    {
    //        Vector3 heartZoomPos = Camera.main.transform.position;
    //        heartZoomPos.z = 0;
    //        heartZoom.transform.position = heartZoomPos;

    //        if (heartZoom.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
    //        {
    //            heartZoom.SetActive(false);
    //        }
    //    }
    //    else
    //    {
    //        heartZoom.SetActive(true);
    //        //heartZoom.transform.position = gameWinner.transform.position;
    //        heartZoom.GetComponent<Animator>().SetBool("gameEndTrig", true);

    //        if (heartZoom.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
    //        {
    //            gameEndPlayAgain.SetActive(true);
    //            gameEndMainMenu.SetActive(true);
    //            Time.timeScale = 0;

    //            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKey("joystick button 1"))
    //            {
    //                Time.timeScale = 1;
    //                Application.LoadLevel(0);
    //            }

    //            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey("joystick button 0"))
    //            {
    //                Time.timeScale = 1;
    //                Application.LoadLevel(1);
    //            }
    //        }
    //    }
    //}
}
