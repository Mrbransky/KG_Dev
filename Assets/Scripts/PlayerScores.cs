using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScores : MonoBehaviour {

    public Text scoreText;
    public Text scoreText2;

    Color textColor;
    private Vector2 textPosition;

    private int player1Score;
    private int player2Score;

    private float TimeAmtAsHuman1;
    private float TimeAmtAsHuman2;

	void Start () 
    {
       
        if (this.gameObject.name == "Player1")
        {
            scoreText.color = Color.green;
        }
        else if (this.gameObject.name == "Player2")
        {
            scoreText2.color = Color.magenta;
        }
	}
	
	void Update () 
    {
        if (this.gameObject.tag == "Human" && this.gameObject.name == "Player1")
        {
            TimeAmtAsHuman1 += Time.deltaTime;
            player1Score = (int)TimeAmtAsHuman1 * 5;

        }
        if (this.gameObject.tag == "Human" && this.gameObject.name == "Player2")
        {
            TimeAmtAsHuman2 += Time.deltaTime;
            player2Score = (int)TimeAmtAsHuman2 * 5;
        }

        if(this.gameObject.name == "Player1") 
            scoreText.text = player1Score.ToString();

        if(this.gameObject.name == "Player2") 
            scoreText2.text = player2Score.ToString();

        //scoreText.text = score.ToString();
        
	}
}
