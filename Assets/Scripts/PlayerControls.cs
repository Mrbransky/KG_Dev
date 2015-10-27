using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

    [HideInInspector]
    public Vector2 direction;
    private Rigidbody2D rigidBody;
    public Sprite HumanSprite, GhostSprite;
    public BoxCollider2D kissCollider;
    public float speed;
    private string EntityID, HorizontalID, VerticalID;
    public bool IsFacingRight;
    public GameManager manager;
    public AudioSource audio;
    public AudioClip[] kisses = new AudioClip[3];
    public GameObject myHearts;
	public RuntimeAnimatorController humanAnimator;
	public RuntimeAnimatorController ghostAnimator;

    bool kissIsPlaying;
    float kissAudioTime;

    public int Health;

    public bool IsDoingKissing;
    
	void Awake () 
    {
        if(this.tag == "Ghost")
        {
            this.HorizontalID = "Horizontal";
            this.VerticalID = "Vertical";
            IsFacingRight = false;
            IsDoingKissing = false;
            this.Health = -5;
            this.GetComponent<SpriteRenderer>().sprite = GhostSprite;
        }

        else if (this.tag == "Human")
        {
            this.HorizontalID = "Horizontal1";
            this.VerticalID = "Vertical1";
            IsFacingRight = false;
            this.Health = 200;
            this.GetComponent<SpriteRenderer>().sprite = HumanSprite;
            //kissCollider.enabled = false;
        }

        rigidBody = this.GetComponent<Rigidbody2D>();
	}
	
	void Update () 
    {
        //Exit
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //Audio Delay
        if(kissIsPlaying)
        {
            kissAudioTime += Time.deltaTime;
            if(kissAudioTime >= .4)
            {
                kissAudioTime = 0;
                kissIsPlaying = false;
            }
        }

        //Player Sprite Check
		if(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("BaseLayer.Ghost_Idle"))
        { 
        	this.GetComponent<SpriteRenderer>().sprite = GhostSprite;
        }
		else if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("BaseLayer.Idle_1"))
		{
			this.GetComponent<SpriteRenderer>().sprite = HumanSprite;
		}
       
        //Kissing Controller
        if(this.name == "Player1" && this.tag == "Ghost")
        {
            kissCollider = this.GetComponent<BoxCollider2D>();
            if (Input.GetButton("GhostA"))
            {
                IsDoingKissing = true;
                
            }               

            else
            {
                IsDoingKissing = false;
            }
        }
        else if (this.name == "Player2" && this.tag == "Ghost")
        {
            kissCollider = this.GetComponent<BoxCollider2D>();
            if (Input.GetButton("HumanA"))
            {
                IsDoingKissing = true;
                
            }

            else
            {
                IsDoingKissing = false;
            }
        }
        
        
        //Direction Controller
        this.direction.x = Input.GetAxis(HorizontalID);
        this.direction.y = Input.GetAxis(VerticalID);

        if (direction.x < 0 && IsFacingRight)
        {
            FlipSprite();
            IsFacingRight = false;
        }

        else if (direction.x > 0 && !IsFacingRight)
        {
            FlipSprite();
            IsFacingRight = true;
        }

        if (direction != Vector2.zero)
        {
            Vector3 calc = new Vector3(direction.x * speed * Time.deltaTime, direction.y * speed * Time.deltaTime, 0);
            this.rigidBody.transform.position += calc;
            if (this.tag == "Human")
            {
				this.GetComponent<Animator>().runtimeAnimatorController = humanAnimator;
                this.GetComponent<Animator>().SetBool("isMoving", true);
            }
			if (this.tag == "Ghost")
			{
				this.GetComponent<Animator>().runtimeAnimatorController = ghostAnimator;
				this.GetComponent<Animator>().SetBool("isMoving", true);
			}
        }
        else
        {
            this.GetComponent<Animator>().SetBool("isMoving", false);
        }
	}

    public void FlipSprite()
    {
        float xScale = this.transform.localScale.x;
        xScale *= -1;
        this.gameObject.transform.localScale = new Vector3(xScale, this.transform.localScale.y);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (this.IsDoingKissing && col.gameObject.tag == "Human")
        {
            if (!kissIsPlaying)
            {
                audio.PlayOneShot(kisses[Random.Range(0, 2)]);
                kissIsPlaying = true;
            }
        }

    }
    void OnTriggerStay2D(Collider2D col)
    {
        if(this.IsDoingKissing && col.gameObject.tag == "Human")
        {
            int HumanHealth = col.GetComponent<PlayerControls>().Health;
            if(HumanHealth >= 0)
            {
                col.GetComponent<PlayerControls>().Health--;
                //HumanHealth = col.GetComponent<PlayerControls>().Health;
                Debug.Log(HumanHealth);
            }

            if (HumanHealth < 0)
                manager.GetComponent<GameManager>().SwapCharacters();
        }

    }


}
