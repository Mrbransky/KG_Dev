using UnityEngine;
using System.Collections.Generic;

public class PullSwitch : MonoBehaviour 
{
    public enum PullState { Idle, BeingPulled, Retracting, FullExtended };
    public PullState State
    {
        get { return state; }
        set
        {
            if (state != value)
                state = OnStateChange(value);
        }
    }
    private PullState state;

    int numPlayersPulling
    {
        get { return players.Count; }
    }

    bool canBeGrabbed;
    public float retractRate, pullRate, maxExtendLength, currentRate;
    float minRetractLength;

    public List<Player> players;

	void Awake () 
    {
        State = PullState.Idle;
        minRetractLength = transform.localScale.x;

        if (minRetractLength > maxExtendLength)
            Debug.Log("Error in PullSwitch obj: MaxLength less than MinLength, check inspector");

        canBeGrabbed = true;

        players = new List<Player>();
	}
	
	void Update () 
    {
        if (State == PullState.BeingPulled || State == PullState.Retracting)
            ScaleSwitchX();
	}

    private void ScaleSwitchX()
    {
        float move = transform.localScale.x + (currentRate * numPlayersPulling * Time.deltaTime);
        transform.localScale.Set(move, transform.localScale.y, transform.localScale.z);            
    }

    public void PlayerGrabbed()
    {

    }

    public void PlayerReleased()
    {

    }

    public PullState OnStateChange(PullState ps)
    {
        switch(ps)
        {
            case PullState.Idle:
                currentRate = 0;
                break;

            case PullState.BeingPulled:
                currentRate = pullRate;
                break;

            case PullState.FullExtended:
                canBeGrabbed = false;
                currentRate = 0;
                break;

            case PullState.Retracting:
                currentRate = retractRate;
                break;
        }

        return ps;
    }
}
