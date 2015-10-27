using UnityEngine;
using System.Collections;

public class GhostKiss : MonoBehaviour {

    public Vector2 GhostDirection;
    public BoxCollider2D kissCollider;
	
	void Update () 
    {
        GhostDirection = this.GetComponent<PlayerControls>().direction;

        if (this.GetComponent<PlayerControls>().IsFacingRight == false)
            kissCollider.offset = new Vector2(GhostDirection.x, GhostDirection.y);
        else
            kissCollider.offset = new Vector2(-GhostDirection.x, GhostDirection.y);
	}

}
