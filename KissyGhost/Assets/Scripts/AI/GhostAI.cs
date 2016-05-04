using UnityEngine;
using System.Collections;

public class GhostAI : MonoBehaviour {

    GameManager gm;
    RoomGenerator rg;

    enum GhostState { Find, Kiss, Wander };
    GhostState currentState;

    GameObject currentRoom;
    public GameObject furnToKiss;
    public GameObject closestPlayer;
    bool TouchingFurniture = false;
    float ghostSpeed = 3.5f;
    Vector2 waypoint;

    float timer = 1;
	void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        rg = GameObject.Find("GameManager").GetComponent<RoomGenerator>();
        currentState = GhostState.Find;
        currentRoom = rg.MainBaseRoomPiece;
	}

	void Update () {
        if(TouchingFurniture)
        { ghostSpeed = 2.5f; }
        else { ghostSpeed = 3.5f; }

        switch(currentState)
        {
            case GhostState.Find:
                //Debug.Log("I am Finding");
                furnToKiss = FindClosestUnkissed();
                closestPlayer = FindClosestPlayer();
                transform.position = Vector3.MoveTowards(transform.position, furnToKiss.transform.position, ghostSpeed * Time.deltaTime);
                break;
            case GhostState.Kiss:
                //Debug.Log("I am Kissing");
                Kiss();
                break;
            case GhostState.Wander:
                //Debug.Log("I am Wandering");
                Wander();
                break;
            default:
                break;
        }
	    
	}

    GameObject FindClosestUnkissed()
    {
        GameObject nearestTarget = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach(GameObject potentialTarget in rg.currentFurniture)
        {
            if (potentialTarget.GetComponent<KissableFurniture>())
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistance && potentialTarget.GetComponent<KissableFurniture>().isKissed == false)
                {
                    closestDistance = dSqrToTarget;
                    nearestTarget = potentialTarget;
                }
                if (directionToTarget == Vector3.zero)
                {
                    currentState = GhostState.Kiss;
                }
            }
        }
        return nearestTarget;
    }
    GameObject FindClosestPlayer()
    {
        GameObject nearestTarget = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject potentialTarget in gm.currentPlayers)
        {
            if (potentialTarget.tag == "Player")
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistance)
                {
                    closestDistance = dSqrToTarget;
                    nearestTarget = potentialTarget;
                    potentialTarget.GetComponent<Human>().priority += 0.1f;

                }
                else
                { potentialTarget.GetComponent<Human>().priority -= 0.1f; }
            }
        }
        return nearestTarget;
    }
    void Kiss()
    {
        furnToKiss.GetComponent<KissableFurniture>().KissFurniture();

        int chanceToWander = Random.Range(1, 4);

        if (chanceToWander != 3)
        {
            currentState = GhostState.Find;
        }
        else
        {
            currentState = GhostState.Wander;
            waypoint = Random.insideUnitCircle * 50;
        }
        }
    void Wander()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoint, ghostSpeed * Time.deltaTime);

        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            currentState = GhostState.Find;
            timer = 1;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Furniture")
        {
            TouchingFurniture = true;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Furniture")
        {
            TouchingFurniture = false;
        }
    }
}
