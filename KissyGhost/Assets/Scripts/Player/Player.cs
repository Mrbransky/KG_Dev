using UnityEngine;
using System.Collections;

public class Player : Entity 
{
    public enum MoveAnim { Idle, Walking, Kicking, notKicking };
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
    private MoveAnim moveAnimation;

    public override Vector2 moveDir
    {
        get { return SetMoveDirection(); }
    }

    public int playerNum;

[Header("Colliders/Triggers")]
    public BoxCollider2D interactTrigger;
    public CircleCollider2D bodyCol;
    public MoveInteractTrigger _MoveInteractTrigger;
    
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
    private Transform playerCanvasTransform;

    public override void Awake() 
    {
        MoveAnimation = MoveAnim.Idle;

        xScale = this.transform.localScale.x;

        interactTrigger = transform.Find("Interact").GetComponent<BoxCollider2D>();
        bodyCol = GetComponent<CircleCollider2D>();
        _MoveInteractTrigger = interactTrigger.GetComponent<MoveInteractTrigger>();

        Canvas playerCanvas = GetComponentInChildren<Canvas>();
        if (playerCanvas != null)
        {
            playerCanvasTransform = playerCanvas.transform;
        }

        base.Awake();
	}
	
	public override void Update () 
    {
        if (moveDir.magnitude >= .25f)
        {           
            SetFaceDirection();

            if (MoveAnimation != MoveAnim.Walking)
                MoveAnimation = MoveAnim.Walking;
        }

        else if (MoveAnimation != MoveAnim.Idle)
            MoveAnimation = MoveAnim.Idle;

#region Keyboard Input Related Code (for Debugging)
#if UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE
        if (moveDir == Vector2.zero)
        {
            DebugPlayerInput();
        }
#endif
#endregion

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
        if (facingRight)
        {
            this.gameObject.transform.localScale =
                new Vector3(-xScale, this.transform.localScale.y);

            if (playerCanvasTransform != null)
            {
                playerCanvasTransform.localScale =
                    new Vector3(-playerCanvasTransform.localScale.x, playerCanvasTransform.localScale.y);
            }
        }
        else
        {
            this.gameObject.transform.localScale =
                new Vector3(xScale, this.transform.localScale.y);

            if (playerCanvasTransform != null)
            {
                playerCanvasTransform.localScale =
                    new Vector3(-playerCanvasTransform.localScale.x, playerCanvasTransform.localScale.y);
            }
        }
    }

    protected void UpdateAnimatorScript(MoveAnim anim)
    {
        this.GetComponent<PlayerAnimator>().UpdateAnimation(anim);
    }

#region Keyboard Input Related Functions (for Debugging)
#if UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE
    protected void DebugPlayerInput()
    {
        #region SetDebugMoveDirection
        debugMoveDir = Vector2.zero;

        switch (playerNum)
        {
            case 1:
                if (Input.GetKey(KeyCode.A))
                {
                    debugMoveDir.x = -1;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    debugMoveDir.x = 1;
                }

                if (Input.GetKey(KeyCode.S))
                {
                    debugMoveDir.y = -1;
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    debugMoveDir.y = 1;
                }
                break;
            case 2:
                if (Input.GetKey(KeyCode.F))
                {
                    debugMoveDir.x = -1;
                }
                else if (Input.GetKey(KeyCode.H))
                {
                    debugMoveDir.x = 1;
                }

                if (Input.GetKey(KeyCode.G))
                {
                    debugMoveDir.y = -1;
                }
                else if (Input.GetKey(KeyCode.T))
                {
                    debugMoveDir.y = 1;
                }
                break;
            case 3:
                if (Input.GetKey(KeyCode.J))
                {
                    debugMoveDir.x = -1;
                }
                else if (Input.GetKey(KeyCode.L))
                {
                    debugMoveDir.x = 1;
                }

                if (Input.GetKey(KeyCode.K))
                {
                    debugMoveDir.y = -1;
                }
                else if (Input.GetKey(KeyCode.I))
                {
                    debugMoveDir.y = 1;
                }
                break;
            case 4:
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    debugMoveDir.x = -1;
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    debugMoveDir.x = 1;
                }

                if (Input.GetKey(KeyCode.DownArrow))
                {
                    debugMoveDir.y = -1;
                }
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    debugMoveDir.y = 1;
                }
                break;
        }
        #endregion

        if (debugMoveDir != Vector2.zero)
        {
            if (debugMoveDir.x > 0) FacingRight = true;
            else if (debugMoveDir.x < 0) FacingRight = false;

            if (MoveAnimation != MoveAnim.Walking)
                MoveAnimation = MoveAnim.Walking;
        }

        else if (MoveAnimation != MoveAnim.Idle)
            MoveAnimation = MoveAnim.Idle;
    }
#endif
#endregion
}
