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

// TODO: Create more room goals other than timers
public enum SubObjectiveTypes
{
    Timer = 0,
    PullSwitch = 1,
    StandOnSwitch = 2
}

public class RoomChangeManager : MonoBehaviour
{
    public float MaxTimerDuration = 10f;
    public float currentTimer;
    private float DoorFadeIncrement = .02f;
    public int NumPlayersIn;
    public Text timerText;

    public GameObject[] CenterRoomDoorArrows;
    public GameObject LeftRoomDoorArrow;
    public GameObject RightRoomDoorArrow;
    public GameObject BottomRoomDoorArrow;

    public List<GameObject> playersGoingBottom = new List<GameObject>();
    public List<GameObject> playersGoingLeft = new List<GameObject>();
    public List<GameObject> playersGoingRight = new List<GameObject>();
    public List<GameObject> playersGoingBack = new List<GameObject>();

    public List<GameObject> doorSprites = new List<GameObject>();

    public SubObjectiveTypes SubObjective_Center = SubObjectiveTypes.Timer;
    public SubObjectiveTypes SubObjective_Left = SubObjectiveTypes.Timer;
    public SubObjectiveTypes SubObjective_Right = SubObjectiveTypes.Timer;
    public SubObjectiveTypes SubObjective_Bottom = SubObjectiveTypes.Timer;

    private SubObjectiveTypes[] roomSubObjectiveTypeArray;
    private bool[] roomSubObjectiveAccomplishedArray;
    private bool[] AreDoorsFadingIn = new bool[4];
    private bool[] HasMadeRoomSound = new bool[4];
    private bool AllDoorsIn;

    private RoomLocations currentRoomLocation = RoomLocations.Center;

    private bool StartFadingDoorsIn;

    int curPlayerCount;

    GameObject GhostAI;

    float testTimer = 1f;
    bool shouldCountdown = false;
    public RoomLocations CurrentRoomLocation
    {
        get { return currentRoomLocation; }
    }

    void Start()
    {
        SetDoorAlphas(0.1f);
        disableDoors();

        curPlayerCount = GetComponent<GameManager>().playerCount;

        currentTimer = MaxTimerDuration;

        roomSubObjectiveTypeArray = new SubObjectiveTypes[(int)RoomLocations.Total];
        roomSubObjectiveAccomplishedArray = new bool[(int)RoomLocations.Total];

        roomSubObjectiveTypeArray[(int)RoomLocations.Center] = SubObjective_Center;
        roomSubObjectiveTypeArray[(int)RoomLocations.Left] = SubObjective_Left;
        roomSubObjectiveTypeArray[(int)RoomLocations.Right] = SubObjective_Right;
        roomSubObjectiveTypeArray[(int)RoomLocations.Bottom] = SubObjective_Bottom;
    }

    void Update()
    {
        if(!AllDoorsIn)
        {
            //foreach(GameObject obj in doorSprites)
            //{
            //    if (currentTimer > 9)
            //        obj.GetComponent<EdgeCollider2D>().enabled = false;
            //    else
            //        obj.GetComponent<EdgeCollider2D>().enabled = true;
            //}

            if (currentTimer <= .2f && !AreDoorsFadingIn[(int)currentRoomLocation])
            {
                AreDoorsFadingIn[(int)currentRoomLocation] = true;
                EnableGhostBarriers(currentRoomLocation);
            }

            for (int i = 0; i < AreDoorsFadingIn.Length; i++)
            {
                if (AreDoorsFadingIn[i] && (int)currentRoomLocation == i)
                {
                    FadeInDoors(currentRoomLocation);    
                }
            }

            AllDoorsIn = AreAllDoorsIn();
        }

        if (!roomSubObjectiveAccomplishedArray[(int)currentRoomLocation])
        {
            int currentRoomSubObjective = (int)roomSubObjectiveTypeArray[(int)currentRoomLocation];

            switch (currentRoomSubObjective)
            {
                case (int)SubObjectiveTypes.Timer:
                    SubObjectiveUpdate_Timer();
                    break;
                case (int)SubObjectiveTypes.PullSwitch:
                    break;
                case (int)SubObjectiveTypes.StandOnSwitch:
                    break;
            }
        }
        else
        {
            GhostAI = GameObject.Find("AI_Ghost(Clone)");

            if(curPlayerCount != 1)
                CheckPlayersWaiting();
        }
    }

    #region Door Fading
    void disableDoors()
    {
        foreach (GameObject obj in doorSprites)
            obj.GetComponent<SpriteRenderer>().enabled = false;
    }

    void enableDoors()
    {
        foreach (GameObject obj in doorSprites)
            obj.GetComponent<SpriteRenderer>().enabled = true;

        soundManager.SOUND_MAN.playSound("Play_DoorOpen", gameObject);
    }

