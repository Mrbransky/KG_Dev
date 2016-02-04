using UnityEngine;
using System.Collections;

public class Player : Entity 
{
    public enum MoveAnim { Idle, Walking };
    public MoveAnim MoveAnimation
    {
        get { return moveAnimation; }

        set
        {
            if (moveAnimation != value)
            {
                UpdateAnimatorScript(value);
                moveAnimation = value;
            }
        }
    }

    public override Vector2 moveDir
    {
        get { return SetMoveDirection(); }
    }

    private MoveAnim moveAnimation;

    public BoxCollider2D interactTrigger;
    public CircleCollider2D bodyCol;

    public int playerNum;
    public bool FacingRight
    {
        get { return facingRight; }

        set
        {
            if(facingRight != value)
            {
                FlipSprite(value);
                facingRight = value;
            }
        }
    }

    private bool facingRight;
    private float xScale;

    public override void Awake() 
    {
        MoveAnimation = MoveAnim.Idle;

        xScale = this.transform.localScale.x;

        interactTrigger = transform.Find("Interact").GetComponent<BoxCollider2D>();
        bodyCol = GetComponent<CircleCollider2D>();
        base.Awake();
	}
	
	public override void Update () 
    {
        SetMoveDirection();

        if (moveDir != Vector2.zero)
        {           
            SetFaceDirection();

            if (MoveAnimation != MoveAnim.Walking)
                MoveAnimation = MoveAnim.Walking;
        }

        else if (MoveAnimation != MoveAnim.Idle)
            MoveAnimation = MoveAnim.Idle;

        base.Update();     
	}

    private Vector2 SetMoveDirection()
    {
        return new Vector2((InputMapper.GrabVal(XBOX360_AXES.LeftStick_Horiz, this.playerNum)),
                             InputMapper.GrabVal(XBOX360_AXES.LeftStick_Vert, this.playerNum));
    }

    protected void SetFaceDirection()
    {
        if (moveDir.x > 0) FacingRight = true;
        else if (moveDir.x < 0) FacingRight = false;
    }

    protected void FlipSprite(bool facingRight)
    {
        if(facingRight)
            this.gameObject.transform.localScale = 
            new Vector3(-xScale, this.transform.localScale.y);

        else
            this.gameObject.transform.localScale =
            new Vector3(xScale, this.transform.localScale.y);

    }

    protected void UpdateAnimatorScript(MoveAnim anim)
    {
        this.GetComponent<PlayerAnimator>().UpdateAnimation(anim);
    }

}
