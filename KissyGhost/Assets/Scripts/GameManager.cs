using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class GameManager : MonoBehaviour {

	public GameObject heart_splosion;
	public GameObject heartZoom;
	public bool gameEnd = false;
	public GameObject gameWinner;

    public List<GameObject> thingsToTurnOffAtGameEnd;
    public List<GameObject> thingsToTurnOnAtGameEnd;

    public int playerCount = 0;
    public List<GameObject> currentPlayers;
    public GameObject currentGhostPlayer;
    public GameObject ghostPrefab;
    private bool[] isPlayerReadyArray;

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
            isPlayerReadyArray = characterSelectData.GetComponent<CharacterSelectData>().IsPlayerReadyArray;
            playerCount = characterSelectData.GetComponent<CharacterSelectData>().PlayerCount;
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

        int randGhostPlayerIndex = Random.Range(0, playerCount);
        int ghostPlayer_ForLoopCounter = 0;
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
                if (randGhostPlayerIndex == ghostPlayer_ForLoopCounter)
                {
                    ghostPlayer = (GameObject)GameObject.Instantiate(ghostPrefab, players[i].transform.position, players[i].transform.rotation);
                    ghostPlayer.GetComponent<Ghost>().playerNum = players[i].GetComponent<Human>().playerNum;
                    ghostPlayer.gameObject.tag = "Ghost";
                    Camera.main.gameObject.GetComponent<NewCameraBehavior>().targets.Remove(players[i]);
                    Destroy(players[i]);
                }
                else
                {
                    currentPlayers.Add(players[i]);
                }

                ++ghostPlayer_ForLoopCounter;
            }
        }

        currentPlayers.Add(ghostPlayer);
        currentGhostPlayer = ghostPlayer;
        Camera.main.gameObject.GetComponent<NewCameraBehavior>().targets.Add(ghostPlayer);
    }
    
	void Update ()
    {
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
    }

    public void OnGhostWin()
    {
        gameEnd = true;
        int ghostPlayerNum = currentGhostPlayer.GetComponent<Ghost>().playerNum;

        StartCoroutine(InputMapper.Vibration(ghostPlayerNum, .75f, 1, 1));
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
                    Application.LoadLevel(0);
                }

                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey("joystick button 0"))
                {
                    Time.timeScale = 1;
                    Application.LoadLevel(1);
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        CeaseAllVibrations();
    }

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