    void SetDoorAlphas(float alphaVal)
    {
        foreach (GameObject obj in doorSprites)
            obj.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alphaVal);
    }

    void EnableGhostBarriers(RoomLocations room)
    {
        string searchString = "";

        switch(room)
        {
            case RoomLocations.Bottom:
                searchString = "Bottom";
                break;

            case RoomLocations.Center:
                searchString = "Center";
                break;

            case RoomLocations.Left:
                searchString = "Left";
                break;

            case RoomLocations.Right:
                searchString = "Right";
                break;
            
            default:
                break;
        }

        foreach(GameObject door in doorSprites)
        {
            if (door.name.Contains(searchString))
            {
                if (door.name != "SouthDoor_Left(Center)" && door.name != "SouthDoor_Left(Bottom)")
                {
                    Transform ghostbarrier = door.transform.Find("GhostBarrier").GetComponent<Transform>();

                    if (ghostbarrier != null)
                    {
                        EdgeCollider2D[] edges = ghostbarrier.GetComponents<EdgeCollider2D>();

                        foreach (EdgeCollider2D edge in edges)
                            edge.enabled = true;
                    }
                }
            }
        }

    }

    bool AreAllDoorsIn()
    {
        int numDoorsIn = 0;

        foreach (bool b in AreDoorsFadingIn)
            if (b) numDoorsIn++;

        if (numDoorsIn == AreDoorsFadingIn.Length)
        {
            foreach (GameObject obj in doorSprites)
            {
                if (obj.GetComponent<SpriteRenderer>().color.a != 1)
                    return false;
            }
            return true;			
        }
            
        return false;
    }

    void FadeInDoors(RoomLocations currentRoom)
    {
        string doorID = "(" + currentRoom.ToString() + ")";

        foreach (GameObject obj in doorSprites)
        {
            if (obj.name.Contains(doorID))
            {
                obj.GetComponent<SpriteRenderer>().enabled = true;
                obj.GetComponent<SpriteRenderer>().color += new Color(0f, 0f, 0f, DoorFadeIncrement);

                if (obj.GetComponent<SpriteRenderer>().color.a >= 1f)
                    obj.GetComponent<EdgeCollider2D>().enabled = false;
            }
        }

        if (!HasMadeRoomSound[(int)currentRoom])
        {
            HasMadeRoomSound[(int)currentRoom] = true;
            soundManager.SOUND_MAN.playSound("Play_DoorOpen", gameObject);           
        }
    }
    #endregion

    #region Sub Objective Functions
    private void SubObjectiveCheck_OnRoomChanged()
    {
        //ROOM CHANGE SOUND :D
		soundManager.SOUND_MAN.playSound("Play_RoomTransition", gameObject);

        if (!roomSubObjectiveAccomplishedArray[(int)currentRoomLocation])
        {
            int currentRoomSubObjective = (int)roomSubObjectiveTypeArray[(int)currentRoomLocation];

            switch (currentRoomSubObjective)
            {
                case (int)SubObjectiveTypes.Timer:
                    timerText.enabled = true;
                    break;
                case (int)SubObjectiveTypes.PullSwitch:
                    break;
                case (int)SubObjectiveTypes.StandOnSwitch:
                    break;
            }

        }
        AkSoundEngine.PostEvent("Stop_FurnitureMove", gameObject);
        if (GetComponent<GameManager>().currentGhostPlayer.GetComponent<Ghost>())
        GetComponent<GameManager>().currentGhostPlayer.GetComponent<Ghost>().SetTimeSinceKiss(1.85f);
        
    }

    private void SubObjectiveUpdate_Timer()
    {
        currentTimer -= Time.deltaTime;
        timerText.text = ((int)currentTimer).ToString();

        if (currentTimer <= 0)
        {
            currentTimer = MaxTimerDuration;
            timerText.text = "0";

            SubObjectiveAccomplished();
        }
    }

    private void SubObjectiveAccomplished()
    {
        roomSubObjectiveAccomplishedArray[(int)currentRoomLocation] = true;

        switch (currentRoomLocation)
        {
            case RoomLocations.Center:
                foreach (GameObject go in CenterRoomDoorArrows)
                {
                    go.GetComponent<DoorArrowTracker>().StartFading();
                }
                break;

            case RoomLocations.Bottom:
                BottomRoomDoorArrow.GetComponent<DoorArrowTracker>().StartFading();
                break;

            case RoomLocations.Left:
                LeftRoomDoorArrow.GetComponent<DoorArrowTracker>().StartFading();
                break;

            case RoomLocations.Right:
                RightRoomDoorArrow.GetComponent<DoorArrowTracker>().StartFading();
                break;
        }
    }
    #endregion Sub Objective Functions

    #region Room Change Functions
    void CheckPlayersWaiting()
    {
        curPlayerCount = GetComponent<GameManager>().playerCount;

        if (playersGoingBottom.Count >= curPlayerCount - 1)
        {
            SendPlayersToBottomRoom();
            currentRoomLocation = RoomLocations.Bottom;
            SubObjectiveCheck_OnRoomChanged();

            if (!roomSubObjectiveAccomplishedArray[(int)currentRoomLocation])
            {
                BottomRoomDoorArrow.SetActive(true);
            }
            if(GhostAI != null) GhostAI.GetComponent<GhostAI>().currentRoom = GetComponent<RoomGenerator>().BottomBaseRoomPiece; 
        }
        else if (playersGoingLeft.Count >= curPlayerCount - 1)
        {
            SendPlayersToLeftRoom();
            currentRoomLocation = RoomLocations.Left;
            SubObjectiveCheck_OnRoomChanged();

            if (!roomSubObjectiveAccomplishedArray[(int)currentRoomLocation])
            {
                LeftRoomDoorArrow.SetActive(true);
            }
            if (GhostAI != null)  GhostAI.GetComponent<GhostAI>().currentRoom = GetComponent<RoomGenerator>().LeftBaseRoomPiece;
        }
        else if (playersGoingRight.Count >= curPlayerCount - 1)
        {
            SendPlayersToRightRoom();
            currentRoomLocation = RoomLocations.Right;
            SubObjectiveCheck_OnRoomChanged();

            if (!roomSubObjectiveAccomplishedArray[(int)currentRoomLocation])
            {
                RightRoomDoorArrow.SetActive(true);
            }
            if (GhostAI != null) GhostAI.GetComponent<GhostAI>().currentRoom = GetComponent<RoomGenerator>().RightBaseRoomPiece;
        }
        else if (playersGoingBack.Count >= curPlayerCount - 1)
        {
            SendPlayersToCenterRoom(currentRoomLocation);
            currentRoomLocation = RoomLocations.Center;
            SubObjectiveCheck_OnRoomChanged();

            if (!roomSubObjectiveAccomplishedArray[(int)currentRoomLocation])
            {
                foreach (GameObject go in CenterRoomDoorArrows)
                {
                    go.SetActive(true);
                }
            }
            if (GhostAI != null) GhostAI.GetComponent<GhostAI>().currentRoom = GetComponent<RoomGenerator>().MainBaseRoomPiece;
        }
    }

    private void SendPlayersToBottomRoom()
    {
        Vector3 curRoomPosition = GetComponent<RoomGenerator>().BottomBaseRoomPiece.transform.position;
        //Camera.main.transform.position = new Vector3(curRoomPosition.x, curRoomPosition.y, Camera.main.transform.position.z);
        foreach (GameObject player in playersGoingBottom)
        {
            player.transform.position = new Vector2(Random.Range(curRoomPosition.x - 2, curRoomPosition.x + 2), curRoomPosition.y + 1);

            GetComponent<GameManager>().currentGhostPlayer.transform.position = new Vector2(curRoomPosition.x, curRoomPosition.y);
            player.GetComponent<Rigidbody2D>().AddForce(-Vector2.up * 200);
        }
    }

    private void SendPlayersToLeftRoom()
    {
        Vector3 curRoomPosition = GetComponent<RoomGenerator>().LeftBaseRoomPiece.transform.position;
        foreach (GameObject player in playersGoingLeft)
        {
            player.transform.position = new Vector2(curRoomPosition.x + 5, Random.Range(curRoomPosition.y - 2, curRoomPosition.y + 2));

            GetComponent<GameManager>().currentGhostPlayer.transform.position = new Vector2(curRoomPosition.x + 8, curRoomPosition.y);
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 200);
        }
    }

    private void SendPlayersToRightRoom()
    {
        Vector3 curRoomPosition = GetComponent<RoomGenerator>().RightBaseRoomPiece.transform.position;
        foreach (GameObject player in playersGoingRight)
        {
            player.transform.position = new Vector2(curRoomPosition.x - 5, Random.Range(curRoomPosition.y - 2, curRoomPosition.y + 2));
            GetComponent<GameManager>().currentGhostPlayer.transform.position = new Vector2(curRoomPosition.x - 8, curRoomPosition.y);
            player.GetComponent<Rigidbody2D>().AddForce(-Vector2.left * 200);
        }
    }

    private void SendPlayersToCenterRoom(RoomLocations roomPosition)
    {
        foreach (GameObject player in playersGoingBack)
        {
            Vector3 curRoomPosition = GetComponent<RoomGenerator>().MainBaseRoomPiece.transform.position;

            switch(roomPosition){
                case RoomLocations.Left:
                    player.transform.position = new Vector2(curRoomPosition.x - 10, Random.Range(curRoomPosition.y - 2, curRoomPosition.y + 2));
                    player.GetComponent<Rigidbody2D>().AddForce(-Vector2.left * 300);
                    break;
                case RoomLocations.Bottom:
                    player.transform.position = new Vector2(Random.Range(curRoomPosition.x - 2, curRoomPosition.x + 2), curRoomPosition.y - 6);
                    player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 300);
                    break;
                case RoomLocations.Right:
                    player.transform.position = new Vector2(curRoomPosition.x + 10, Random.Range(curRoomPosition.y - 2, curRoomPosition.y + 2));
                    player.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 300);
                    break;
            }

            //player.transform.position = curRoomPosition;

            GetComponent<GameManager>().currentGhostPlayer.transform.position = curRoomPosition;
        }
    }
    #endregion Room Change Functions
}