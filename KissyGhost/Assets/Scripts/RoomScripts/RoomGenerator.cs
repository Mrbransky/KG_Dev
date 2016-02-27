using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum RoomTypes
{
    Center = 0, // "Big"
    Left = 1,   // "Medium Left"
    Right = 2,  // "Medium Right"
    Bottom = 3  // "Small"
}

public class RoomGenerator : MonoBehaviour 
{
	public GameObject BottomBaseRoomPiece;
	public GameObject LeftBaseRoomPiece;
    public GameObject RightBaseRoomPiece;
	public GameObject MainBaseRoomPiece;

    //Furniture list
	public List<GameObject> currentFurniture;
	public int numberOfFurniture = 3;
    public GameObject[] furnitureOptions;

    //Special Items
    public List<GameObject> AllSpecialItems;
    private List<GameObject> currentSpecialItems;

    void Awake()
    {
        currentSpecialItems = new List<GameObject>();
        currentFurniture = new List<GameObject>();

        for (int i = 0; i < 4; i++)
        {
            int rand = Random.Range(0, AllSpecialItems.Count);
            currentSpecialItems.Add(AllSpecialItems[rand]);
        }
    }

    void Start()
    {
        GenerateInternals(MainBaseRoomPiece.GetComponent<SpriteRenderer>().sprite.bounds.size, MainBaseRoomPiece.transform.position, RoomTypes.Center);
        GenerateInternals(LeftBaseRoomPiece.GetComponent<SpriteRenderer>().sprite.bounds.size, LeftBaseRoomPiece.transform.position, RoomTypes.Left);
        GenerateInternals(RightBaseRoomPiece.GetComponent<SpriteRenderer>().sprite.bounds.size, RightBaseRoomPiece.transform.position, RoomTypes.Right);
        GenerateInternals(BottomBaseRoomPiece.GetComponent<SpriteRenderer>().sprite.bounds.size, BottomBaseRoomPiece.transform.position, RoomTypes.Bottom);
        
        initializeMissionManager();
    }

    public void GenerateInternals(Vector2 RoomSize, Vector2 roomCenterPoint, RoomTypes roomType)
	{
        switch ((int)roomType)
        {
            case (int)RoomTypes.Left:
                spawnSpecItem(0, roomCenterPoint, RoomSize.x - 5, RoomSize.y - 5, RoomSize.y - 5);
                spawnFurniture(LeftBaseRoomPiece, roomCenterPoint, RoomSize.x - 5, RoomSize.y - 5, RoomSize.y - 5);
                break;

            case (int)RoomTypes.Right:
                spawnSpecItem(1, roomCenterPoint, RoomSize.x - 5, RoomSize.y - 5, RoomSize.y - 5);
                spawnFurniture(RightBaseRoomPiece, roomCenterPoint, RoomSize.x - 5, RoomSize.y - 5, RoomSize.y - 5);
                break;

            case (int)RoomTypes.Bottom:
                spawnSpecItem(2, roomCenterPoint, RoomSize.x - 8, RoomSize.y - 8, RoomSize.y - 8);
                spawnFurniture(BottomBaseRoomPiece, roomCenterPoint, RoomSize.x - 8, RoomSize.y - 8, RoomSize.y - 8);
                break;

            case (int)RoomTypes.Center:
                spawnSpecItem(3, roomCenterPoint, RoomSize.x - 2, RoomSize.y - 2, RoomSize.y - 6);
                spawnFurniture(MainBaseRoomPiece, roomCenterPoint, RoomSize.x - 2, RoomSize.y - 2, RoomSize.y - 5);
                break;
        }
	}

    private void spawnFurniture(GameObject roomObject, Vector2 roomCenterPoint, float newPos_x_delta, float newPos_y_delta1, float newPos_y_delta2)
    {
        Vector2 newPos = Vector2.zero;

        for (int i = 0; i < numberOfFurniture; i++)
        {
            newPos = getFurniturePos(roomCenterPoint, newPos_x_delta, newPos_y_delta1, newPos_y_delta2);
            GameObject newFurniture = (GameObject)Instantiate(furnitureOptions[Random.Range(0, furnitureOptions.Length)], newPos, Quaternion.identity);

            if (newFurniture.tag == "Furniture")
            {
                newFurniture.GetComponent<SpriteRenderer>().sortingOrder = (int)(-newFurniture.transform.localPosition.y);
            }
            else
            {
                newFurniture.GetComponent<SpriteRenderer>().sortingOrder = -20;
            }

            currentFurniture.Add(newFurniture);
            newFurniture.transform.SetParent(roomObject.transform);
        }
    }

    private Vector2 getFurniturePos(Vector2 roomCenterPoint, float newPos_x_delta, float newPos_y_delta1, float newPos_y_delta2)
    {
        float newPos_x = Random.Range(roomCenterPoint.x - newPos_x_delta, roomCenterPoint.x + newPos_x_delta);
        float newPos_y = Random.Range(roomCenterPoint.y - newPos_y_delta1, roomCenterPoint.y + newPos_y_delta2);
        Vector2 newPos = new Vector2(newPos_x, newPos_y);

        return newPos;
    }

    private void spawnSpecItem(int index, Vector2 roomCenterPoint, float specPos_x_delta, float specPos_y_delta1, float specPos_y_delta2)
    {
        float specPos_x = Random.Range(roomCenterPoint.x - specPos_x_delta, roomCenterPoint.x + specPos_x_delta);
        float specPos_y = Random.Range(roomCenterPoint.y - specPos_y_delta1, roomCenterPoint.y + specPos_y_delta2);
        Vector2 specPos = new Vector2(specPos_x, specPos_y);

        GameObject specialItem = (GameObject)Instantiate(currentSpecialItems[index], specPos, Quaternion.identity);
        GetComponent<MissionManager>().AddMissionObjective(specialItem);    
    }

    private void initializeMissionManager()
    {
        GetComponent<MissionManager>().Initialize();
    }
}
