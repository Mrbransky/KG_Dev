using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using XInputDotNetPure;

public enum CharacterSelectStates
{
    WaitingForPlayers = 0,
    SelectingGhost = 1,
    GhostRevealed = 2,
    Panic = 3,
    RunFromGhost = 4,
    LoadMainScene = 5
}

public class CharacterSelectManager : MonoBehaviour
{
    // Constants
    private const int MAX_PLAYER_COUNT = 4;
    private const int MIN_PLAYER_COUNT_TO_START = 2;

    // XInput
    private bool[] getAButtonDown;
    private bool[] getBButtonDown;
    private bool[] wasAButtonPressed;
    private bool[] wasBButtonPressed;

    // General
    public HeartZoomTransition _HeartZoomTransition;
    private bool[] isPlayerReadyArray;
    private int playerCount = 0;
    private int ghostPlayerIndex = -1;
    private CharacterSelectStates currentCharSelectState = CharacterSelectStates.WaitingForPlayers;

    // Game Start Sequence: General
    public Transform GhostSpriteReferencePointTransform;
    public float GhostSelectionDuration = 5;
    public float GhostRevealDuration = 1.5f;
    public float PanicDuration = 1.5f;
    public float RunFromGhostDuration = 2;

    // Game Start Sequence: Run from ghost
    public float PlayerRunSpeed = 10;
    public float GhostRunSpeed = 7;
    private int[] runFromGhostDirection;

    // Game Start Sequence: Ghost selection
    public float initialTimeToNextGhostSelector = 0.05f;
    public float maxTimeBetweenGhostSelector = 1;

    private float timeSinceGhostSelectingStart = 0;
    private float timeToNextGhostSelector;
    private float maxGhostSelectionDuration;
    private int currentGhostSelectorIndex = 0;

    // UI
    public Transform[] PlayerSpriteReferencePointArray;
    public SpriteRenderer[] PlayerSpriteRendererArray;
    public Text[] ReadyTextArray;
    public Text[] PlayerNumTextArray;
    public Image[] ButtonImageArray;
    public Image[] GhostSelectorImageArray;
    public GameObject PressToStartTextObject;

    private List<Image> ghostSelectorImageList;
    private Text[] buttonTextArray;
    private Color transparentColor;

    // Debug UI
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
                    case (int)CharacterSelectStates.SelectingGhost:
                        Debug.Log("Starting CharacterSelectStates.SelectingGhost");

                        hideWaitingForPlayerUI();
                        initializeGhostPlayerIndex();
                        setRunFromGhostDirection();
                        setCharacterSelectData();
                        break;

                    case (int)CharacterSelectStates.GhostRevealed:
                        Debug.Log("Starting CharacterSelectStates.GhostRevealed");

                        initializeGhostSprite();
                        setGhostRevealedPlayerDirections();
                        break;

                    case (int)CharacterSelectStates.Panic:
                        Debug.Log("Starting CharacterSelectStates.Panic");

                        startPanicAnimations();
                        break;

                    case (int)CharacterSelectStates.RunFromGhost:
                        Debug.Log("Starting CharacterSelectStates.RunFromGhost");

                        startRunAnimations();
                        setRunFromGhostPlayerDirections();
                        break;

                    case (int)CharacterSelectStates.LoadMainScene:
                        Debug.Log("Starting CharacterSelectStates.LoadMainScene");

                        _HeartZoomTransition.enabled = true;
                        _HeartZoomTransition.StartHeartZoomIn(Application.loadedLevel + 1);
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
        runFromGhostDirection = new int[MAX_PLAYER_COUNT];

        getAButtonDown = new bool[MAX_PLAYER_COUNT];
        getBButtonDown = new bool[MAX_PLAYER_COUNT];
        wasAButtonPressed = new bool[MAX_PLAYER_COUNT];
        wasBButtonPressed = new bool[MAX_PLAYER_COUNT];

        for (int i = 0; i < MAX_PLAYER_COUNT; ++i)
        {
            isPlayerReadyArray[i] = false;
            buttonTextArray[i] = ButtonImageArray[i].GetComponentInChildren<Text>();
            runFromGhostDirection[i] = -1;
        }

        transparentColor = new Color(0, 0, 0, 0);
        maxGhostSelectionDuration = GhostSelectionDuration;
        timeToNextGhostSelector = initialTimeToNextGhostSelector;

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
            if (currentCharSelectState == CharacterSelectStates.LoadMainScene)
            {
                moveSprites();
            }

