using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using XInputDotNetPure;
using UnityEngine.SceneManagement;
public class CharacterSelectManager_Single : MonoBehaviour
{
    // Constants
    private const int MAX_PLAYER_COUNT = 1;
    private const int MIN_PLAYER_COUNT_TO_START = 1;
    private const float STICK_HORIZ_THRESHOLD = .5f;

    // XInput
    private bool[] getAButtonDown;
    private bool[] getBButtonDown;
    private bool[] wasAButtonPressed;
    private bool[] wasBButtonPressed;
    private StickStates[] playerAnalogStickStates;

    // General
    public HeartZoomTransition _HeartZoomTransition;
    public Sprite OldieStartSprite;
    public Sprite WomanStartSprite;
    public GameObject HumanSpriteObject;
    public GameObject GhostSpriteObject;
    private bool[] isPlayerReadyArray;
    private PlayerStates[] playerStates;
    private int playerCount = 0;
    private int ghostPlayerIndex = -1;
    private CharacterSelectStates currentCharSelectState = CharacterSelectStates.WaitingForPlayers;

    // UI
    [Header("Player")]
    public Transform[] PlayerSpriteReferencePointArray;
    public SpriteRenderer[] PlayerSpriteRendererArray;
    public PaletteSwapper[] PlayerPaletteSwapperArray;
    public GoBack goBackScript;

    [Header("Text")]
    public Text[] ReadyTextArray;
    public Text[] ColorTextArray;
    public Text[] PlayerNumTextArray;
    public GameObject PressToStartTextObject;

    [Header("Images")]
    public Image[] ButtonImageArray;
    public Image[] LeftColorArrowArray;
    public Image[] RightColorArrowArray;

    private List<Image> ghostSelectorImageList;
    private List<ColorPalette> PickedPalettesList;
    public int[] PlayerPosInPaletteList;
    private Text[] buttonTextArray;
    private Color transparentColor;

    [Header("Palettes")]
    public List<ColorPalette> AvailablePalettesList;
    public Sprite[] startingSprites;

    // Debug UI
    [Header("Debug")]

    public Text CharacterSelectDebugText;
    private string[] debugTextArray;

    private CharacterSelectStates CharSelectState
    {
        set
        {
            if (currentCharSelectState != value)
            {
                currentCharSelectState = value;

                switch ((int)currentCharSelectState)
                {
                    case (int)CharacterSelectStates.LoadMainScene:
                        Debug.Log("Starting CharacterSelectStates.LoadMainScene");

                        setCharacterSelectData();
                        _HeartZoomTransition.enabled = true;
                        Scene scene = SceneManager.GetActiveScene();
                        if (GhostSpriteObject.activeInHierarchy)
                        { 
                            _HeartZoomTransition.StartHeartZoomIn(scene.buildIndex + 1);
                        }
                        else
                        {
                            _HeartZoomTransition.StartHeartZoomIn(scene.buildIndex + 2);
                        }
                        // Application.LoadLevel(Application.loadedLevel + 1);
                        break;
                }
            }
        }
    }

    void Start()
    {
        Time.timeScale = 1;
        initializeVariables();

        #region Debug Code
#if UNITY_EDITOR
        updateDebugUI();

        Debug.Log("Gamepads connected: " + Input.GetJoystickNames().Length);
        foreach (string joystickName in Input.GetJoystickNames())
        {
            Debug.Log(joystickName);
        }
#else
        CharacterSelectDebugText.enabled = false;
#endif
        #endregion
    }

