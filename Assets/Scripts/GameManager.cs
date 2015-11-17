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

    public GameObject gameEndPlayAgain, gameEndMainMenu;

    public float GhostSpeed, HumanSpeed;
	// Update is called once per frame

    void Start()
    {
        player1_ = Player1.GetComponent<PlayerControls>();
        player2_ = Player2.GetComponent<PlayerControls>();

        player1_.currentSpeed = HumanSpeed;
        player2_.currentSpeed = GhostSpeed;

        Player1.transform.Find("Hearts").GetComponent<ParticleSystem>().enableEmission = false;
        Player2.transform.Find("Hearts").GetComponent<ParticleSystem>().enableEmission = true;
    }
	void Update () {
        //win condition
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
            GhostToHuman(Player1, player1_);

            //GameObject hearts = Instantiate(heart_splosion, Player1.transform.position,Quaternion.identity) as GameObject;
            //hearts.transform.SetParent(Player1.transform);
            //player1_.myHearts.SetActive(false);
            //Player1.tag = "Human";
            //Player1.transform.FindChild("Collider1").gameObject.layer = 8;
            //Player1.transform.Find("Collider1").tag = "HumanCollider";
            //Player1.transform.Find("Crosshair").GetComponent<SpriteRenderer>().enabled = false;
            //Player1.GetComponent<BoxCollider2D>().enabled = false;
            //player1_.Health = 100;
            //Player1.GetComponent<SpriteRenderer>().sprite = player1_.HumanSprite;           
            //player1_.IsDoingKissing = false;
            //Player1.GetComponent<PlayerControls>().SetAnimation();
            //Player1.GetComponent<PlayerControls>().currentSpeed = HumanSpeed;
            //player1_.currentSpeed = HumanSpeed; player1_.SetBaseSpeed(HumanSpeed);

            // kissCollider.enabled = false;
        }
        else
        {
            HumanToGhost(Player1, player1_);

            //player1_.myHearts.SetActive(true);
            //Player1.tag = "Ghost";
            //Player1.transform.FindChild("Collider1").gameObject.layer= 9;
            //Player1.transform.Find("Collider1").tag = "Untagged";
            //player1_.Health = 0;
            //Player1.GetComponent<SpriteRenderer>().sprite = player1_.GhostSprite;
            //Player1.transform.Find("Crosshair").GetComponent<SpriteRenderer>().enabled = true;
            //Player1.GetComponent<BoxCollider2D>().enabled = true;
            //player1_.IsDoingKissing = false;
            //player1_.SetAnimation();
            //player1_.currentSpeed = GhostSpeed;
            //player1_.currentSpeed = GhostSpeed; player1_.SetBaseSpeed(GhostSpeed);
            // kissCollider.enabled = true;
        }

        if (Player2.tag == "Ghost")
        {
            
            GhostToHuman(Player2, player2_);

            //GameObject hearts = Instantiate(heart_splosion, Player2.transform.position,Quaternion.identity) as GameObject;
            //hearts.transform.SetParent(Player2.transform);
            //player2_.myHearts.SetActive(false);
            //Player2.tag = "Human";
            //Player2.transform.FindChild("Collider2").gameObject.layer = 8;
            //Player2.transform.Find("Crosshair").GetComponent<SpriteRenderer>().enabled = false;
            //Player2.GetComponent<BoxCollider2D>().enabled = false;
            //Player2.transform.Find("Collider2").tag = "HumanCollider";
            //player2_.Health = 100;
            //Player2.GetComponent<SpriteRenderer>().sprite = player2_.HumanSprite;
            //player2_.IsDoingKissing = false;
            //player2_.SetAnimation();
            //player2_.currentSpeed = HumanSpeed; player2_.SetBaseSpeed(HumanSpeed);
            // kissCollider.enabled = false;
        }
        else
        {
            HumanToGhost(Player2, player2_);

            //player2_.myHearts.SetActive(true);
            //Player2.tag = "Ghost";
            //Player2.transform.FindChild("Collider2").gameObject.layer= 9;
            //player2_.Health = 0;
            //Player2.GetComponent<SpriteRenderer>().sprite = player2_.GhostSprite;
            //Player2.GetComponent<BoxCollider2D>().enabled = true;
            //Player2.transform.Find("Crosshair").GetComponent<SpriteRenderer>().enabled = true;
            //Player2.transform.Find("Collider2").tag = "Untagged";
            //player2_.IsDoingKissing = false;
            //Player2.GetComponent<PlayerControls>().SetAnimation();
            //Player2.GetComponent<PlayerControls>().currentSpeed = GhostSpeed;
            //player2_.SetAnimation();
            //player2_.currentSpeed = GhostSpeed; player2_.SetBaseSpeed(GhostSpeed);
            // kissCollider.enabled = true;
        }
    }

    void GhostToHuman(GameObject obj, PlayerControls controls)
    {
        if(obj == Player1)
        {
            obj.transform.FindChild("Collider1").gameObject.layer = 8;
            obj.transform.Find("Collider1").tag = "HumanCollider";
        }

        else if(obj == Player2)
        {
            obj.transform.FindChild("Collider2").gameObject.layer = 8;
            obj.transform.Find("Collider2").tag = "HumanCollider";
        }

        GameObject hearts = Instantiate(heart_splosion, obj.transform.position, Quaternion.identity) as GameObject;
        hearts.transform.SetParent(obj.transform);

        obj.tag = "Human";
        obj.transform.Find("Crosshair").GetComponent<SpriteRenderer>().enabled = false;
        obj.GetComponent<BoxCollider2D>().enabled = false;
        obj.GetComponent<SpriteRenderer>().sprite = controls.HumanSprite;

        obj.transform.Find("Hearts").GetComponent<ParticleSystem>().enableEmission = false;

        //controls.myHearts.SetActive(false);
        controls.Health = 100;
        controls.IsDoingKissing = false;
        controls.SetAnimation();
        controls.currentSpeed = HumanSpeed; 
        controls.SetBaseSpeed(HumanSpeed);
    }

    void HumanToGhost(GameObject obj, PlayerControls controls)
    {
        if (obj == Player1)
        {
            obj.transform.FindChild("Collider1").gameObject.layer = 9;
            obj.transform.Find("Collider1").tag = "Untagged";
        }

        else if (obj == Player2)
        {
            obj.transform.FindChild("Collider2").gameObject.layer = 9;
            obj.transform.Find("Collider2").tag = "Untagged";
        }

        obj.tag = "Ghost";
        obj.GetComponent<SpriteRenderer>().sprite = controls.GhostSprite;
        obj.GetComponent<BoxCollider2D>().enabled = true;
        obj.transform.Find("Crosshair").GetComponent<SpriteRenderer>().enabled = true;

        obj.transform.Find("Hearts").GetComponent<ParticleSystem>().enableEmission = true;

        //controls.myHearts.SetActive(true);
        controls.Health = 0;
        controls.IsDoingKissing = false;
        controls.SetAnimation();
        controls.currentSpeed = GhostSpeed; 
        controls.SetBaseSpeed(GhostSpeed);
    }
}
