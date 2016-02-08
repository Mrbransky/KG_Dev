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



        if(playersGoingBottom.Count >= curPlayerCount-1)
        {
            foreach (GameObject player in playersGoingBottom)
            {
                player.transform.position = GetComponent<RoomGenerator>().BottomBaseRoomPiece.transform.position;
                GetComponent<GameManager>().currentGhostPlayer.transform.position = GetComponent<RoomGenerator>().BottomBaseRoomPiece.transform.position;
            }
        }
        if (playersGoingLeft.Count >= curPlayerCount-1)
        {
            foreach (GameObject player in playersGoingLeft)
            {
                player.transform.position = GetComponent<RoomGenerator>().LeftBaseRoomPiece.transform.position;
                GetComponent<GameManager>().currentGhostPlayer.transform.position = GetComponent<RoomGenerator>().LeftBaseRoomPiece.transform.position;
            }
        }
        if (playersGoingRight.Count >= curPlayerCount-1)
        {
            foreach (GameObject player in playersGoingRight)
            {
                player.transform.position = GetComponent<RoomGenerator>().RightBaseRoomPiece.transform.position;
                GetComponent<GameManager>().currentGhostPlayer.transform.position = GetComponent<RoomGenerator>().RightBaseRoomPiece.transform.position;
            }
        }

	}
}
