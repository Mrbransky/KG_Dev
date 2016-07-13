using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GhostAI : MonoBehaviour {

    GameManager gm;
    RoomGenerator rg;

    public enum GhostState { Find, Kiss, Wander, Track, End };
    public GhostState currentState;

    public GameObject currentRoom;
    public GameObject furnToKiss;
    public GameObject closestPlayer;
    bool TouchingFurniture = false;
    public float ghostSpeed = 3.5f;
    Vector2 waypoint;

    float timer = 3;
    int kissCooldown = 3;
    int rand;
    Vector3 randomVec = Vector3.zero;
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
                furnToKiss = FindClosestUnkissed();
                closestPlayer = FindClosestPlayer();
                transform.position = Vector3.MoveTowards(transform.position, furnToKiss.transform.position, ghostSpeed * Time.deltaTime);
                break;
            case GhostState.Kiss:
                Kiss();
                break;
            case GhostState.Wander:
                Wander();
                break;
            //case GhostState.Track:
            //    Track();
            //    break;
            case GhostState.End:
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

        //add switch statement that checks which room ghost is in then changes furniture options
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
        if (kissCooldown > 0)
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
                rand = Random.Range(1, 3);
                waypoint = Random.insideUnitCircle * 25;
            }
            kissCooldown--;
        }
        else
        {
            currentState = GhostState.Wander;
            rand = Random.Range(1, 3);
        }

    }
    void Wander()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            kissCooldown = 3;
            currentState = GhostState.Find;
            timer = 3;
        }

        switch(rand)
        {
            case 1:
                transform.position = Vector3.MoveTowards(transform.position, RandomVector(), ghostSpeed * Time.deltaTime);
                break;
            case 2:
                transform.position = Vector3.MoveTowards(transform.position, closestPlayer.transform.position, ghostSpeed * Time.deltaTime);
                break;
            case 3:
                transform.position = Vector3.MoveTowards(transform.position, furnToKiss.transform.position, ghostSpeed * Time.deltaTime);
                break;
        }

        if(this.transform.position == randomVec)
        {
            randomVec = Vector3.zero;
        }
    }

    Vector3 RandomVector()
    {

        if(randomVec == Vector3.zero)
        randomVec = Random.insideUnitCircle * 8;


        return randomVec;
    }
    //void Track()
    //{
    //    transform.position = Vector3.MoveTowards(transform.position, closestPlayer.transform.position, ghostSpeed * Time.deltaTime);
    //    timer -= Time.deltaTime;
    //    if (timer <= 0)
    //    {
    //        currentState = GhostState.Find;
    //        timer = 1;
    //    }
    //}
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
