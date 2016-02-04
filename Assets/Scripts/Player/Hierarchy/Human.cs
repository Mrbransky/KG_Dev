using UnityEngine;
using System.Collections;

public class Human : Player 
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
