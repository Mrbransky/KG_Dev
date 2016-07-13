using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;
using System.Linq;

public class GameManager : MonoBehaviour {

    public bool CatMode = false;
    public GameObject heart_splosion;
    public GameObject heartZoom;
    public HeartZoomTransition _HeartZoomTransition;
    public SpriteSorter _SpriteSorter;

    public Sprite startingWomanSprite;
    public Sprite startingOldieSprite;

    public AudioClip[] music;

    public Text[] playerNumText;
    public HeartComponentManager[] playerHeartUIManagers;
    
    [Header("End Game")]
    public List<GameObject> thingsToTurnOffAtGameEnd;
    public List<GameObject> thingsToTurnOnAtGameEnd;
    public bool gameEnd = false;
    public bool didHumansWin;
    public Text winnerNameText;
    
    [Header("Player Data")]
    public int playerCount = 0;
    public List<GameObject> currentPlayers;
    public GameObject currentGhostPlayer;
    public GameObject ghostPrefab;
    public GameObject ghostAIPrefab;

    private bool[] isPlayerReadyArray;
    private int ghostPlayerIndex = -1;
    public ColorPalette[] oldieColorPalettes = new ColorPalette[4];
    public ColorPalette[] womanColorPalettes = new ColorPalette[4];

    private ColorPalette[] playerColorPalettes = new ColorPalette[4];
    private Sprite[] startingSprites = new Sprite[4];
    private bool[] isFemaleCharacter = new bool[4];

    private bool SceneStartedInEditor = false;

    private float timer = 2;

    private const string rPath_womanAnimController = "Animations/R_oldWoman_idle_Controller";
    private const string rPath_oldieAnimController = "Animations/R_oldie_animation_controller";

    public bool isGhostAI = false;

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

            if (_CharacterSelectData.gameObject.name.Contains("Debug"))
                SceneStartedInEditor = true;

