using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveInteractTrigger : MonoBehaviour {

    [HideInInspector]
    public BoxCollider2D InteractTrigger;
    private Vector2 PlayerMoveDir;

#if UNITY_EDITOR || UNITY_WEBGL //|| UNITY_STANDALONE
    private Vector2 debugPlayerMoveDir;
#endif

    public float HorizReach, VertReach;
    public float TriggerVertOffset;

    Transform InteractTransform;
    bool PlayerFacingRight;
    public bool IsOnItemNode;

    public bool isGhostInteractTrigger = false;
    public List<Collider2D> colliderList;
    private SpriteRenderer spriteRenderer_InteractButtonPrompt;

	void Awake () 
    {
        InteractTransform = this.transform;
        InteractTrigger = this.GetComponent<BoxCollider2D>();

        if (isGhostInteractTrigger)
        {
            colliderList = new List<Collider2D>();
            spriteRenderer_InteractButtonPrompt = GetComponentInChildren<SpriteRenderer>();
        }
	}
	
	void Update () 
    {
        PlayerMoveDir = this.GetComponentInParent<Player>().moveDir;
#if UNITY_EDITOR || UNITY_WEBGL //|| UNITY_STANDALONE
        debugPlayerMoveDir = this.GetComponentInParent<Player>().debugMoveDir;
#endif

        PlayerFacingRight = this.GetComponentInParent<Player>().FacingRight;

        if (PlayerMoveDir.magnitude >= .75)
            SetInteractTrigOffset();
#if UNITY_EDITOR || UNITY_WEBGL //|| UNITY_STANDALONE
        else if (debugPlayerMoveDir.magnitude >= .75)
            DEBUG_SetInteractTrigOffset();
#endif

    }

    void SetInteractTrigOffset_Constant()
    {
        if (PlayerFacingRight)
        {
            InteractTrigger.offset = new Vector2(-HorizReach, 0);
        }
        else
        {
            InteractTrigger.offset = new Vector2(-HorizReach, 0);
        }
    }

    void SetInteractTrigOffset()
    {
        if (PlayerFacingRight) InteractTrigger.offset = 
            new Vector2(-PlayerMoveDir.x * HorizReach, (PlayerMoveDir.y * VertReach) + TriggerVertOffset);

        else InteractTrigger.offset = 
            new Vector2(PlayerMoveDir.x * HorizReach, (PlayerMoveDir.y * VertReach) + TriggerVertOffset);
    }
#if UNITY_EDITOR || UNITY_WEBGL //|| UNITY_STANDALONE
    void DEBUG_SetInteractTrigOffset()
    {
        if (PlayerFacingRight) InteractTrigger.offset =
            new Vector2(-debugPlayerMoveDir.x * HorizReach, (debugPlayerMoveDir.y * VertReach) + TriggerVertOffset);

        else InteractTrigger.offset =
            new Vector2(debugPlayerMoveDir.x * HorizReach, (debugPlayerMoveDir.y * VertReach) + TriggerVertOffset);
    }
#endif
    void OnTriggerStay2D(Collider2D col)
    {
        if (isGhostInteractTrigger && col.tag == "Furniture" && !colliderList.Contains(col))
        {
            colliderList.Add(col);
            spriteRenderer_InteractButtonPrompt.enabled = true;
        }

        else if (col.tag == "Cat" && !isGhostInteractTrigger)
        {
            Human player = this.GetComponentInParent<Human>();
            if (player.GetAButtonDown)
            {
                if (player.CanGrabItem)
                    player.GrabItem(col.gameObject);
            }

#if UNITY_EDITOR || UNITY_WEBGL //|| UNITY_STANDALONE
            else if (Input.GetKeyDown(player.ItemPickUpKeycode))
            {
                if (player.CanGrabItem)
                    player.GrabItem(col.gameObject);
            }
#endif
        }

        //else if (col.tag == "Pull")
        //{
        //    Human human = this.GetComponentInParent<Human>();
        //    Debug.Log("WIZ BIZ");
        //    if (InputMapper.GrabVal(XBOX360_BUTTONS.A, human.playerNum) || Input.GetKeyDown(human.ItemPickUpKeycode))
        //    {
        //        human.AttachToPullSwitch(col.gameObject);
        //    }
        //}
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("ItemNode"))
            IsOnItemNode = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (isGhostInteractTrigger && col.tag == "Furniture" && colliderList.Contains(col))
        {
            colliderList.Remove(col);
            
            if (colliderList.Count == 0)
            {
                spriteRenderer_InteractButtonPrompt.enabled = false;
            }
        }

        if (col.gameObject.name.Contains("ItemNode"))
            IsOnItemNode = false;
    }
}