            return;
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

                if (playerCount >= MIN_PLAYER_COUNT_TO_START)
                {
                    for (int i = 0; i < isPlayerReadyArray.Length; i++)
                    {
                        if (isPlayerReadyArray[i] && InputMapper.GrabVal(XBOX360_BUTTONS.X, i + 1))
                        {
                            startGame();
                            soundManager.SOUND_MAN.playSound("Play_MenuConfirm", gameObject);
                        }
                    }
                }

                checkIfPlayerReady();

#region Debug Code
#if UNITY_EDITOR || UNITY_WEBGL //|| UNITY_STANDALONE
                if (Input.GetKeyDown(KeyCode.Space) && playerCount >= MIN_PLAYER_COUNT_TO_START)
                {
                    startGame();
                    soundManager.SOUND_MAN.playSound("Play_MenuConfirm", gameObject);
                }

                debugCheckIfPlayerReady();
#endif
#endregion
                break;

            case (int)CharacterSelectStates.SelectingGhost:
                if (GhostSelectionDuration > 0)
                {
                    GhostSelectionDuration -= Time.deltaTime;
                    timeSinceGhostSelectingStart += Time.deltaTime;

                    if (GhostSelectionDuration < maxTimeBetweenGhostSelector)
                    {
                        setFinalGhostSelectorImage();
                    }
                    else if (timeToNextGhostSelector > 0)
                    {
                        timeToNextGhostSelector -= Time.deltaTime;
                    }
                    else
                    {
                        setNextGhostSelectorImage();
                        soundManager.SOUND_MAN.playSound("Play_MenuDown", gameObject);
                        timeToNextGhostSelector = easeInCubic(timeSinceGhostSelectingStart, initialTimeToNextGhostSelector, maxTimeBetweenGhostSelector, maxGhostSelectionDuration);
                    }
                }
                else
                {
                    CharSelectState = CharacterSelectStates.GhostRevealed;
                }
                break;

            case (int)CharacterSelectStates.GhostRevealed:
                if (GhostRevealDuration > 0)
                {
                    GhostRevealDuration -= Time.deltaTime;
                }
                else
                {
                    CharSelectState = CharacterSelectStates.Panic;
                }
                break;

            case (int)CharacterSelectStates.Panic:
                if (PanicDuration > 0)
                {
                    PanicDuration -= Time.deltaTime;
                }
                else
                {
                    CharSelectState = CharacterSelectStates.RunFromGhost;
                }
                break;

            case (int)CharacterSelectStates.RunFromGhost:
            case (int)CharacterSelectStates.LoadMainScene:
                if (RunFromGhostDuration > 0)
                {
                    RunFromGhostDuration -= Time.deltaTime;
                }
                else
                {
                    CharSelectState = CharacterSelectStates.LoadMainScene;
                }
                   
                moveSprites();
                break;
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

