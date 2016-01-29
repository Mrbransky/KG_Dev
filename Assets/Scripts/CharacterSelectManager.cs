using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    }

    private void checkIfPlayerReady()
    {
        for (int i = 1; i <= MAX_PLAYER_COUNT; ++i)
        {
            if (Input.GetButtonDown("P" + i + "pad_A") && !isPlayerReadyArray[i - 1])
            {
                // Player ready
                isPlayerReadyArray[i - 1] = true;
                ++playerCount;

                // UI
                debugTextArray[i - 1] = "P" + i + ": Ready\n";
                updateUI();
            }
            else if (Input.GetButtonDown("P" + i + "pad_B") && isPlayerReadyArray[i - 1])
            {
                // Player cancels ready
                isPlayerReadyArray[i - 1] = false;
                --playerCount;

                // UI
                debugTextArray[i - 1] = "P" + i + ": Not Ready\n";
                updateUI();
            }
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
            characterSelectData.GetComponent<CharacterSelectData>().SetIsPlayerReady(isPlayerReadyArray);
            Application.LoadLevel("CharacterSelectDemo");
        }
        else
        {
            Debug.LogError("CharacterSelectManager: Could not find game object with tag \"CharacterSelectData\"");
        }
    }
}
