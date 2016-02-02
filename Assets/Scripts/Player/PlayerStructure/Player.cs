using UnityEngine;
using System.Collections;

public class Player : Entity 
{
    public BoxCollider2D interactTrigger;
    public CircleCollider2D bodyCol;

    public int playerNum;
    protected bool FacingRight;

    public override void Awake() 
    {
        interactTrigger = transform.Find("Interact").GetComponent<BoxCollider2D>();
        bodyCol = GetComponent<CircleCollider2D>();
        base.Awake();
	}
	
	public override void Update () 
    {
        SetMoveDirection();    
        base.Update();     
	}

    private void SetMoveDirection()
    {
        moveDir = new Vector2((InputMapper.GrabVal(XBOX360_AXES.LeftStick_Horiz, this.playerNum)),
                             InputMapper.GrabVal(XBOX360_AXES.LeftStick_Vert, this.playerNum));
    }

    protected void SetFaceDirection()
    {
        if (this.transform.localScale.x == 1)
            FacingRight = false;

        else if (this.transform.localScale.x == -1)
            FacingRight = true;
    }
}
