using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class GameManager : MonoBehaviour {

    public GameObject heart_splosion;
    public GameObject heartZoom;
    public bool gameEnd = false;
    public bool didHumansWin;
    public Text winnerNameText;
    public HeartZoomTransition _HeartZoomTransition;
    public SpriteSorter _SpriteSorter;

    public AudioClip[] music;

    public List<GameObject> thingsToTurnOffAtGameEnd;
    public List<GameObject> thingsToTurnOnAtGameEnd;

    public int playerCount = 0;
    public List<GameObject> currentPlayers;
    public GameObject currentGhostPlayer;
    public GameObject ghostPrefab;

    private bool[] isPlayerReadyArray;
    private int ghostPlayerIndex = -1;

    private float timer = 2;

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

        if (_SpriteSorter != null)
        {
            foreach (GameObject playerObject in currentPlayers)
            {
                _SpriteSorter.AddToAllLists(playerObject.GetComponent<SpriteRenderer>());
            }
        }

        currentPlayers.Add(ghostPlayer);
        currentGhostPlayer = ghostPlayer;
        Camera.main.gameObject.GetComponent<NewCameraBehavior>().targets.Add(ghostPlayer);
    }

    void Update()
    {
        checkIsGameEnd();
        
#if !UNITY_EDITOR && !UNITY_WEBGL && !UNITY_WEBPLAYER
        if (Input.GetKeyDown(KeyCode.Escape) && !_HeartZoomTransition.enabled)
        {
            _HeartZoomTransition.enabled = true;
            _HeartZoomTransition.StartHeartZoomIn(-1);
        }
#endif
    }

    public void OnHumanDead(GameObject obj, SpriteRenderer sr)
    {
        _SpriteSorter.RemoveFromAllLists(sr);

        currentPlayers.Remove(obj);
        --playerCount;

        if (playerCount == 1)
        {
            OnGhostWin();
        }
    }

    public void OnHumansWin()
    {
        didHumansWin = true;
        gameEnd = true;

        VibrateAllHumans(.75f, 1, 1);

        AudioSource source = this.GetComponent<AudioSource>();
        source.Stop();
        source.clip = music[1];
        source.Play(); 

        //Humans Win Music
        //soundManager.SOUND_MAN.switchVoid("MusicSwitch", "HumanWinMusic", gameObject);
        soundManager.SOUND_MAN.PlayHumanWinMusic();
        //soundManager.SOUND_MAN.playSound("HumanWinMusic", gameObject);
    }

    public void OnGhostWin()
    {
        didHumansWin = false;
        gameEnd = true;
        int ghostPlayerNum = currentGhostPlayer.GetComponent<Ghost>().playerNum;

        StartCoroutine(InputMapper.Vibration(ghostPlayerNum, .75f, 1, 1));

        AudioSource source = this.GetComponent<AudioSource>();
        source.Stop();
        source.clip = music[0];
        source.Play(); 

        //Ghost Win Music
        //soundManager.SOUND_MAN.switchVoid("MusicSwitch", "GhostWinMusic", gameObject);
        soundManager.SOUND_MAN.PlayGhostWinMusic();
        //soundManager.SOUND_MAN.playSound("GhostWinMusic", gameObject);
    }

    private void checkIsGameEnd()
    {
        if (gameEnd)
        {
            if (didHumansWin)
            {
                GhostPullToMiddle();
                if (Vector2.Distance(currentGhostPlayer.transform.position, GetComponent<RoomGenerator>().MainBaseRoomPiece.transform.position) < 1.6f)
                {
                    timer -= Time.fixedDeltaTime;

                    _HeartZoomTransition.enabled = true;
                    _HeartZoomTransition.StartHeartZoomInHalfway();
                }
            }
            else
            {
                timer = 0;

                _HeartZoomTransition.enabled = true;
                _HeartZoomTransition.StartHeartZoomInHalfway();
            }
            if (_HeartZoomTransition.IsZoomInHalfwayDone())
            {
                if (didHumansWin)
                {
                    winnerNameText.text = "Humans Win!";
                }
                else
                {
                    winnerNameText.text = "Ghost Wins!";
                }
                
                foreach (GameObject i in thingsToTurnOffAtGameEnd)
                {
                    i.SetActive(false);
                }
                
                foreach (GameObject i in thingsToTurnOnAtGameEnd)
                {
                    i.SetActive(true);
                }
                
                Time.timeScale = 0;

                if (timer <= 0)
                {
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey("joystick button 0"))
                    {
                        _HeartZoomTransition.StartHeartZoomInFinish(1);
                    }
                    else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKey("joystick button 1"))
                    {
                        _HeartZoomTransition.StartHeartZoomInFinish(0);
                    }

                    timer = -1;
                }
            }
        }
    }
    void GhostPullToMiddle()
    {
        #if UNITY_EDITOR || UNITY_WEBGL || UNITY_WEBPLAYER //|| UNITY_STANDALONE
        currentGhostPlayer.GetComponent<Ghost>().debugCurrentSpeed = 0;
        #endif

        Vector2 roomPos = GetComponent<RoomGenerator>().MainBaseRoomPiece.transform.position;
        currentGhostPlayer.GetComponent<Ghost>().currentSpeed = 0;
        if (Vector2.Distance(currentGhostPlayer.transform.position, roomPos) > 1.6f)
        {
            Vector2 direction = currentGhostPlayer.transform.position - transform.position;

            currentGhostPlayer.GetComponent<Rigidbody2D>().AddForceAtPosition(-direction.normalized * 3, roomPos);
        }
        else
        {
            currentGhostPlayer.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            currentGhostPlayer.transform.FindChild("Heart_Splosion").gameObject.SetActive(true);
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
}
