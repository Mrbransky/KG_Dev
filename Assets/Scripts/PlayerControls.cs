using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerControls : MonoBehaviour {

    [HideInInspector]
    public Vector2 direction;
    private Rigidbody2D rigidBody;
    public Sprite HumanSprite, GhostSprite;
    public BoxCollider2D kissCollider;

    public float speed;
    private string EntityID, HorizontalID, VerticalID, ActionID;
    public bool IsFacingRight;
    public float dodgeRollForce;

    public GameManager manager;
    public AudioSource audio;
    public AudioClip[] kisses = new AudioClip[3];
    public GameObject myHearts;
	public RuntimeAnimatorController humanAnimator;
	public RuntimeAnimatorController ghostAnimator;

    int numGamePads
    {
        get
        {
            int numControllers = 0;
            foreach(string s in controllers)
            {
                if (s == "Controller (XBOX 360 For Windows)")
                    numControllers++;
            }
            return numControllers;
        }
    }

    string[] controllers
    {
        get { return Input.GetJoystickNames(); }
    }

    string[] cachedControllerList;

    bool kissIsPlaying;
    float kissAudioTime;

    public int Health;

    public bool IsDoingKissing;
    private bool HasDodgeRolled = false;
    public float dodgeRollCoolDown;
    public float dodgeRollTimeAmt;
    
	void Awake () 
    {
        //Player1
        if (this.tag == "Human")
        {
            IsFacingRight = false;
            this.Health = 200;
            this.GetComponent<SpriteRenderer>().sprite = HumanSprite;
        }

        //Player2
        else if(this.tag == "Ghost")
        {
            IsFacingRight = false;
            IsDoingKissing = false;
            this.Health = -5;
            this.GetComponent<SpriteRenderer>().sprite = GhostSprite;
        }

        SetControls();
        rigidBody = this.GetComponent<Rigidbody2D>();
	}
	
	void Update () 
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        DelaySmoochAudio();

        //Player Sprite Check
		if(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("BaseLayer.Ghost_Idle"))
        	this.GetComponent<SpriteRenderer>().sprite = GhostSprite;

		else if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("BaseLayer.Idle_1"))
			this.GetComponent<SpriteRenderer>().sprite = HumanSprite;

        SetFaceDirection();

        if (ShouldFlipSprite())
            FlipSprite();

        HandleKissButton();

        //CLEAN THIS UP ._.
        #region DodgeRollCode
        if (!HasDodgeRolled)
            HandleDirectionInput();

        HandleDodgeRoll();

        if (HasDodgeRolled)
        {
            if (dodgeRollTimeAmt > 0)
                dodgeRollTimeAmt -= Time.deltaTime;
        }

        if(dodgeRollCoolDown > 0)
            dodgeRollCoolDown -= Time.deltaTime;
        #endregion

        //If the controller list changes, change player inputs accordingly
        if (!cachedControllerList.SequenceEqual(controllers))
            SetControls();
	}

    public void FlipSprite()
    {
        float xScale = this.transform.localScale.x;
        xScale *= -1;
        this.gameObject.transform.localScale = new Vector3(xScale, this.transform.localScale.y);
        IsFacingRight = !IsFacingRight;
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
        if (this.IsDoingKissing && col.gameObject.tag == "Human")
        {
            int HumanHealth = col.GetComponent<PlayerControls>().Health;
            if (HumanHealth >= 0)
            {
                col.GetComponent<PlayerControls>().Health--;
                Debug.Log(HumanHealth);
            }

            if (HumanHealth < 0)
                manager.GetComponent<GameManager>().SwapCharacters();
        }
    }

    void DelaySmoochAudio()
    {
        if (kissIsPlaying)
        {
            kissAudioTime += Time.deltaTime;
            if (kissAudioTime >= .4)
            {
                kissAudioTime = 0;
                kissIsPlaying = false;
            }
        }
    }

    void HandleKissButton()
    {
        if(this.tag == "Ghost")
        {
            kissCollider = this.GetComponent<BoxCollider2D>();
            if (Input.GetButton(ActionID))
                IsDoingKissing = true;

            else IsDoingKissing = false;
        }
    }

    //THIS IS WHERE DODGE ROLLING HAPPENS, JACK
    void HandleDodgeRoll()
    {
        if (this.tag == "Human" && !HasDodgeRolled && Input.GetButton(ActionID) && dodgeRollCoolDown <= 0 )
        {
            Vector2 dir = direction.normalized;
            rigidBody.AddForce(new Vector2(dir.x * dodgeRollForce, dir.y * dodgeRollForce));
            HasDodgeRolled = true;
            dodgeRollTimeAmt = .35f;
            Debug.Log("DODGE ROLLED");
        }

        else if (HasDodgeRolled && dodgeRollTimeAmt <= 0)
        {
            rigidBody.velocity = Vector2.zero;
            HasDodgeRolled = false;
            dodgeRollCoolDown = 3;
        }
    }

    void SetAnimation()
    {
        if (this.tag == "Human")
        {
            this.GetComponent<Animator>().runtimeAnimatorController = humanAnimator;
            this.GetComponent<Animator>().SetBool("isMoving", true);
        }

        else if (this.tag == "Ghost")
        {
            this.GetComponent<Animator>().runtimeAnimatorController = ghostAnimator;
            this.GetComponent<Animator>().SetBool("isMoving", true);
        }
    }

    bool ShouldFlipSprite()
    {
        if ((direction.x < 0 && IsFacingRight) || (direction.x > 0 && !IsFacingRight))
            return true;
        else
            return false;
    }

    void ApplyMovement()
    {
        Vector3 calc = new Vector3(direction.x * speed * Time.deltaTime, direction.y * speed * Time.deltaTime, 0);
        this.rigidBody.transform.position += calc;
    }

    void HandleDirectionInput()
    {
        this.direction.x = Input.GetAxis(HorizontalID);
        this.direction.y = Input.GetAxis(VerticalID);

        if (direction != Vector2.zero)
        {
            ApplyMovement();
            SetAnimation();
        }
        else
            this.GetComponent<Animator>().SetBool("isMoving", false);
    }

    void SetFaceDirection()
    {
        if (this.transform.localScale.x == 1)
            IsFacingRight = false;

        else if (this.transform.localScale.x == -1)
            IsFacingRight = true;
    }

    void SetControls()
    {
        if (this.name == "Player1")
        {
            if (controllers.Length > 0 && controllers[0] == "Controller (XBOX 360 For Windows)")
            {
                this.HorizontalID = "P1pad_Horiz";
                this.VerticalID = "P1pad_Vert";
                this.ActionID = "P1pad_A";
            }

            else
            {
                this.HorizontalID = "P1keys_Horiz";
                this.VerticalID = "P1keys_Vert";
                this.ActionID = "P1keys_Action";
            }
        }

        else if (this.name == "Player2")
        {
            if (controllers.Length > 1 && controllers[1] == "Controller (XBOX 360 For Windows)")
            {
                this.HorizontalID = "P2pad_Horiz";
                this.VerticalID = "P2pad_Vert";
                this.ActionID = "P2pad_A";
            }

            else
            {
                this.HorizontalID = "P2keys_Horiz";
                this.VerticalID = "P2keys_Vert";
                this.ActionID = "P2keys_Action";
            }
        }
        cachedControllerList = controllers;
    }
}
