using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveInteractTrigger : MonoBehaviour {

    [HideInInspector]
    public BoxCollider2D InteractTrigger;
    private Vector2 PlayerMoveDir;

#if UNITY_EDITOR
    private Vector2 debugPlayerMoveDir;
#endif

    public float HorizReach, VertReach;
    public float TriggerVertOffset;

    Transform InteractTransform;
    bool PlayerFacingRight;

    public bool isGhostInteractTrigger = false;
    public List<Collider2D> colliderList;
    private SpriteRenderer spriteRenderer;

	void Awake () 
    {
        InteractTransform = this.transform;
        InteractTrigger = this.GetComponent<BoxCollider2D>();

        if (isGhostInteractTrigger)
        {
            colliderList = new List<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
	}
	
	void Update () 
    {
        PlayerMoveDir = this.GetComponentInParent<Player>().moveDir;

#if UNITY_EDITOR
        debugPlayerMoveDir = this.GetComponentInParent<Player>().debugMoveDir;
#endif

        PlayerFacingRight = this.GetComponentInParent<Player>().FacingRight;

        if (PlayerMoveDir.magnitude >= .75)
            SetInteractTrigOffset();

#if UNITY_EDITOR
        else if(debugPlayerMoveDir.magnitude >= .75)
            DEBUG_SetInteractTrigOffset();
#endif

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

    void DEBUG_SetInteractTrigOffset()
    {
        if (PlayerFacingRight) InteractTrigger.offset =
            new Vector2(-debugPlayerMoveDir.x * HorizReach, (debugPlayerMoveDir.y * VertReach) + TriggerVertOffset);

        else InteractTrigger.offset =
            new Vector2(debugPlayerMoveDir.x * HorizReach, (debugPlayerMoveDir.y * VertReach) + TriggerVertOffset);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (isGhostInteractTrigger && col.tag == "Furniture" && !colliderList.Contains(col))
        {
            colliderList.Add(col);
            spriteRenderer.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (isGhostInteractTrigger && col.tag == "Furniture" && colliderList.Contains(col))
        {
            colliderList.Remove(col);
            
            if (colliderList.Count == 0)
            {
                spriteRenderer.enabled = false;
            }
        }
    }
}
