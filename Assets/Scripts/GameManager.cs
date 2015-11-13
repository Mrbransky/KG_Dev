using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject Player1;
    public GameObject Player2;
    PlayerControls player1_;
    PlayerControls player2_;
	public GameObject heart_splosion;
	public GameObject heartZoom;
	public bool gameEnd = false;
	public GameObject gameWinner;
	// Update is called once per frame

    void Start()
    {
        player1_ = Player1.GetComponent<PlayerControls>();
        player2_ = Player2.GetComponent<PlayerControls>();
    }
	void Update () {
		if (gameEnd == false) {
			heartZoom.transform.position = Vector3.Lerp (Player1.transform.position, Player2.transform.position, 0.5f);
			if (heartZoom.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime > 1) {
				heartZoom.SetActive (false);
			}
		}
		else
		{
			heartZoom.SetActive(true);
            heartZoom.transform.position = gameWinner.transform.position;
			if (heartZoom.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).normalizedTime > 0.5f) {
				heartZoom.SetActive (false);
			}
		}

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
		heartZoom.SetActive (true);

        if (Player1.tag == "Ghost")
        {
			GameObject hearts = Instantiate(heart_splosion, Player1.transform.position,Quaternion.identity) as GameObject;
			hearts.transform.SetParent(Player1.transform);
            player1_.myHearts.SetActive(false);
            Player1.tag = "Human";
			Player1.transform.FindChild("Collider1").gameObject.layer = 8;
            player1_.Health = 200;
            Player1.GetComponent<SpriteRenderer>().sprite = player1_.HumanSprite;
            player1_.IsDoingKissing = false;
            // kissCollider.enabled = false;
        }
        else
        {
            player1_.myHearts.SetActive(true);
            Player1.tag = "Ghost";
			Player1.transform.FindChild("Collider1").gameObject.layer= 9;
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
			Player2.transform.FindChild("Collider2").gameObject.layer = 8;
            player2_.Health = 200;
            Player2.GetComponent<SpriteRenderer>().sprite = player2_.HumanSprite;
            player2_.IsDoingKissing = false;
            // kissCollider.enabled = false;
        }
        else
        {
            player2_.myHearts.SetActive(true);
            Player2.tag = "Ghost";
			Player2.transform.FindChild("Collider2").gameObject.layer= 9;
            player2_.Health = 0;
            Player2.GetComponent<SpriteRenderer>().sprite = player2_.GhostSprite;
            player2_.IsDoingKissing = false;
            // kissCollider.enabled = true;
        }
    }
}