    private void initializeVariables()
    {
        isPlayerReadyArray = new bool[MAX_PLAYER_COUNT];

        buttonTextArray = new Text[MAX_PLAYER_COUNT];
        startingSprites = new Sprite[MAX_PLAYER_COUNT];

        getAButtonDown = new bool[MAX_PLAYER_COUNT];
        getBButtonDown = new bool[MAX_PLAYER_COUNT];
        wasAButtonPressed = new bool[MAX_PLAYER_COUNT];
        wasBButtonPressed = new bool[MAX_PLAYER_COUNT];

        playerStates = new PlayerStates[MAX_PLAYER_COUNT];

        playerAnalogStickStates = new StickStates[MAX_PLAYER_COUNT];

        for (int i = 0; i < MAX_PLAYER_COUNT; ++i)
        {
            isPlayerReadyArray[i] = false;
            buttonTextArray[i] = ButtonImageArray[i].GetComponentInChildren<Text>();
        }

        PickedPalettesList = new List<ColorPalette>();

        transparentColor = new Color(0, 0, 0, 0);

        debugTextArray = new string[MAX_PLAYER_COUNT];

        #region Debug Code
#if UNITY_EDITOR

        for (int i = 0; i < MAX_PLAYER_COUNT; ++i)
        {
            debugTextArray[i] = "P" + (i + 1) + ": Not Ready\n";
        }
#endif
        #endregion
    }

    void Update()
    {
        if (_HeartZoomTransition.enabled)
        {
            return;
        }

        if ((InputMapper.GrabVal(XBOX360_BUTTONS.B, 1) || Input.GetKeyDown(KeyCode.Backspace)) && CanGoBackToInstructions() && !wasBButtonPressed[0])
        {
            goBackScript.GoBackToInstructions();
            soundManager.SOUND_MAN.playSound("Play_MenuDown", gameObject);
        }

#if !UNITY_EDITOR && !UNITY_WEBGL && !UNITY_WEBPLAYER
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _HeartZoomTransition.enabled = true;
            _HeartZoomTransition.StartHeartZoomIn(-1);
        }
#endif

