using UnityEngine;
using System.Collections;

public class GhostKiss : MonoBehaviour {

    public Vector2 GhostDirection;
    public BoxCollider2D KissCollider;

    Transform crosshair;
    bool IsFacingRight;

    void Awake()
    {
        crosshair = this.transform.Find("Crosshair").GetComponent<Transform>();
    }

	void Update () 
    {
        GhostDirection = this.GetComponent<PlayerControls>().direction;
        IsFacingRight = this.GetComponent<PlayerControls>().IsFacingRight;

        if(this.tag == "Ghost")
        {
            SetKissColliderOffset();
            SetCrossHairPos();
        }

	}

    void SetCrossHairPos()
    {
        if (GhostDirection == Vector2.zero)
            this.transform.Find("Crosshair").GetComponent<SpriteRenderer>().enabled = false;
        else
        {
            if (!this.transform.Find("Crosshair").GetComponent<SpriteRenderer>().enabled)
            {
                this.transform.Find("Crosshair").GetComponent<SpriteRenderer>().enabled = true;
            }
        }

        if (IsFacingRight)
            crosshair.localPosition = new Vector3(-GhostDirection.x, GhostDirection.y, 0); 

        else
            crosshair.localPosition = new Vector3(GhostDirection.x, GhostDirection.y, 0);
    }

    void SetKissColliderOffset()
    {
        if (IsFacingRight) 
            KissCollider.offset = new Vector2(-GhostDirection.x, GhostDirection.y);

        else
            KissCollider.offset = new Vector2(GhostDirection.x, GhostDirection.y);
    }

}
