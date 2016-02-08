using UnityEngine;
using System.Collections;

public class Human : Player 
{
    public bool IsCarryingItem;

    public override void Awake() 
    {
        FacingRight = false;
        
        //Check if user has pressed A
        //Create function to resolve that
        //Right now, just keeping it strictly to grabbing and putting down

        if (IsCarryingItem && InputMapper.GrabVal(XBOX360_BUTTONS.A, this.playerNum))
            PutItemDown();
        

        base.Awake();
	}

    public override void Update() 
    {
        base.Update();
	}

    void GrabItem()
    {
        this.IsCarryingItem = true;
        //Move object ABOVE the player
        //make them a child of this object
    }
    
    void PutItemDown()
    {
        this.IsCarryingItem = false;
        //Move object to center of InteractTrigger
        //make it NO LONGER a child of said object
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (this.interactTrigger.IsTouching(col) && InputMapper.GrabVal(XBOX360_BUTTONS.A, this.playerNum))
        {
            if (!IsCarryingItem)
                GrabItem();
        }
    }


}
