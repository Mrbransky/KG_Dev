using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class RoomChangeManager : MonoBehaviour {

    public bool RoomGoalAccomplished = false;

    //TODO:Create more room goals other than timers
    public float timer = 30f;
    public Text timerText;

    public List<GameObject> playersGoingBottom = new List<GameObject>();
    public List<GameObject> playersGoingLeft = new List<GameObject>();
    public List<GameObject> playersGoingRight = new List<GameObject>();
    public List<GameObject> playersGoingBack = new List<GameObject>();

    int curPlayerCount;

    void Start()
    {
        curPlayerCount = GetComponent<GameManager>().playerCount;
    }
	void Update () {

        CountDown();
        CheckPlayersWaiting();

	}


//----------------------------------------------------------------------------
    void CheckPlayersWaiting()
    {
        if (RoomGoalAccomplished == true)
        {
            curPlayerCount = GetComponent<GameManager>().playerCount;
            if (playersGoingBottom.Count >= curPlayerCount - 1)
            {
                foreach (GameObject player in playersGoingBottom)
                {
                    Vector3 curRoomPosition = GetComponent<RoomGenerator>().BottomBaseRoomPiece.transform.position;
                    player.transform.position = new Vector2(Random.Range(curRoomPosition.x-2,curRoomPosition.x+2),curRoomPosition.y - 1);
                   

                    GetComponent<GameManager>().currentGhostPlayer.transform.position = new Vector2(curRoomPosition.x, curRoomPosition.y -1);
                    player.GetComponent<Rigidbody2D>().AddForce(-Vector2.up * 500);
                }
                RoomGoalAccomplished = false;
            }
            if (playersGoingLeft.Count >= curPlayerCount - 1)
            {
                foreach (GameObject player in playersGoingLeft)
                {
                    Vector3 curRoomPosition = GetComponent<RoomGenerator>().LeftBaseRoomPiece.transform.position;
                    player.transform.position = new Vector2(curRoomPosition.x + 10, Random.Range(curRoomPosition.y - 2, curRoomPosition.y + 2));

                    GetComponent<GameManager>().currentGhostPlayer.transform.position = new Vector2(curRoomPosition.x + 10, curRoomPosition.y);
                    player.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 1000);
                }
                RoomGoalAccomplished = false;
            }
            if (playersGoingRight.Count >= curPlayerCount - 1)
            {
                foreach (GameObject player in playersGoingRight)
                {
                    Vector3 curRoomPosition = GetComponent<RoomGenerator>().RightBaseRoomPiece.transform.position;
                    player.transform.position = new Vector2(curRoomPosition.x - 10, Random.Range(curRoomPosition.y - 2, curRoomPosition.y + 2));

                    GetComponent<GameManager>().currentGhostPlayer.transform.position = new Vector2(curRoomPosition.x - 10, curRoomPosition.y);
                    player.GetComponent<Rigidbody2D>().AddForce(-Vector2.left * 1000);
                }
                RoomGoalAccomplished = false;
            }
            if (playersGoingBack.Count >= curPlayerCount - 1)
            {
                foreach (GameObject player in playersGoingBack)
                {
                    Vector3 curRoomPosition = GetComponent<RoomGenerator>().MainBaseRoomPiece.transform.position;
                    player.transform.position = curRoomPosition;

                    GetComponent<GameManager>().currentGhostPlayer.transform.position = curRoomPosition;

                }
                RoomGoalAccomplished = false;
            }
            
        }
    }//CheckPlayerWaiting end

    void CountDown()
    {
        if (RoomGoalAccomplished == false)
        {
            timer -= Time.deltaTime;
            timerText.text = ((int)timer).ToString();
        }
        if (timer <= 0)
        {
            RoomGoalAccomplished = true;
            timer =30f;
        }
    }

}//class end

