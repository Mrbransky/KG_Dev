using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScores : MonoBehaviour {

    public Image scoreBar1;
    public Image scoreBar2;

    Color textColor;
    private Vector2 textPosition;
	bool isPlaying = true;

    private int player1Score;
    private int player2Score;

    private float TimeAmtAsHuman1;
    private float TimeAmtAsHuman2;

	void Start () 
    {
       
        if (this.gameObject.tag == "Human")
        {
            //scoreText.color = Color.green;
        }
        else if (this.gameObject.tag == "Ghost")
        {
            //scoreText2.color = Color.magenta;
        }
	}
	
	void Update () 
    {
        if (this.gameObject.tag == "Human" && this.gameObject.name == "Player1" && isPlaying == true)
        {
            TimeAmtAsHuman1 += Time.deltaTime ;
            player1Score = (int)TimeAmtAsHuman1 * 5;
            scoreBar1.transform.localScale += new Vector3(0.02f, 0, 0)*Time.deltaTime;

        }
        if (this.gameObject.tag == "Human" && this.gameObject.name == "Player2" && isPlaying == true)
        {
            TimeAmtAsHuman2 += Time.deltaTime;
            player2Score = (int)TimeAmtAsHuman2 * 5;
            scoreBar2.transform.localScale += new Vector3(0.02f, 0, 0) * Time.deltaTime;
        }

		//Win condition!
		if (scoreBar1.transform.localScale.x >= 1f && this.gameObject.name == "Player1") {
			isPlaying = false;
			GameObject gm = GameObject.Find("GameManager");
			gm.GetComponent<GameManager>().gameWinner = this.gameObject;
			gm.GetComponent<GameManager>().gameEnd = true;
		}
		if (scoreBar2.transform.localScale.x >= 1f && this.gameObject.name == "Player2") {
			isPlaying = false;
			GameObject gm = GameObject.Find("GameManager");
			gm.GetComponent<GameManager>().gameWinner = this.gameObject;
			gm.GetComponent<GameManager>().gameEnd = true;
		}
        
	}
}
