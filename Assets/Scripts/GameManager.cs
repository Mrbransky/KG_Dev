using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject Player1;
    public GameObject Player2;
    PlayerControls player1_;
    PlayerControls player2_;
	public GameObject heart_splosion;
	// Update is called once per frame

    void Start()
    {
        player1_ = Player1.GetComponent<PlayerControls>();
        player2_ = Player2.GetComponent<PlayerControls>();
    }
	void Update () {

        if (Player1.tag == "Ghost" && Player2.GetComponent<PlayerControls>().Health <= 0)
        {
            if (Player1.GetComponent<PlayerControls>().IsFacingRight != true)
            {
                Player1.GetComponent<PlayerControls>().IsFacingRight = true;
            }
            SwapCharacters();
            
        }
        if (Player2.tag == "Ghost" && Player1.GetComponent<PlayerControls>().Health <= 0)
        {

            if(Player2.GetComponent<PlayerControls>().IsFacingRight != true)
            {
                Player2.GetComponent<PlayerControls>().IsFacingRight = true;
            }
            SwapCharacters();
        }


	}
    public void SwapCharacters()
    {
        this.GetComponent<RoomGenerator>().GenerateNewRoom();

        if (Player1.tag == "Ghost")
        {
			GameObject hearts = Instantiate(heart_splosion, Player1.transform.position,Quaternion.identity) as GameObject;
			hearts.transform.SetParent(Player1.transform);
            player1_.myHearts.SetActive(false);
            Player1.tag = "Human";
            player1_.Health = 200;
            Player1.GetComponent<SpriteRenderer>().sprite = player1_.HumanSprite;
            player1_.IsDoingKissing = false;
            // kissCollider.enabled = false;
        }
        else
        {
            player1_.myHearts.SetActive(true);
            player1_.tag = "Ghost";
            player1_.Health = 0;
            Player1.GetComponent<SpriteRenderer>().sprite = player1_.GhostSprite;
            player1_.IsDoingKissing = false;
            // kissCollider.enabled = true;
        }

        if (Player2.tag == "Ghost")
        {
			GameObject hearts = Instantiate(heart_splosion, Player2.transform.position,Quaternion.identity) as GameObject;
			hearts.transform.SetParent(Player2.transform);
            player2_.myHearts.SetActive(false);
            Player2.tag = "Human";
            player2_.Health = 200;
            Player2.GetComponent<SpriteRenderer>().sprite = player2_.HumanSprite;
            player2_.IsDoingKissing = false;
            // kissCollider.enabled = false;
        }
        else
        {
            player2_.myHearts.SetActive(true);
            player2_.tag = "Ghost";
            player2_.Health = 0;
            Player2.GetComponent<SpriteRenderer>().sprite = player2_.GhostSprite;
            player2_.IsDoingKissing = false;
            // kissCollider.enabled = true;
        }
    }
}
