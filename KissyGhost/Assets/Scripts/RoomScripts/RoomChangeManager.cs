using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum RoomLocations
{
    Center = 0,
    Left = 1,
    Right = 2,
    Bottom = 3,
    Total = 4
}

public enum SubObjectiveTypes
{
    Timer = 0,
    PullSwitch = 1,
    StandOnSwitch = 2
}

public class RoomChangeManager : MonoBehaviour
{
    public bool RoomGoalAccomplished = false;

    //TODO:Create more room goals other than timers
    public float timer = 10f;
    public Text timerText;

    public List<GameObject> playersGoingBottom = new List<GameObject>();
    public List<GameObject> playersGoingLeft = new List<GameObject>();
    public List<GameObject> playersGoingRight = new List<GameObject>();
    public List<GameObject> playersGoingBack = new List<GameObject>();

    public SubObjectiveTypes SubObjective_Center = SubObjectiveTypes.Timer;
    public SubObjectiveTypes SubObjective_Left = SubObjectiveTypes.Timer;
    public SubObjectiveTypes SubObjective_Right = SubObjectiveTypes.Timer;
    public SubObjectiveTypes SubObjective_Bottom = SubObjectiveTypes.Timer;

    private SubObjectiveTypes[] roomSubObjectiveTypeArray;
    private bool[] roomSubObjectiveAccomplishedArray;
    private RoomLocations currentRoomLocation = RoomLocations.Center;

    int curPlayerCount;

    void Start()
    {
        curPlayerCount = GetComponent<GameManager>().playerCount;

        roomSubObjectiveTypeArray = new SubObjectiveTypes[(int)RoomLocations.Total];
        roomSubObjectiveAccomplishedArray = new bool[(int)RoomLocations.Total];

        roomSubObjectiveTypeArray[(int)RoomLocations.Center] = SubObjective_Center;
        roomSubObjectiveTypeArray[(int)RoomLocations.Left] = SubObjective_Left;
        roomSubObjectiveTypeArray[(int)RoomLocations.Right] = SubObjective_Right;
        roomSubObjectiveTypeArray[(int)RoomLocations.Bottom] = SubObjective_Bottom;
    }

	void Update ()
    {
        if (!roomSubObjectiveAccomplishedArray[(int)currentRoomLocation])
        {
            int currentRoomSubObjective = (int)roomSubObjectiveTypeArray[(int)currentRoomLocation];

            switch (currentRoomSubObjective)
            {
                case (int)SubObjectiveTypes.Timer:
                    CountDown();
                    break;
                case (int)SubObjectiveTypes.PullSwitch:
                    break;
                case (int)SubObjectiveTypes.StandOnSwitch:
                    break;
            }
        }
        else
        {
            CheckPlayersWaiting();
        }
	}

//----------------------------------------------------------------------------
    void CheckPlayersWaiting()
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
        }
        else if (playersGoingLeft.Count >= curPlayerCount - 1)
        {
            foreach (GameObject player in playersGoingLeft)
            {
                Vector3 curRoomPosition = GetComponent<RoomGenerator>().LeftBaseRoomPiece.transform.position;
                player.transform.position = new Vector2(curRoomPosition.x + 10, Random.Range(curRoomPosition.y - 2, curRoomPosition.y + 2));

                GetComponent<GameManager>().currentGhostPlayer.transform.position = new Vector2(curRoomPosition.x + 10, curRoomPosition.y);
                player.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 1000);
            }
        }
        else if (playersGoingRight.Count >= curPlayerCount - 1)
        {
            foreach (GameObject player in playersGoingRight)
            {
                Vector3 curRoomPosition = GetComponent<RoomGenerator>().RightBaseRoomPiece.transform.position;
                player.transform.position = new Vector2(curRoomPosition.x - 10, Random.Range(curRoomPosition.y - 2, curRoomPosition.y + 2));

                GetComponent<GameManager>().currentGhostPlayer.transform.position = new Vector2(curRoomPosition.x - 10, curRoomPosition.y);
                player.GetComponent<Rigidbody2D>().AddForce(-Vector2.left * 1000);
            }
        }
        else if (playersGoingBack.Count >= curPlayerCount - 1)
        {
            foreach (GameObject player in playersGoingBack)
            {
                Vector3 curRoomPosition = GetComponent<RoomGenerator>().MainBaseRoomPiece.transform.position;
                player.transform.position = curRoomPosition;

                GetComponent<GameManager>().currentGhostPlayer.transform.position = curRoomPosition;

            }
        }
    }//CheckPlayerWaiting end

    void CountDown()
    {
        timer -= Time.deltaTime;
        timerText.text = ((int)timer).ToString();

        if (timer <= 0)
        {
            roomSubObjectiveAccomplishedArray[(int)currentRoomLocation] = true;
            timer = 10f;
        }
    }

}//class end