            isPlayerReadyArray = _CharacterSelectData.IsPlayerReadyArray;
            playerCount = _CharacterSelectData.PlayerCount;
            ghostPlayerIndex = _CharacterSelectData.GhostPlayerIndex;
            playerColorPalettes = _CharacterSelectData.PlayerPaletteArray;
            startingSprites = _CharacterSelectData.StartingSprites;            
            isFemaleCharacter = _CharacterSelectData.IsFemaleCharacter;
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
                    if (!isGhostAI)
                    {
                        ghostPlayer = (GameObject)GameObject.Instantiate(ghostPrefab, players[i].transform.position, players[i].transform.rotation);
                        ghostPlayer.GetComponent<Ghost>().playerNum = players[i].GetComponent<Human>().playerNum;
                        ghostPlayer.gameObject.tag = "Ghost";

                        if (!CatMode)
                        {
                            ghostPlayer.GetComponentInChildren<Text>().text = "P" + (ghostPlayerIndex + 1);
                        }
                        else
                        {
                            ghostPlayer.GetComponentInChildren<Text>().text = "MEOW";
                        }

                        ghostPlayer.GetComponentInChildren<FadeOnTimeScale1>().timeScale = .45f;
                        Camera.main.gameObject.GetComponent<NewCameraBehavior>().targets.Remove(players[i]);
                        Destroy(players[i]);
                    }
                    else
                    {
                        ghostPlayer = (GameObject)GameObject.Instantiate(ghostAIPrefab, players[i].transform.position, players[i].transform.rotation);
                        Camera.main.gameObject.GetComponent<NewCameraBehavior>().targets.Remove(players[i]);
                        Destroy(players[i]);
                    }
                }
                else
                {
                    currentPlayers.Add(players[i]);
                }
            }
        }

        if (_SpriteSorter != null)
        {
            GameObject[]sortedPlayerList = new GameObject[4];

            foreach (GameObject playerObject in currentPlayers)
            {
                _SpriteSorter.AddToAllLists(playerObject.GetComponent<SpriteRenderer>());

                //sorts player list
                if (playerObject.name.Contains("1"))
                    sortedPlayerList[0] =  playerObject;
                else if (playerObject.name.Contains("2"))
                    sortedPlayerList[1] = playerObject;
                else if (playerObject.name.Contains("3"))
                    sortedPlayerList[2] = playerObject;
                else if (playerObject.name.Contains("4"))
                    sortedPlayerList[3] = playerObject;
            }

            currentPlayers = sortedPlayerList.ToList();
            
        }

        currentPlayers[ghostPlayerIndex] = ghostPlayer;

        for (int i = 0; i < currentPlayers.Count; i++)
        {
            if(currentPlayers[i] != null && currentPlayers[i].gameObject.tag != "Ghost")
            {
                Human playerHuman = currentPlayers[i].GetComponent<Human>();

                if(!SceneStartedInEditor)
                    playerHuman.IsFemaleWizard = isFemaleCharacter[i];

                if (playerColorPalettes[i] == null)
                {
                    if (playerHuman.IsFemaleWizard)
                        playerColorPalettes[i] = womanColorPalettes[i];
                    else
                        playerColorPalettes[i] = oldieColorPalettes[i];
                }

                VerifyCorrectAnimController(i);

                PaletteSwapper currentPlayer_PS = currentPlayers[i].GetComponent<PaletteSwapper>();
                currentPlayer_PS.currentPalette = playerColorPalettes[i];
                currentPlayer_PS.SwapColors_Custom(currentPlayer_PS.currentPalette);

                int targetPaletteIndex = (int)char.GetNumericValue(currentPlayer_PS.currentPalette.name[0]);
                playerHuman.MainColor = currentPlayer_PS.currentPalette.newPalette[targetPaletteIndex];
                playerNumText[i].color = playerHuman.MainColor;

                currentPlayers[i].GetComponent<SpriteRenderer>().material.SetColor("_OColor", playerNumText[i].color);

                playerHeartUIManagers[i].OccludeMat = currentPlayers[i].GetComponent<SpriteRenderer>().material;
                playerHeartUIManagers[i].SetHeartMaterial();
            }
        }

        

        currentGhostPlayer = ghostPlayer;
        currentPlayers.RemoveAll(item => item == null);
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
            OnGhostWin();
    }

    public void OnHumansWin()
    {
        didHumansWin = true;
        gameEnd = true;

        VibrateAllHumans(.75f, 1, 1);

		AkSoundEngine.PostEvent("Stop_FurnitureMove", gameObject);

        soundManager.SOUND_MAN.PlayHumanWinMusic();
    }

    public void OnGhostWin()
    {
        didHumansWin = false;
        gameEnd = true;
        int ghostPlayerNum = currentGhostPlayer.GetComponent<Ghost>().playerNum;   

		AkSoundEngine.PostEvent("Stop_FurnitureMove", gameObject);

        StartCoroutine(InputMapper.Vibration(ghostPlayerNum, .75f, 1, 1));

        soundManager.SOUND_MAN.PlayGhostWinMusic();
    }

    private void checkIsGameEnd()
    {
        if (gameEnd)
        {
            if (currentGhostPlayer.GetComponent<Ghost>())
            currentGhostPlayer.GetComponent<Ghost>().UpdateGameHasEnded(this);

            if (didHumansWin)
            {
                GhostPullToMiddle();
                if (Vector2.Distance(currentGhostPlayer.transform.position, GetComponent<RoomGenerator>().MainBaseRoomPiece.transform.position) < 1f)
                {
                    timer -= Time.fixedDeltaTime;

                    _HeartZoomTransition.enabled = true;
                    _HeartZoomTransition.StartHeartZoomInHalfway();
                }
            }
            else
            {
                timer = 0;

                Camera.main.transform.position = new Vector3(currentGhostPlayer.transform.position.x,currentGhostPlayer.transform.position.y, Camera.main.transform.position.z);
                _HeartZoomTransition.enabled = true;
                _HeartZoomTransition.StartHeartZoomInHalfway();
            }
            if (_HeartZoomTransition.IsZoomInHalfwayDone())
            {
                if (didHumansWin)
                {
                    if (!CatMode)
                    {
                        winnerNameText.text = "Humans Win!";
                    }
                    else
                    {
                        winnerNameText.text = "Meow Meow!";
                    }
                }
                else
                {
                    if (!CatMode)
                    {
                        winnerNameText.text = "Ghost Wins!";
                    }
                    else
                    {
                        winnerNameText.text = "Meow Meow!";
                    }
                }
                
                foreach (GameObject i in thingsToTurnOffAtGameEnd)
                {
                    i.SetActive(false);
                }
                
                foreach (GameObject i in thingsToTurnOnAtGameEnd)
                {
                    i.SetActive(true);
                }

                CeaseAllVibrations();
                Time.timeScale = 0;

                if (timer <= 0)
                {
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey("joystick button 0"))
                    {
                        _HeartZoomTransition.StartHeartZoomInFinish(2);
                    }
                    else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKey("joystick button 1"))
                    {
                        _HeartZoomTransition.StartHeartZoomInFinish(1);
                    }

                    timer = -1;
                }
            }
        }
    }
    void GhostPullToMiddle()
    {
        #if UNITY_EDITOR || UNITY_WEBGL || UNITY_WEBPLAYER || UNITY_STANDALONE
        if (currentGhostPlayer.GetComponent<Ghost>())
        currentGhostPlayer.GetComponent<Ghost>().debugCurrentSpeed = 0;
        #endif

        Vector3 roomPos = GetComponent<RoomGenerator>().MainBaseRoomPiece.transform.position;
            if (currentGhostPlayer.GetComponent<Ghost>())
            {
                currentGhostPlayer.GetComponent<Ghost>().currentSpeed = 0;
            }
            else 
            { 
                currentGhostPlayer.GetComponent<GhostAI>().ghostSpeed = 0;
                currentGhostPlayer.GetComponent<GhostAI>().currentState = GhostAI.GhostState.End;
            }
        if (Vector2.Distance(currentGhostPlayer.transform.position, roomPos) > 0.5f)
        {
            Vector2 direction = currentGhostPlayer.transform.position - roomPos;

            currentGhostPlayer.GetComponent<Rigidbody2D>().transform.position -= new Vector3(direction.x, direction.y, 0).normalized * 3 * Time.deltaTime;
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

    #region Palette & Spritesheet Methods
    private void VerifyCorrectAnimController(int playerIndex)
    {
        bool IsFemale = currentPlayers[playerIndex].GetComponent<Human>().IsFemaleWizard;
        Animator anim = currentPlayers[playerIndex].GetComponent<Animator>();

        if (IsFemale && !anim.runtimeAnimatorController.name.Contains("Woman"))
        {
            ChangePlayerAnimController(rPath_womanAnimController, anim);
            //ChangePlayerStartingSprite(playerIndex, IsFemale);
        }

        else if (!IsFemale && !anim.runtimeAnimatorController.name.Contains("oldie"))
        {
            ChangePlayerAnimController(rPath_oldieAnimController, anim);
            //ChangePlayerStartingSprite(playerIndex, IsFemale);
        }
    }

    private void ChangePlayerAnimController(string ResourcePath, Animator anim)
    {
        anim.runtimeAnimatorController = Resources.Load(ResourcePath) as RuntimeAnimatorController;
    }

    private void ChangePlayerAnimController(int playerIndex, string ResourcePath)
    {
        Animator anim = currentPlayers[playerIndex].GetComponent<Animator>();
        anim.runtimeAnimatorController = Resources.Load(ResourcePath) as RuntimeAnimatorController;
    }

    private void ChangePlayerStartingSprite(int playerIndex, bool IsWoman)
    {
        SpriteRenderer spriteRend = currentPlayers[playerIndex].GetComponent<SpriteRenderer>();

        if (IsWoman) spriteRend.sprite = startingWomanSprite;
        else spriteRend.sprite = startingOldieSprite;

    }
    #endregion

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
