using UnityEngine;
using System.Collections;

public class PlayerAI : MonoBehaviour {

    public enum HumanStates { Search, Kick, Dodge, PickUp, Place, FindDoor };
    HumanStates currentState;
    private GameManager gm;
    private RoomGenerator rm;
    private RoomChangeManager rcm;
    public GameObject closestItem;
	void Start () {
        currentState = HumanStates.Search;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        rm = GameObject.Find("GameManager").GetComponent<RoomGenerator>();
        rcm = GameObject.Find("GameManager").GetComponent<RoomChangeManager>();
	}

	void Update () {

        closestItem = FindClosestSpecialItem(rm.AiTracker.ToArray());

	    switch(currentState)
        {
            case HumanStates.Search:
                transform.position = Vector3.MoveTowards(transform.position,closestItem.transform.position,4f*Time.deltaTime);
                break;
            case HumanStates.Kick:
                break;
            case HumanStates.Dodge:
                break;
            case HumanStates.PickUp:
                break;
            case HumanStates.Place:
                break;
        }
	}

    GameObject FindClosestSpecialItem(GameObject[] specItems)
    {
        GameObject nearestTarget = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach(GameObject potentialTarget in specItems)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistance)
            {
                closestDistance = dSqrToTarget;
                nearestTarget = potentialTarget;
            }
        }
        return nearestTarget;
    }
}
