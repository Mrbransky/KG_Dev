using UnityEngine;
using System.Collections;

public class MoveInteractTrigger : MonoBehaviour {

    [HideInInspector]
    public BoxCollider2D InteractTrigger;
    private Vector2 PlayerMoveDir;

    public float HorizReach, VertReach;
    public float TriggerVertOffset;

    Transform InteractTransform;
    bool PlayerFacingRight;

	void Awake () 
    {
        InteractTransform = this.transform;
        InteractTrigger = this.GetComponent<BoxCollider2D>();
	}
	
	void Update () 
    {
        PlayerMoveDir = this.GetComponentInParent<Player>().moveDir;
        PlayerFacingRight = this.GetComponentInParent<Player>().FacingRight;

        if(PlayerMoveDir.magnitude >= .75)
            SetInteractTrigOffset();
	}

    //Old Method for moving sprite centered in the middle of InteractTrigger
    //void SetCrossHairPos()
    //{
    //    if (GhostDirection == Vector2.zero)
    //        this.transform.Find("Crosshair").GetComponent<SpriteRenderer>().enabled = false;
    //    else
    //    {
    //        if (!this.transform.Find("Crosshair").GetComponent<SpriteRenderer>().enabled)
    //        {
    //            this.transform.Find("Crosshair").GetComponent<SpriteRenderer>().enabled = true;
    //        }
    //    }

    //    if (IsFacingRight)
    //        crosshair.localPosition = new Vector3(-GhostDirection.x, GhostDirection.y, 0);

    //    else
    //        crosshair.localPosition = new Vector3(GhostDirection.x, GhostDirection.y, 0);
    //}

    void SetInteractTrigOffset()
    {
        if (PlayerFacingRight) InteractTrigger.offset = 
            new Vector2(-PlayerMoveDir.x * HorizReach, (PlayerMoveDir.y * VertReach) + TriggerVertOffset);

        else InteractTrigger.offset = 
            new Vector2(PlayerMoveDir.x * HorizReach, (PlayerMoveDir.y * VertReach) + TriggerVertOffset);
    }
}
