using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterSelectManager : MonoBehaviour
{
    // Constants
    private const int MAX_PLAYER_COUNT = 4;
    private const int MIN_PLAYER_COUNT_TO_START = 2;

    // General
    private bool[] isPlayerReadyArray;
    private int playerCount = 0;

    // UI
    public SpriteRenderer[] PlayerSpriteRendererArray;
    public Text[] ReadyTextArray;
    public Image[] ButtonImageArray;
    public GameObject PressToStartTextObject;

    private Text[] buttonTextArray;
    private Color transparentColor;

    // Debug UI
    public Text CharacterSelectDebugText;
    private string[] debugTextArray;

    void Start()
    {
        initializeVariables();

        #region Debug Code
#if UNITY_EDITOR
        updateDebugUI();

        Debug.Log("Gamepads connected: " + Input.GetJoystickNames().Length);
        foreach (string joystickName in Input.GetJoystickNames())
        {
            Debug.Log(joystickName);
        }
#endif
        #endregion
    }

    private void initializeVariables()
    {
        isPlayerReadyArray = new bool[MAX_PLAYER_COUNT];
        buttonTextArray = new Text[MAX_PLAYER_COUNT];

        for (int i = 0; i < MAX_PLAYER_COUNT; ++i)
        {
            isPlayerReadyArray[i] = false;
            buttonTextArray[i] = ButtonImageArray[i].GetComponentInChildren<Text>();
        }

        transparentColor = new Color(0, 0, 0, 0);

        #region Debug Code
#if UNITY_EDITOR
        debugTextArray = new string[MAX_PLAYER_COUNT];

        for (int i = 0; i < MAX_PLAYER_COUNT; ++i)
        {
            debugTextArray[i] = "P" + (i + 1) + ": Not Ready\n";
        }
#endif
        #endregion
    }

    void Update()
    {
        if (Input.GetButtonDown("Start") && playerCount >= MIN_PLAYER_COUNT_TO_START)
        {
            startGame();
        }

        checkIfPlayerReady();

        #region Debug Code
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space) && playerCount >= MIN_PLAYER_COUNT_TO_START)
        {
            startGame();
        }

        debugCheckIfPlayerReady();
#endif
        #endregion
    }

    private void checkIfPlayerReady()
    {
        for (int i = 1; i <= MAX_PLAYER_COUNT; ++i)
        {
            if (Input.GetButtonDown("P" + i + "pad_A") && !isPlayerReadyArray[i - 1])
            {
                updateUI_playerReady(i - 1, true);

                #region Debug Code
#if UNITY_EDITOR
                debugTextArray[i - 1] = "P" + i + ": Ready\n";
                updateDebugUI();
#endif
                #endregion
            }
            else if (Input.GetButtonDown("P" + i + "pad_B") && isPlayerReadyArray[i - 1])
            {
                updateUI_playerReady(i - 1, false);

                #region Debug Code
#if UNITY_EDITOR
                debugTextArray[i - 1] = "P" + i + ": Ready\n";
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
            buttonTextArray[playerIndex].color = transparentColor;

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
            buttonTextArray[playerIndex].color = Color.black;

            if (playerCount < MIN_PLAYER_COUNT_TO_START)
            {
                PressToStartTextObject.SetActive(false);
            }
        }
    }

    private void startGame()
    {
        Debug.Log("Starting game...");
        GameObject characterSelectData = GameObject.FindGameObjectWithTag("CharacterSelectData");
        if (characterSelectData != null)
        {
            characterSelectData.GetComponent<CharacterSelectData>().SetIsPlayerReady(isPlayerReadyArray, playerCount);
            Application.LoadLevel(Application.loadedLevel + 1);
        }
        else
        {
            Debug.LogError("CharacterSelectManager: Could not find game object with tag \"CharacterSelectData\"");
        }
    }

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
            }
            else
            {
                updateUI_playerReady(0, false);
                debugTextArray[0] = "P1: Not Ready\n";
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
            }
            else
            {
                updateUI_playerReady(1, false);
                debugTextArray[1] = "P2: Not Ready\n";
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
            }
            else
            {
                updateUI_playerReady(2, false);
                debugTextArray[2] = "P3: Not Ready\n";
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
            }
            else
            {
                updateUI_playerReady(3, false);
                debugTextArray[3] = "P4: Not Ready\n";
            }

            updateDebugUI();
        }
    }

    private void updateDebugUI()
    {
        CharacterSelectDebugText.text = "";

        for (int i = 0; i < MAX_PLAYER_COUNT; ++i)
        {
            CharacterSelectDebugText.text += debugTextArray[i];
        }
    }
    #endregion
}
