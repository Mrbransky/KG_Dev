using UnityEngine;
using System.Collections;

public class Ghost : Player
{
    public float timeBetweenKisses = 1.5f;
    private float timeSinceKiss;
    private MoveInteractTrigger _MoveInteractTrigger;

    public override void Awake() 
    {
        FacingRight = false;
        base.Awake();

        _MoveInteractTrigger = this.interactTrigger.GetComponent<MoveInteractTrigger>();
	}

    public override void Update()
    {
        if (timeSinceKiss > 0)
        {
            timeSinceKiss -= Time.deltaTime;
        }
        else if (InputMapper.GrabVal(XBOX360_BUTTONS.A, this.playerNum) && canKissObject())
        {
            kissObject();
        }
#region Keyboard Input Related Code (for Debugging)
#if UNITY_EDITOR
        else if (Input.GetKeyDown(KeyCode.M) && canKissObject())
        {
            kissObject();
        }
#endif
#endregion

        base.Update();
	}
    
    private bool canKissObject()
    {
        return (_MoveInteractTrigger.colliderList.Count > 0 && timeSinceKiss <= 0);
    }

    private void kissObject()
    {
        // Don't put kiss on cooldown if the furniture is already kissed
        foreach (Collider2D col in _MoveInteractTrigger.colliderList)
        {
            if (col.GetComponent<KissableFurniture>().KissFurniture())
            {
                timeSinceKiss = timeBetweenKisses;
            }
        }
    }
}
