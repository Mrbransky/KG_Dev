using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject heart_splosion;
	public GameObject heartZoom;
	public bool gameEnd = false;
	public GameObject gameWinner;

    public GameObject gameEndPlayAgain, gameEndMainMenu;

    public int playerCount = 0;
    public List<GameObject> currentPlayers;
    private bool[] isPlayerReadyArray;

    void Start()
    {
        handleCharacterSelectData();
        initializePlayers();
        assignGhost();
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

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            int playerReadyArrayIndex = player.GetComponent<Human>().playerNum - 1;

            if (!isPlayerReadyArray[playerReadyArrayIndex])
            {
                player.SetActive(false);
            }
            else
            {
                currentPlayers.Add(player);
            }
        }
    }

    private void assignGhost()
    {
        int randPlayerIndex = Random.Range(0, playerCount - 1);

        // TODO Set player to ghost here using randPlayerIndex
    }

	void Update ()
    {
        //win condition
		if (gameEnd == false) {
            heartZoom.transform.position = Camera.main.transform.position;
			if (heartZoom.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime > 1) {
				heartZoom.SetActive (false);
			}
		}
		else
		{
			heartZoom.SetActive(true);
            //heartZoom.transform.position = gameWinner.transform.position;
            heartZoom.GetComponent<Animator>().SetBool("gameEndTrig", true);
            if (heartZoom.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                gameEndPlayAgain.SetActive(true);
                gameEndMainMenu.SetActive(true);
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

}