        switch ((int)currentCharSelectState)
        {
            case (int)CharacterSelectStates.WaitingForPlayers:
                updateButtonDownArrays();
                updateAnalogStickArray();

                if (playerCount >= MIN_PLAYER_COUNT_TO_START)
                {
                    for (int i = 0; i < isPlayerReadyArray.Length; i++)
                    {
                        if (isPlayerReadyArray[i] && InputMapper.GrabVal(XBOX360_BUTTONS.X, i + 1))
                        {
                            if (CanStartGame())
                            {
                                startGame();
                                soundManager.SOUND_MAN.playSound("Play_MenuConfirm", gameObject);
                            }
                        }
                    }
                }
                CheckForPlayerStateChanges();

                #region Debug Code
#if UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE
                if (Input.GetKeyDown(KeyCode.Space) && playerCount >= MIN_PLAYER_COUNT_TO_START)
                {
                    if (CanStartGame())
                    {
                        startGame();
                        soundManager.SOUND_MAN.playSound("Play_MenuConfirm", gameObject);
                    }
                }

                KeysCheckIfPlayerReady();
#endif
                #endregion
                break;

            case (int)CharacterSelectStates.LoadMainScene:
                CharSelectState = CharacterSelectStates.LoadMainScene;
                break;
        }
    }

    private bool CanGoBackToInstructions()
    {
        for (int i = 0; i < MAX_PLAYER_COUNT; i++)
        {
            if (playerStates[i] != PlayerStates.Inactive)
                return false;
        }

        return true;
    }

    private void setCharacterSelectData()
    {
        GameObject characterSelectData = GameObject.FindGameObjectWithTag("CharacterSelectData");
        if (characterSelectData != null)
        {
            CharacterSelectData charSelectData = characterSelectData.GetComponent<CharacterSelectData>();

            isPlayerReadyArray = new bool[4];
            isPlayerReadyArray[0] = true;
            isPlayerReadyArray[3] = true;
            playerCount = 2;

            if (GhostSpriteObject.activeInHierarchy)
            {
                ghostPlayerIndex = 0;
            }
            else
            {
                ghostPlayerIndex = 3;
            }

            charSelectData.SetIsPlayerReady(isPlayerReadyArray, playerCount, ghostPlayerIndex);
            charSelectData.LoadPaletteArray(PlayerPaletteSwapperArray);
            charSelectData.LoadStartSpritesArray(startingSprites);
            charSelectData.LoadIsFemaleBoolArray(OldieStartSprite, WomanStartSprite);
        }
        else
        {
            Debug.LogError("CharacterSelectManager: Could not find game object with tag \"CharacterSelectData\"");
        }
    }

    #region CharacterSelectStates.WaitingForPlayers Functions
    private void updateButtonDownArrays()
    {
        for (int i = 0; i < MAX_PLAYER_COUNT; ++i)
        {
            bool isAPressed = InputMapper.GrabVal(XBOX360_BUTTONS.A, i + 1);
            getAButtonDown[i] = false;

            if (isAPressed && !wasAButtonPressed[i])
            {
                wasAButtonPressed[i] = true;
                getAButtonDown[i] = true;
            }
            else if (!isAPressed && wasAButtonPressed[i])
            {
                wasAButtonPressed[i] = false;
            }

            bool isBPressed = InputMapper.GrabVal(XBOX360_BUTTONS.B, i + 1);
            getBButtonDown[i] = false;

            if (isBPressed && !wasBButtonPressed[i])
            {
                wasBButtonPressed[i] = true;
                getBButtonDown[i] = true;
            }
            else if (!isBPressed && wasBButtonPressed[i])
            {
                wasBButtonPressed[i] = false;
            }
        }
    }

    private void updateAnalogStickArray()
    {
        for (int i = 0; i < MAX_PLAYER_COUNT; i++)
        {
            if (playerStates[i] == PlayerStates.Inactive || playerStates[i] == PlayerStates.PickingColor)
            {
                float AnalogXDir = InputMapper.GrabVal(XBOX360_AXES.LeftStick_Horiz, i + 1);

                switch (playerAnalogStickStates[i])
                {
                    case StickStates.Neutral:
                        if (AnalogXDir > STICK_HORIZ_THRESHOLD)
                            playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Right, i);
                        else if (AnalogXDir < -STICK_HORIZ_THRESHOLD)
                            playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Left, i);
#if  UNITY_WEBGL || UNITY_WEBPLAYER || UNITY_STANDALONE || UNITY_EDITOR
                        else
                        {
                            switch (i)
                            {
                                case 0:
                                    if (Input.GetKeyDown(KeyCode.D))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Right, i);
                                    }
                                    else if (Input.GetKeyDown(KeyCode.A))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Left, i);
                                    }

                                    break;
                                case 1:
                                    if (Input.GetKeyDown(KeyCode.H))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Right, i);
                                    }
                                    else if (Input.GetKeyDown(KeyCode.F))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Left, i);
                                    }
                                    break;
                                case 2:
                                    if (Input.GetKeyDown(KeyCode.L))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Right, i);
                                    }
                                    else if (Input.GetKeyDown(KeyCode.J))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Left, i);
                                    }
                                    break;
                                case 3:
                                    if (Input.GetKeyDown(KeyCode.RightArrow))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Right, i);
                                    }
                                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Left, i);
                                    }
                                    break;
                            }
                        }
#endif
                        break;

                    case StickStates.Left:
                        if (AnalogXDir < -STICK_HORIZ_THRESHOLD)
                            return;
                        else if (AnalogXDir > -STICK_HORIZ_THRESHOLD && AnalogXDir < STICK_HORIZ_THRESHOLD)
                            playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Neutral, i);
                        else if (AnalogXDir > STICK_HORIZ_THRESHOLD)
                            playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Right, i);
#if  UNITY_WEBGL || UNITY_WEBPLAYER || UNITY_STANDALONE || UNITY_EDITOR
                        else
                        {
                            switch (i)
                            {
                                case 0:
                                    if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Neutral, i);
                                    }
                                    else if (Input.GetKeyDown(KeyCode.D))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Right, i);
                                    }
                                    break;
                                case 1:
                                    if (!Input.GetKey(KeyCode.H) && !Input.GetKey(KeyCode.F))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Neutral, i);
                                    }
                                    else if (Input.GetKeyDown(KeyCode.H))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Right, i);
                                    }
                                    break;
                                case 2:
                                    if (!Input.GetKey(KeyCode.L) && !Input.GetKey(KeyCode.J))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Neutral, i);
                                    }
                                    else if (Input.GetKeyDown(KeyCode.L))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Right, i);
                                    }
                                    break;
                                case 3:
                                    if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Neutral, i);
                                    }
                                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Right, i);
                                    }
                                    break;
                            }
                        }
