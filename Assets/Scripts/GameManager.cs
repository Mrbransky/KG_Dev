using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject heart_splosion;
	public GameObject heartZoom;
	public bool gameEnd = false;
	public GameObject gameWinner;

    public GameObject gameEndPlayAgain, gameEndMainMenu;

    //For David
    int playerCount = 0;

    GameObject[] currentPlayers;

    void Start()
    {
        currentPlayers = GameObject.FindGameObjectsWithTag("Player");
    }
	void Update () {
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
