using UnityEngine;
using System.Collections;

public class Ghost : Player 
{
	public override void Awake() 
    {
        FacingRight = false;
        base.Awake();
	}

    public override void Update() 
    {
        base.Update();
	}
}
