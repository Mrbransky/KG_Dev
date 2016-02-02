using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoomChangeManager : MonoBehaviour {

    public List<GameObject> playersGoingBottom = new List<GameObject>();
    public List<GameObject> playersGoingLeft = new List<GameObject>();
    public List<GameObject> playersGoingRight = new List<GameObject>();

    int curPlayerCount;

    void Start()
    {
        curPlayerCount = GetComponent<GameManager>().playerCount;
    }
	void Update () {



        if(playersGoingBottom.Count >= curPlayerCount)
        {
            //TODO Put player nodes into the bottom room
        }
        if (playersGoingLeft.Count >= curPlayerCount)
        {
            //TODO Put player nodes into the left room
        }
        if (playersGoingRight.Count >= curPlayerCount)
        {
            //TODO Put player nodes into the right room
        }

	}
}