    private void checkIfPlayerReady()
    {
        for (int i = 1; i <= MAX_PLAYER_COUNT; ++i)
        {
            //if(InputMapper.GrabVal(XBOX360_BUTTONS.A, i) && InputMapper.GrabVal(XBOX360_BUTTONS.B, i))
            //{
            //    if(isPlayerReadyArray[i-1])
            //    {
            //        updateUI_playerReady(i - 1, false);
            //        soundManager.SOUND_MAN.playSound("Play_MenuDown", gameObject);
            //    }
            //}

            if (getAButtonDown[i - 1] && !isPlayerReadyArray[i - 1])
            {
                updateUI_playerReady(i - 1, true);
                StartCoroutine(InputMapper.Vibration(i, .2f, 0, .8f));
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

    private void updateUI_playerReady(int playerIndex, bool isPlayerReady)
    {
        isPlayerReadyArray[playerIndex] = isPlayerReady;

        PlayerSpriteRendererArray[playerIndex].enabled = isPlayerReady;

        if (isPlayerReady)
        {
            ++playerCount;

            ReadyTextArray[playerIndex].color = Color.black;
            ButtonImageArray[playerIndex].color = transparentColor;
            ButtonImageArray[playerIndex].GetComponent<UIFlasher>().enabled = false;
            buttonTextArray[playerIndex].color = transparentColor;
            buttonTextArray[playerIndex].GetComponent<UIFlasher>().enabled = false;

            if (playerCount == MIN_PLAYER_COUNT_TO_START)
            {
                PressToStartTextObject.SetActive(true);
            }
        }
        else
        {
            --playerCount;

            ReadyTextArray[playerIndex].color = transparentColor;
            ButtonImageArray[playerIndex].color = Color.white;
            ButtonImageArray[playerIndex].GetComponent<UIFlasher>().enabled = true;
            buttonTextArray[playerIndex].color = Color.black;
            buttonTextArray[playerIndex].GetComponent<UIFlasher>().enabled = true;

            if (playerCount < MIN_PLAYER_COUNT_TO_START)
            {
                PressToStartTextObject.SetActive(false);
            }
        }
    }

    private void startGame()
    {
        Debug.Log("Starting game...");

        CharSelectState = CharacterSelectStates.SelectingGhost;
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

    private void initializeGhostPlayerIndex()
    {
        ghostSelectorImageList = new List<Image>();
        int randGhostPlayerNumber = Random.Range(0, playerCount);
        int ghostPlayer_ForLoopCounter = 0;

        for (int i = 0; i < isPlayerReadyArray.Length; ++i)
        {
            if (isPlayerReadyArray[i])
            {
                if (ghostPlayer_ForLoopCounter == randGhostPlayerNumber)
                {
                    ghostPlayerIndex = i;
                }
                else
                {
                    ghostSelectorImageList.Add(GhostSelectorImageArray[i]);
                }

                ++ghostPlayer_ForLoopCounter;
            }
        }

        ghostSelectorImageList.Add(GhostSelectorImageArray[ghostPlayerIndex]);
    }

    private void setRunFromGhostDirection()
    {
        int playersToRightOfGhost = 0;
        int playersToLeftOfGhost = 0;

        for (int i = MAX_PLAYER_COUNT - 1; i > ghostPlayerIndex; --i)
        {
            if (isPlayerReadyArray[i])
            {
                runFromGhostDirection[i] = 1;
                ++playersToRightOfGhost;
            }
        }

        playersToLeftOfGhost = playerCount - playersToRightOfGhost - 1;

        if (playersToRightOfGhost > playersToLeftOfGhost)
        {
            flipGhostDirection();
        }
    }

    private void flipGhostDirection()
    {
        runFromGhostDirection[ghostPlayerIndex] = 1;

        Vector3 newScale = GhostSpriteReferencePointTransform.localScale;
        newScale.x *= -1;
        GhostSpriteReferencePointTransform.localScale = newScale;
    }

    private void setCharacterSelectData()
    {
        GameObject characterSelectData = GameObject.FindGameObjectWithTag("CharacterSelectData");
        if (characterSelectData != null)
        {
            characterSelectData.GetComponent<CharacterSelectData>().SetIsPlayerReady(isPlayerReadyArray, playerCount, ghostPlayerIndex);
        }
        else
        {
            Debug.LogError("CharacterSelectManager: Could not find game object with tag \"CharacterSelectData\"");
        }
    }

    private void setNextGhostSelectorImage()
    {
        // Disable and move current selector to back of list
        ghostSelectorImageList[currentGhostSelectorIndex].enabled = false;
        Image tempImage = ghostSelectorImageList[currentGhostSelectorIndex];
        ghostSelectorImageList.RemoveAt(currentGhostSelectorIndex);
        ghostSelectorImageList.Add(tempImage);

        // Enable next selector
        currentGhostSelectorIndex = Random.Range(0, ghostSelectorImageList.Count - 1);
        ghostSelectorImageList[currentGhostSelectorIndex].enabled = true;
    }

    private void setFinalGhostSelectorImage()
    {
        ghostSelectorImageList[currentGhostSelectorIndex].enabled = false;
        GhostSelectorImageArray[ghostPlayerIndex].enabled = true;
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

#region CharacterSelectStates.GhostRevealed Functions
    private void initializeGhostSprite()
    {
        GhostSelectorImageArray[ghostPlayerIndex].enabled = false;
        GhostSpriteReferencePointTransform.position = PlayerSpriteRendererArray[ghostPlayerIndex].transform.position;
        PlayerSpriteRendererArray[ghostPlayerIndex].gameObject.SetActive(false);
    }

    private void setGhostRevealedPlayerDirections()
    {
        for (int i = 0; i < ghostPlayerIndex; ++i)
        {
            if (isPlayerReadyArray[i])
            {
                flipPlayerDirection(i);
            }
        }
    }

    private void flipPlayerDirection(int index)
    {
        Vector3 newScale = PlayerSpriteReferencePointArray[index].localScale;
        newScale.x *= -1;
        PlayerSpriteReferencePointArray[index].localScale = newScale;
    }
#endregion

#region CharacterSelectStates.Panic Functions
    private void startPanicAnimations()
    {
        GhostSpriteReferencePointTransform.GetComponentInChildren<Animator>().enabled = true;

        for (int i = 0; i < MAX_PLAYER_COUNT; ++i)
        {
            if (isPlayerReadyArray[i])
            {
                PlayerSpriteRendererArray[i].GetComponent<Animator>().SetBool("goToPanic", true);
            }
        }
    }
#endregion

#region CharacterSelectStates.RunFromGhost Functions
    private void setRunFromGhostPlayerDirections()
    {
        for (int i = 0; i < MAX_PLAYER_COUNT; ++i)
        {
            if (i != ghostPlayerIndex)
            {
                flipPlayerDirection(i);
            }
        }
    }

    private void startRunAnimations()
    {
        for (int i = 0; i < MAX_PLAYER_COUNT; ++i)
        {
            if (isPlayerReadyArray[i])
            {
                PlayerSpriteRendererArray[i].GetComponent<Animator>().SetBool("goToRun", true);
            }
        }
    }

    private void moveSprites()
    {
        for (int i = 0; i < MAX_PLAYER_COUNT; ++i)
        {
            if (isPlayerReadyArray[i] && i != ghostPlayerIndex)
            {
                Vector2 newPosition = PlayerSpriteReferencePointArray[i].transform.position;
                newPosition += Vector2.right * runFromGhostDirection[i] * PlayerRunSpeed * Time.deltaTime;
                PlayerSpriteReferencePointArray[i].transform.position = newPosition;
            }
        }

        Vector2 newGhostPosition = GhostSpriteReferencePointTransform.position;
        newGhostPosition += Vector2.right * runFromGhostDirection[ghostPlayerIndex] * GhostRunSpeed * Time.deltaTime;
        GhostSpriteReferencePointTransform.transform.position = newGhostPosition;
    }
#endregion

#region Debug Functions
    private void debugCheckIfPlayerReady()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isPlayerReadyArray[0] = !isPlayerReadyArray[0];

            if (isPlayerReadyArray[0])
            {
                updateUI_playerReady(0, true);
                debugTextArray[0] = "P1: Ready\n";
                soundManager.SOUND_MAN.playSound("Play_PlayerJoin", gameObject);
            }
            else
            {
                updateUI_playerReady(0, false);
                debugTextArray[0] = "P1: Not Ready\n";
                soundManager.SOUND_MAN.playSound("Play_MenuDown", gameObject);
            }

            updateDebugUI();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isPlayerReadyArray[1] = !isPlayerReadyArray[1];

            if (isPlayerReadyArray[1])
            {
                updateUI_playerReady(1, true);
                debugTextArray[1] = "P2: Ready\n";
                soundManager.SOUND_MAN.playSound("Play_PlayerJoin", gameObject);
            }
            else
            {
                updateUI_playerReady(1, false);
                debugTextArray[1] = "P2: Not Ready\n";
                soundManager.SOUND_MAN.playSound("Play_MenuDown", gameObject);
            }

            updateDebugUI();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            isPlayerReadyArray[2] = !isPlayerReadyArray[2];

            if (isPlayerReadyArray[2])
            {
                updateUI_playerReady(2, true);
                debugTextArray[2] = "P3: Ready\n";
                soundManager.SOUND_MAN.playSound("Play_PlayerJoin", gameObject);
            }
            else
            {
                updateUI_playerReady(2, false);
                debugTextArray[2] = "P3: Not Ready\n";
                soundManager.SOUND_MAN.playSound("Play_MenuDown", gameObject);
            }

            updateDebugUI();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            isPlayerReadyArray[3] = !isPlayerReadyArray[3];

            if (isPlayerReadyArray[3])
            {
                updateUI_playerReady(3, true);
                debugTextArray[3] = "P4: Ready\n";
                soundManager.SOUND_MAN.playSound("Play_PlayerJoin", gameObject);
            }
            else
            {
                updateUI_playerReady(3, false);
                debugTextArray[3] = "P4: Not Ready\n";
                soundManager.SOUND_MAN.playSound("Play_MenuDown", gameObject);
            }

            updateDebugUI();
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
