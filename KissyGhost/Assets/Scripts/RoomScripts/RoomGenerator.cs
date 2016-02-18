using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour {


	public GameObject BottomBaseRoomPiece;
	public GameObject LeftBaseRoomPiece;
    public GameObject RightBaseRoomPiece;
	public GameObject MainBaseRoomPiece;

    //Furniture list
	public List<GameObject> currentFurniture;
	public int numberOfFurniture = 3;
    public GameObject[] furnitureOptions;

    //Room zeroing
    Vector2 curRoomCenter;
    string roomType;
    private Vector2 newPos;

    //Special Items
    public List<GameObject> AllSpecialItems;
    List<GameObject> currentSpecialItems = new List<GameObject>();
    Vector2 specPos;

    void Start()
    {
        GenerateInternals(MainBaseRoomPiece.GetComponent<SpriteRenderer>().sprite.bounds.size, MainBaseRoomPiece.transform.position, "big");
        GenerateInternals(LeftBaseRoomPiece.GetComponent<SpriteRenderer>().sprite.bounds.size, LeftBaseRoomPiece.transform.position, "mediumLeft");
        GenerateInternals(RightBaseRoomPiece.GetComponent<SpriteRenderer>().sprite.bounds.size, RightBaseRoomPiece.transform.position, "mediumRight");
        GenerateInternals(BottomBaseRoomPiece.GetComponent<SpriteRenderer>().sprite.bounds.size, BottomBaseRoomPiece.transform.position, "small");
        
        initializeMissionManager();
    }

    void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            int rand = Random.Range(0, AllSpecialItems.Count);
            currentSpecialItems.Add(AllSpecialItems[rand]);
        }
    }

	public void GenerateInternals(Vector2 RoomSize, Vector2 roomCenterPoint, string room)
	{
        int NoF = numberOfFurniture;
		currentFurniture = new List<GameObject>();
        bool needsSpecialItem = true;

		for(int i = 0; i<NoF; i++)
		{
            switch (room)
            {
                case "mediumLeft":
                    newPos = new Vector2(Random.Range(roomCenterPoint.x - (RoomSize.x - 5f), roomCenterPoint.x + (RoomSize.x - 5f)), Random.Range(roomCenterPoint.y - (RoomSize.y - 5f), roomCenterPoint.y + (RoomSize.y - 5f)));
                    if (needsSpecialItem == true)
                    {
                        specPos = new Vector2(Random.Range(roomCenterPoint.x - (RoomSize.x - 5f), roomCenterPoint.x + (RoomSize.x - 5f)), Random.Range(roomCenterPoint.y - (RoomSize.y - 5f), roomCenterPoint.y + (RoomSize.y - 5f)));
                        GameObject specialItem = (GameObject)Instantiate(currentSpecialItems[0], specPos, Quaternion.identity);
                        GetComponent<MissionManager>().AddMissionObjective(specialItem);
                        needsSpecialItem = false;
                    }
                    break;
                case "mediumRight":
                    newPos = new Vector2(Random.Range(roomCenterPoint.x - (RoomSize.x - 5f), roomCenterPoint.x + (RoomSize.x - 5f)), Random.Range(roomCenterPoint.y - (RoomSize.y - 5f), roomCenterPoint.y + (RoomSize.y - 5f)));
                    if (needsSpecialItem == true)
                    {
                        specPos = new Vector2(Random.Range(roomCenterPoint.x - (RoomSize.x - 5f), roomCenterPoint.x + (RoomSize.x - 5f)), Random.Range(roomCenterPoint.y - (RoomSize.y - 5f), roomCenterPoint.y + (RoomSize.y - 5f)));
                        GameObject specialItem = (GameObject)Instantiate(currentSpecialItems[1], specPos, Quaternion.identity);
                        GetComponent<MissionManager>().AddMissionObjective(specialItem);
                        needsSpecialItem = false;
                    }
                    break;
                case"small":
                    newPos = new Vector2(Random.Range(roomCenterPoint.x - (RoomSize.x - 8f), roomCenterPoint.x + (RoomSize.x - 8f)), Random.Range(roomCenterPoint.y - (RoomSize.y - 8f), roomCenterPoint.y + (RoomSize.y - 8f)));
                    if (needsSpecialItem == true)
                    {
                        specPos = new Vector2(Random.Range(roomCenterPoint.x - (RoomSize.x - 8f), roomCenterPoint.x + (RoomSize.x - 8f)), Random.Range(roomCenterPoint.y - (RoomSize.y - 8f), roomCenterPoint.y + (RoomSize.y - 8f)));
                        GameObject specialItem = (GameObject)Instantiate(currentSpecialItems[2], specPos, Quaternion.identity);
                        GetComponent<MissionManager>().AddMissionObjective(specialItem);
                        needsSpecialItem = false;
                    }
                    break;
                case"big":
                    newPos = new Vector2(Random.Range(roomCenterPoint.x - (RoomSize.x - 2f), roomCenterPoint.x + (RoomSize.x - 2f)), Random.Range(roomCenterPoint.y - (RoomSize.y - 2f), roomCenterPoint.y + (RoomSize.y - 3f)));
                    if (needsSpecialItem == true)
                    {
                        specPos = new Vector2(Random.Range(roomCenterPoint.x - (RoomSize.x - 2f), roomCenterPoint.x + (RoomSize.x - 2f)), Random.Range(roomCenterPoint.y - (RoomSize.y - 2f), roomCenterPoint.y + (RoomSize.y - 3f)));
                        GameObject specialItem = (GameObject)Instantiate(currentSpecialItems[3], specPos, Quaternion.identity);
                        GetComponent<MissionManager>().AddMissionObjective(specialItem);
                        needsSpecialItem = false;
                    }                 
                    break;
            }

            GameObject newFurniture = Instantiate(furnitureOptions[Random.Range(0, furnitureOptions.Length)], newPos, Quaternion.identity) as GameObject;
            
            if (newFurniture.gameObject.tag == "Furniture")
            {
                newFurniture.GetComponent<SpriteRenderer>().sortingOrder = (int)(-newFurniture.transform.localPosition.y);
                currentFurniture.Add(newFurniture);
            }
            else 
            {
                newFurniture.GetComponent<SpriteRenderer>().sortingOrder = -20;
                currentFurniture.Add(newFurniture);
            }
		}
	}
    void Update()
    {
        foreach (GameObject fur in currentFurniture)
        {
            fur.GetComponent<SpriteRenderer>().sortingOrder = (int)(-fur.transform.localPosition.y);
        }
    }
    private void initializeMissionManager()
    {
        GetComponent<MissionManager>().Initialize();
    }
}
