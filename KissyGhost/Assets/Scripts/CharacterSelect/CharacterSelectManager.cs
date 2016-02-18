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
    public Text CharacterSelectDebugText;
    private string[] debugTextArray;
    public List<GameObject> playerScreens;
    public List<GameObject> playerButtons;

    void Start()
    {
        initializeVariables();
        updateUI();

#if UNITY_EDITOR
        Debug.Log("Gamepads connected: " + Input.GetJoystickNames().Length);
        foreach (string joystickName in Input.GetJoystickNames())
        {
            Debug.Log(joystickName);
        }
#endif
    }

    private void initializeVariables()
    {
        isPlayerReadyArray = new bool[MAX_PLAYER_COUNT];
        debugTextArray = new string[MAX_PLAYER_COUNT];

        for (int i = 0; i < MAX_PLAYER_COUNT; ++i)
        {
            isPlayerReadyArray[i] = false;
            debugTextArray[i] = "P" + (i + 1) + ": Not Ready\n";
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Start") && playerCount >= MIN_PLAYER_COUNT_TO_START)
        {
            startGame();
        }

        checkIfPlayerReady();

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space) && playerCount >= MIN_PLAYER_COUNT_TO_START)
        {
            startGame();
        }

        debugCheckIfPlayerReady();
#endif
    }

    private void checkIfPlayerReady()
    {
        for (int i = 1; i <= MAX_PLAYER_COUNT; ++i)
        {
            if (Input.GetButtonDown("P" + i + "pad_A") && !isPlayerReadyArray[i - 1])
            {
                // Player ready
                isPlayerReadyArray[i - 1] = true;
                playerScreens[i - 1].gameObject.SetActive(true);
                playerButtons[i - 1].gameObject.SetActive(false);
                ++playerCount;

                // UI
                debugTextArray[i - 1] = "P" + i + ": Ready\n";
                updateUI();
            }
            else if (Input.GetButtonDown("P" + i + "pad_B") && isPlayerReadyArray[i - 1])
            {
                // Player cancels ready
                isPlayerReadyArray[i - 1] = false;
                playerScreens[i - 1].gameObject.SetActive(false);
                playerButtons[i - 1].gameObject.SetActive(true);
                --playerCount;

                // UI
                debugTextArray[i - 1] = "P" + i + ": Not Ready\n";
                updateUI();
            }
        }
    }

    private void debugCheckIfPlayerReady()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isPlayerReadyArray[0] = !isPlayerReadyArray[0];

            if (isPlayerReadyArray[0])
            {
                ++playerCount;
                debugTextArray[0] = "P1: Ready\n";
                playerScreens[0].gameObject.SetActive(true);
                playerButtons[0].gameObject.SetActive(false);
            }
            else
            {
                --playerCount;
                debugTextArray[0] = "P1: Not Ready\n";
                playerScreens[0].gameObject.SetActive(false);
                playerButtons[0].gameObject.SetActive(true);
            }

            updateUI();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isPlayerReadyArray[1] = !isPlayerReadyArray[1];

            if (isPlayerReadyArray[1])
            {
                ++playerCount;
                debugTextArray[1] = "P2: Ready\n";
                playerScreens[1].gameObject.SetActive(true);
                playerButtons[1].gameObject.SetActive(false);
            }
            else
            {
                --playerCount;
                debugTextArray[1] = "P2: Not Ready\n";
                playerScreens[1].gameObject.SetActive(false);
                playerButtons[1].gameObject.SetActive(true);
            }

            updateUI();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            isPlayerReadyArray[2] = !isPlayerReadyArray[2];

            if (isPlayerReadyArray[2])
            {
                ++playerCount;
                debugTextArray[2] = "P3: Ready\n";
                playerScreens[2].gameObject.SetActive(true);
                playerButtons[2].gameObject.SetActive(false);
            }
            else
            {
                --playerCount;
                debugTextArray[2] = "P3: Not Ready\n";
                playerScreens[2].gameObject.SetActive(false);
                playerButtons[2].gameObject.SetActive(true);
            }

            updateUI();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            isPlayerReadyArray[3] = !isPlayerReadyArray[3];

            if (isPlayerReadyArray[3])
            {
                ++playerCount;
                debugTextArray[3] = "P4: Ready\n";
                playerScreens[3].gameObject.SetActive(true);
                playerButtons[3].gameObject.SetActive(false);
            }
            else
            {
                --playerCount;
                debugTextArray[3] = "P4: Not Ready\n";
                playerScreens[3].gameObject.SetActive(false);
                playerButtons[3].gameObject.SetActive(true);
            }

            updateUI();
        }
    }

    private void updateUI()
    {
        CharacterSelectDebugText.text = "";

        for (int i = 0; i < MAX_PLAYER_COUNT; ++i)
        {
            CharacterSelectDebugText.text += debugTextArray[i];
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
}