#endif
                        break;

                    case StickStates.Right:
                        if (AnalogXDir > STICK_HORIZ_THRESHOLD)
                            return;
                        else if (AnalogXDir < STICK_HORIZ_THRESHOLD && AnalogXDir > -STICK_HORIZ_THRESHOLD)
                            playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Neutral, i);
                        else if (AnalogXDir < -STICK_HORIZ_THRESHOLD)
                            playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Left, i);
#if UNITY_WEBGL || UNITY_WEBPLAYER || UNITY_STANDALONE || UNITY_EDITOR
                        else
                        {
                            switch (i)
                            {
                                case 0:
                                    if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Neutral, i);
                                    }
                                    else if (Input.GetKeyDown(KeyCode.A))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Left, i);
                                    }
                                    break;
                                case 1:
                                    if (!Input.GetKey(KeyCode.H) && !Input.GetKey(KeyCode.F))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Neutral, i);
                                    }
                                    else if (Input.GetKeyDown(KeyCode.F))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Left, i);
                                    }
                                    break;
                                case 2:
                                    if (!Input.GetKey(KeyCode.L) && !Input.GetKey(KeyCode.J))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Neutral, i);
                                    }
                                    else if (Input.GetKeyDown(KeyCode.J))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Left, i);
                                    }
                                    break;
                                case 3:
                                    if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Neutral, i);
                                    }
                                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                                    {
                                        playerAnalogStickStates[i] = OnAnalogStickStateChange(StickStates.Left, i);
                                    }
                                    break;
                            }
                        }
