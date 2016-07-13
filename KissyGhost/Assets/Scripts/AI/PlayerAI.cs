using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class PlayerAI : MonoBehaviour {

    public enum HumanStates { Search, Kick, Dodge, PickUp, Place, FindDoor };
    public HumanStates currentState;
    private GameManager gm;
    private RoomGenerator rm;
    private RoomChangeManager rcm;

    public  List<GameObject>SpecialItems;
    private List<GameObject> ItemGoalPlacement;
    public GameObject closestItem;
    private GameObject itemToPickUp;
    void Start () {
        currentState = HumanStates.Search;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        rm = GameObject.Find("GameManager").GetComponent<RoomGenerator>();
        rcm = GameObject.Find("GameManager").GetComponent<RoomChangeManager>();

        SpecialItems = rm.AiTracker;
        ItemGoalPlacement = GameObject.FindGameObjectsWithTag("Heartagram").ToList();
	}

	void Update () {

        closestItem = FindClosestSpecialItem(SpecialItems);

	    switch(currentState)
        {
            case HumanStates.Search:
                if(closestItem != null)
                transform.position = Vector3.MoveTowards(transform.position,closestItem.transform.position,4f*Time.deltaTime);
                break;
            case HumanStates.Kick:
                break;
            case HumanStates.Dodge:
                break;
            case HumanStates.PickUp:
                PickUpSpecialItem(itemToPickUp);
                break;
            case HumanStates.Place:
                GoToPlaceItem(itemToPickUp);
                break;
        }
	}

    GameObject FindClosestSpecialItem(List<GameObject> specItems)
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
            if(directionToTarget == Vector3.zero) {
                itemToPickUp = nearestTarget;
                currentState = HumanStates.PickUp;
            }
        }
        return nearestTarget;
    }

    void PickUpSpecialItem(GameObject item)
    {
        item.transform.position = new Vector2(transform.position.x, transform.position.y + 1);
        item.transform.SetParent(this.transform);
        SpecialItems.Remove(item);

        currentState = HumanStates.Place;
    }

    void GoToPlaceItem(GameObject item)
    {
        transform.position = Vector3.MoveTowards(transform.position, ItemGoalPlacement[0].transform.position, 4f * Time.deltaTime);

        if (Vector2.Distance(transform.position, ItemGoalPlacement[0].transform.position) <= 0)
        { 
            currentState = HumanStates.Search;
            ItemGoalPlacement[0].GetComponent<MissionObjective_ItemNode>().HasItem = true;
            item.transform.SetParent(null);
            ItemGoalPlacement.Remove(ItemGoalPlacement[0]);
        }
        
    }
}