#endif
                        break;
                }
            }
        }
    }



    private void checkIfPlayerReady()
    {
        for (int i = 1; i <= MAX_PLAYER_COUNT; ++i)
        {
            if (getAButtonDown[i - 1] && !isPlayerReadyArray[i - 1])
            {
                updateUI_playerReady(i - 1, true);
                StartCoroutine(InputMapper.Vibration(i, .2f, 0, .8f));
                PlayerPaletteSwapperArray[i - 1].UpdatePlayerNumTextColor();
                soundManager.SOUND_MAN.playSound("Play_PlayerJoin", gameObject);

                #region Debug Code
#if UNITY_EDITOR
                debugTextArray[i - 1] = "P" + i + ": Ready\n";
                updateDebugUI();
#endif
                #endregion
            }
            else if (getBButtonDown[i - 1] && isPlayerReadyArray[i - 1])
            {
                updateUI_playerReady(i - 1, false);
                PlayerPaletteSwapperArray[i - 1].UpdatePlayerNumTextColor(Color.white);
                soundManager.SOUND_MAN.playSound("Play_MenuDown", gameObject);

                #region Debug Code
#if UNITY_EDITOR
                debugTextArray[i - 1] = "P" + i + ": Not Ready\n";
                updateDebugUI();
#endif
                #endregion
            }
        }
    }

    private bool CanStartGame()
    {
        foreach (PlayerStates state in playerStates)
        {
            if (state == PlayerStates.PickingColor)
                return false;
        }

        return true;
    }

    //Zero-based, P1 = 0

    #region PlayerStateChecks

    private void CheckForPlayerStateChanges()
    {
        for (int i = 0; i < MAX_PLAYER_COUNT; i++)
        {
            switch (playerStates[i])
            {
                case PlayerStates.Inactive:
                    if (CheckPlayerJoinedIn(i) && !GhostSpriteObject.activeInHierarchy)
                        playerStates[i] = ChangePlayerState(PlayerStates.PickingColor, i);
                    break;

                case PlayerStates.PickingColor:
                    playerStates[i] = ChangePlayerState(CheckPlayerPickedColor(i), i);
                    break;

                case PlayerStates.Ready:
                    if (CheckPlayerOptedOut(i))
                        playerStates[i] = ChangePlayerState(PlayerStates.PickingColor, i);
                    break;
            }
        }
    }

    private bool CheckPlayerJoinedIn(int playerNum)
    {
        if (getAButtonDown[playerNum])
        {
            #region Debug Code
#if UNITY_EDITOR
            debugTextArray[playerNum] = "P" + (playerNum + 1) + ": Joined in\n";
            updateDebugUI();
#endif
            #endregion
            return true;
        }

        return false;
    }

    private PlayerStates CheckPlayerPickedColor(int playerNum)
    {
        if (getAButtonDown[playerNum] && CanClaimPaletteColor(playerNum))
        {
            return PlayerStates.Ready;
            #region Debug Code
#if UNITY_EDITOR
            debugTextArray[playerNum] = "P" + (playerNum + 1) + ": Ready\n";
            updateDebugUI();
#endif
            #endregion
        }

        else if (getBButtonDown[playerNum])
            return PlayerStates.Inactive;

        return PlayerStates.PickingColor;
    }

    private bool CheckPlayerOptedOut(int playerNum)
    {
        if (getBButtonDown[playerNum])
        {
            #region Debug Code
#if UNITY_EDITOR
            debugTextArray[playerNum] = "P" + (playerNum + 1) + ": Un-Ready\n";
            updateDebugUI();
#endif
            #endregion
            return true;
        }

        return false;
    }
    #endregion

    StickStates OnAnalogStickStateChange(StickStates newState, int player)
    {
        if (playerStates[0] == PlayerStates.Inactive)
        {
            switch (newState)
            {
                case StickStates.Right:
                    HumanSpriteObject.SetActive(!HumanSpriteObject.activeInHierarchy);
                    GhostSpriteObject.SetActive(!HumanSpriteObject.activeInHierarchy);

                    RightColorArrowArray[player].transform.localScale = new Vector3(.6f, .6f, 1);
                    soundManager.SOUND_MAN.playSound("Play_Menu_Up", gameObject);

                    if (GhostSpriteObject.activeInHierarchy)
                    {
                        updateUI_playerReady(0, true);
                    }
                    else
                    {
                        updateUI_playerReady(0, false);
                    }
                    break;

                case StickStates.Left:
                    HumanSpriteObject.SetActive(!HumanSpriteObject.activeInHierarchy);
                    GhostSpriteObject.SetActive(!HumanSpriteObject.activeInHierarchy);

                    LeftColorArrowArray[player].transform.localScale = new Vector3(.6f, .6f, 1);
                    soundManager.SOUND_MAN.playSound("Play_Menu_Up", gameObject);

                    if (GhostSpriteObject.activeInHierarchy)
                    {
                        updateUI_playerReady(0, true);
                    }
                    else
                    {
                        updateUI_playerReady(0, false);
                    }
                    break;

                case StickStates.Neutral:
                    if (playerAnalogStickStates[player] == StickStates.Right)
                        RightColorArrowArray[player].transform.localScale = new Vector3(.5f, .5f, 1);
                    else
                        LeftColorArrowArray[player].transform.localScale = new Vector3(.5f, .5f, 1);
                    break;
            }
        }
        if (playerStates[0] == PlayerStates.PickingColor)
        {
            switch (newState)
            {
                case StickStates.Right:
                    if (PlayerPosInPaletteList[player] >= AvailablePalettesList.Count - 1)
                        PlayerPosInPaletteList[player] = 0;
                    else
                        PlayerPosInPaletteList[player]++;

                    RightColorArrowArray[player].transform.localScale = new Vector3(.6f, .6f, 1);
                    soundManager.SOUND_MAN.playSound("Play_Menu_Up", gameObject);
                    break;

                case StickStates.Left:
                    if (PlayerPosInPaletteList[player] == 0)
                        PlayerPosInPaletteList[player] = AvailablePalettesList.Count - 1;
                    else
                        PlayerPosInPaletteList[player]--;

                    LeftColorArrowArray[player].transform.localScale = new Vector3(.6f, .6f, 1);
                    soundManager.SOUND_MAN.playSound("Play_Menu_Up", gameObject);
                    break;

                case StickStates.Neutral:
                    if (playerAnalogStickStates[player] == StickStates.Right)
                        RightColorArrowArray[player].transform.localScale = new Vector3(.5f, .5f, 1);
                    else
                        LeftColorArrowArray[player].transform.localScale = new Vector3(.5f, .5f, 1);
                    break;
            }

            SwapAndUpdatePalette(player);
            VerifyCharacterSprite(player);
            SetCorrectPlayerNumTextColor(player);
        }

        return newState;
    }

    private void updateUI_playerReady(int playerIndex, bool isPlayerReady)
    {
        isPlayerReadyArray[playerIndex] = isPlayerReady;

        //PlayerSpriteRendererArray[playerIndex].enabled = isPlayerReady;

        if (isPlayerReady)
        {
            ++playerCount;

            ReadyTextArray[playerIndex].color = Color.black;
            HideTextPrompt(playerIndex);

            if (playerCount == MIN_PLAYER_COUNT_TO_START)
            {
                PressToStartTextObject.SetActive(true);
            }
        }
        else
        {
            --playerCount;

            ReadyTextArray[playerIndex].color = transparentColor;
            ShowTextPrompt(playerIndex);

            if (playerCount < MIN_PLAYER_COUNT_TO_START)
            {
                PressToStartTextObject.SetActive(false);
            }
        }
    }

    private void startGame()
    {
        Debug.Log("Starting game...");
        FillStartSpriteArray();
        CharSelectState = CharacterSelectStates.LoadMainScene;
    }

    private void FillStartSpriteArray()
    {
        for (int i = 0; i < MAX_PLAYER_COUNT; i++)
        {
            if (PlayerPaletteSwapperArray[i].currentPalette.name.Contains("Lady"))
                startingSprites[i] = WomanStartSprite;
            else if (PlayerPaletteSwapperArray[i].currentPalette.name.Contains("Man"))
                startingSprites[i] = OldieStartSprite;
        }
    }

    private PlayerStates ChangePlayerState(PlayerStates newState, int playerIndex)
    {
        PlayerStates currentState = playerStates[playerIndex];

        if (newState == currentState)
            return currentState;

        switch (currentState)
        {
            case PlayerStates.Inactive:
                if (newState == PlayerStates.Ready)
                    return currentState;

                else if (newState == PlayerStates.PickingColor)
                {
                    SetCorrectPlayerNumTextColor(playerIndex);

                    StartCoroutine(InputMapper.Vibration(playerIndex + 1, .2f, 0, .8f));
                    soundManager.SOUND_MAN.playSound("Play_PlayerJoin", gameObject);

                    //ShowSpriteAndSetPalette(playerIndex);
                    ColorTextArray[playerIndex].color = Color.black;

                    HideTextPrompt(playerIndex);

                    SwapAndUpdatePalette(playerIndex);
                    VerifyCharacterSprite(playerIndex);
                    SetCorrectPlayerNumTextColor(playerIndex);
                }
                break;

            case PlayerStates.PickingColor:
                if (newState == PlayerStates.PickingColor)
                    return currentState;

                else if (newState == PlayerStates.Inactive)
                {
                    soundManager.SOUND_MAN.playSound("Play_MenuDown", gameObject);
                    PlayerPaletteSwapperArray[playerIndex].UpdatePlayerNumTextColor(Color.white);

                    //PlayerSpriteRendererArray[playerIndex].enabled = false;
                    ColorTextArray[playerIndex].color = transparentColor;

                    ShowTextPrompt(playerIndex);
                    ResetArrowSizes(playerIndex);
                }

                else //state must be PlayerStates.Ready
                {
                    updateUI_playerReady(playerIndex, true);
                    StartCoroutine(InputMapper.Vibration(playerIndex + 1, .2f, 0, .8f));
                    soundManager.SOUND_MAN.playSound("Play_PlayerJoin", gameObject);

                    ColorTextArray[playerIndex].color = transparentColor;

                    ResetArrowSizes(playerIndex);

                    ClaimPaletteColor(playerIndex);
                }

                break;

            case PlayerStates.Ready:
                if (newState == PlayerStates.Inactive)
                    return currentState;

                else if (newState == PlayerStates.PickingColor)
                {
                    updateUI_playerReady(playerIndex, false);
                    PlayerSpriteRendererArray[playerIndex].enabled = true;
                    soundManager.SOUND_MAN.playSound("Play_MenuDown", gameObject);

                    ColorTextArray[playerIndex].color = Color.black;

                    HideTextPrompt(playerIndex);
                    ResetArrowSizes(playerIndex);

                    UnclaimPaletteColor(playerIndex);

                    PlayerPosInPaletteList[playerIndex] = AvailablePalettesList.Count - 1;

                    SwapAndUpdatePalette(playerIndex);
                    VerifyCharacterSprite(playerIndex);
                    SetCorrectPlayerNumTextColor(playerIndex);
                }
                break;
        }

        return newState;
    }

    #endregion

    #region UI Functions

    void ShowTextPrompt(int playerIndex)
    {
        ButtonImageArray[playerIndex].color = Color.white;
        ButtonImageArray[playerIndex].GetComponent<UIFlasher>().enabled = true;
        buttonTextArray[playerIndex].color = Color.black;
        buttonTextArray[playerIndex].GetComponent<UIFlasher>().enabled = true;
    }

    void HideTextPrompt(int playerIndex)
    {
        ButtonImageArray[playerIndex].color = transparentColor;
        ButtonImageArray[playerIndex].GetComponent<UIFlasher>().enabled = false;
        buttonTextArray[playerIndex].color = transparentColor;
        buttonTextArray[playerIndex].GetComponent<UIFlasher>().enabled = false;
    }

    void ResetArrowSizes(int playerIndex)
    {
        RightColorArrowArray[playerIndex].transform.localScale = new Vector3(.5f, .5f, 1);
        LeftColorArrowArray[playerIndex].transform.localScale = new Vector3(.5f, .5f, 1);
    }

    #endregion

    #region Palette Functions

    void ShowSpriteAndSetPalette(int playerIndex)
    {
        int targetPaletteIndex = (int)char.GetNumericValue(AvailablePalettesList[PlayerPosInPaletteList[playerIndex]].name[0]);
        PlayerPaletteSwapperArray[playerIndex].UpdatePlayerNumTextColor(AvailablePalettesList[PlayerPosInPaletteList[playerIndex]].newPalette[targetPaletteIndex]);
        PlayerSpriteRendererArray[playerIndex].enabled = true;
    }

    void VerifyCharacterSprite(int playerIndex)
    {
        if (PlayerPaletteSwapperArray[playerIndex].currentPalette.name.Contains("Lady") && !PlayerSpriteRendererArray[playerIndex].GetComponent<Animator>().GetBool("IsWoman"))
            PlayerSpriteRendererArray[playerIndex].GetComponent<Animator>().SetBool("IsWoman", true);

        else if (!PlayerPaletteSwapperArray[playerIndex].currentPalette.name.Contains("Lady") && PlayerSpriteRendererArray[playerIndex].GetComponent<Animator>().GetBool("IsWoman"))
            PlayerSpriteRendererArray[playerIndex].GetComponent<Animator>().SetBool("IsWoman", false);
    }

    void SetCorrectPlayerNumTextColor(int playerIndex)
    {
        int paletteColorIndex;
        if (PlayerSpriteRendererArray[playerIndex].GetComponent<Animator>().GetBool("IsWoman"))
            paletteColorIndex = 6;
        else
            paletteColorIndex = 7;

        PlayerPaletteSwapperArray[playerIndex].UpdatePlayerNumTextColor(AvailablePalettesList[PlayerPosInPaletteList[playerIndex]].newPalette[paletteColorIndex]);
    }

    void SwapAndUpdatePalette(int playerIndex)
    {
        PlayerPaletteSwapperArray[playerIndex].SwapColors_Custom(AvailablePalettesList[PlayerPosInPaletteList[playerIndex]]);
        PlayerPaletteSwapperArray[playerIndex].currentPalette = AvailablePalettesList[PlayerPosInPaletteList[playerIndex]];
    }

    private bool CanClaimPaletteColor(int playerNum)
    {
        if (PickedPalettesList.Contains(PlayerPaletteSwapperArray[playerNum].currentPalette))
        {
            Debug.Log("Cannot claim " + PlayerPaletteSwapperArray[playerNum].currentPalette);
            return false;
        }
        return true;
    }

    private void ClaimPaletteColor(int playerNum)
    {
        PickedPalettesList.Add(PlayerPaletteSwapperArray[playerNum].currentPalette);
        AvailablePalettesList.Remove(PlayerPaletteSwapperArray[playerNum].currentPalette);
        Debug.Log("Claimed " + PlayerPaletteSwapperArray[playerNum].currentPalette);
    }

    private void UnclaimPaletteColor(int playerNum)
    {
        AvailablePalettesList.Add(PlayerPaletteSwapperArray[playerNum].currentPalette);
        PickedPalettesList.Remove(PlayerPaletteSwapperArray[playerNum].currentPalette);
        Debug.Log("Removed " + PlayerPaletteSwapperArray[playerNum].currentPalette);
    }

    #endregion

    #region CharacterSelectStates.SelectingGhost Functions
    private void hideWaitingForPlayerUI()
    {
        PressToStartTextObject.SetActive(false);

        for (int i = 0; i < MAX_PLAYER_COUNT; ++i)
        {
            ReadyTextArray[i].color = transparentColor;
            ButtonImageArray[i].color = transparentColor;
            ButtonImageArray[i].GetComponent<UIFlasher>().enabled = false;
            buttonTextArray[i].color = transparentColor;
            buttonTextArray[i].GetComponent<UIFlasher>().enabled = false;
            PlayerNumTextArray[i].enabled = false;
        }
    }

    // t = current, how long into the ease are we
    // b = initial value, starting value if current were 0
    // c = total change, total change in the value (not the end value, but the end - start)
    // d = duration, the total amount of time (when current == duration, the returned value will == initial + totalChange)
    private float easeInCubic(float t, float b, float c, float d)
    {
        t /= d;
        return (c * t * t * t + b);
    }
    #endregion

    #region Debug Functions
    private void KeysCheckIfPlayerReady()
    {
        CheckPlayerStateViaKeys(0, KeyCode.W, KeyCode.S);
    }

    void CheckPlayerStateViaKeys(int playerIndex, KeyCode OptIn, KeyCode OptOut)
    {
        if (Input.GetKeyDown(OptIn) && !GhostSpriteObject.activeInHierarchy)
        {
            if (playerStates[playerIndex] == PlayerStates.Inactive)
                playerStates[playerIndex] = ChangePlayerState(PlayerStates.PickingColor, playerIndex);

            else if (playerStates[playerIndex] == PlayerStates.PickingColor && CanClaimPaletteColor(playerIndex))
                playerStates[playerIndex] = ChangePlayerState(PlayerStates.Ready, playerIndex);
        }

        else if (Input.GetKeyDown(OptOut))
        {
            if (playerStates[playerIndex] == PlayerStates.Ready)
                playerStates[playerIndex] = ChangePlayerState(PlayerStates.PickingColor, playerIndex);

            else if (playerStates[playerIndex] == PlayerStates.PickingColor)
                playerStates[playerIndex] = ChangePlayerState(PlayerStates.Inactive, playerIndex);
        }
    }

    private void updateDebugUI()
    {
#if UNITY_EDITOR
        CharacterSelectDebugText.text = "";

        for (int i = 0; i < MAX_PLAYER_COUNT; ++i)
        {
            CharacterSelectDebugText.text += debugTextArray[i];
        }
#endif
    }
    #endregion
}
