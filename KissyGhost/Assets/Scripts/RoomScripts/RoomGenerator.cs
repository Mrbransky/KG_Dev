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
    public SpriteSorter _SpriteSorter;

    //Furniture list
	public List<GameObject> currentFurniture;
	public int numberOfFurniture = 3;
    public GameObject[] furnitureOptions;

    //Special Items
    public List<GameObject> AllSpecialItems;
    private List<GameObject> currentSpecialItems;

    public float minDistance = 10;
    void Awake()
    {
        currentSpecialItems = new List<GameObject>();
        currentFurniture = new List<GameObject>();

        for (int i = 0; i < 4; i++)
        {
            int rand = Random.Range(0, AllSpecialItems.Count);
            currentSpecialItems.Add(AllSpecialItems[rand]);
            AllSpecialItems.Remove(AllSpecialItems[rand]);

            //if (AllSpecialItems[rand].name.Contains("Cat"))
            //{
            //    List<GameObject> CatsToDelete = new List<GameObject>();
            //    foreach (GameObject obj in AllSpecialItems)
            //    {
            //        if (obj.name.Contains("Cat"))
            //            CatsToDelete.Add(obj);
            //    }

            //    foreach (GameObject obj in CatsToDelete)
            //    {
            //        AllSpecialItems.Remove(obj);
            //    }
            //    CatsToDelete.Clear();
            //    return;
            //}

            
        }
    }

    void Start()
    {
        GenerateInternals(MainBaseRoomPiece.GetComponent<SpriteRenderer>().sprite.bounds.size, MainBaseRoomPiece.transform.position, RoomTypes.Center);
        GenerateInternals(LeftBaseRoomPiece.GetComponent<SpriteRenderer>().sprite.bounds.size, LeftBaseRoomPiece.transform.position, RoomTypes.Left);
        GenerateInternals(RightBaseRoomPiece.GetComponent<SpriteRenderer>().sprite.bounds.size, RightBaseRoomPiece.transform.position, RoomTypes.Right);
        GenerateInternals(BottomBaseRoomPiece.GetComponent<SpriteRenderer>().sprite.bounds.size, BottomBaseRoomPiece.transform.position, RoomTypes.Bottom);
        
        initializeMissionManager();

        if (_SpriteSorter != null)
        {
            _SpriteSorter.isInitialized = true;
        }
    }

    public void GenerateInternals(Vector2 RoomSize, Vector2 roomCenterPoint, RoomTypes roomType)
	{
        switch ((int)roomType)
        {
            case (int)RoomTypes.Left:
                spawnSpecItem(0, roomCenterPoint, RoomSize.x - 5, RoomSize.y - 5, RoomSize.y - 5);
                spawnFurniture(LeftBaseRoomPiece, roomCenterPoint, RoomSize.x - 5, RoomSize.y - 5, RoomSize.y - 5, roomType);
                break;

            case (int)RoomTypes.Right:
                spawnSpecItem(1, roomCenterPoint, RoomSize.x - 5, RoomSize.y - 5, RoomSize.y - 5);
                spawnFurniture(RightBaseRoomPiece, roomCenterPoint, RoomSize.x - 5, RoomSize.y - 5, RoomSize.y - 5, roomType);
                break;

            case (int)RoomTypes.Bottom:
                spawnSpecItem(2, roomCenterPoint, RoomSize.x - 8, RoomSize.y - 8, RoomSize.y - 8);
                spawnFurniture(BottomBaseRoomPiece, roomCenterPoint, RoomSize.x - 8, RoomSize.y - 8, RoomSize.y - 8, roomType);
                break;

            case (int)RoomTypes.Center:
                spawnSpecItem(3, roomCenterPoint, RoomSize.x - 2, RoomSize.y - 2, RoomSize.y - 6);
                spawnFurniture(MainBaseRoomPiece, roomCenterPoint, RoomSize.x - 2, RoomSize.y - 2, RoomSize.y - 5, roomType);
                break;
        }
	}

    private void spawnFurniture(GameObject roomObject, Vector2 roomCenterPoint, float newPos_x_delta, float newPos_y_delta1, float newPos_y_delta2, RoomTypes roomType)
    {
        Vector2 newPos = Vector2.zero;

        for (int i = 0; i < numberOfFurniture; i++)
        {
            GameObject FurnitureToSpawn = furnitureOptions[Random.Range(0, furnitureOptions.Length)];
            newPos = getFurniturePos(roomCenterPoint, newPos_x_delta, newPos_y_delta1, newPos_y_delta2);
            GameObject newFurniture = (GameObject)Instantiate(FurnitureToSpawn, newPos, Quaternion.identity);

            if (newFurniture.tag == "Furniture")
            {
                newFurniture.GetComponent<SpriteRenderer>().sortingOrder = (int)(-newFurniture.transform.localPosition.y);

                if (_SpriteSorter != null)
                {
                    switch ((int)roomType)
                    {
                        case (int)RoomTypes.Left:
                            _SpriteSorter.LeftRoom_SpriteRendererList.Add(newFurniture.GetComponent<SpriteRenderer>());
                            break;

                        case (int)RoomTypes.Right:
                            _SpriteSorter.RightRoom_SpriteRendererList.Add(newFurniture.GetComponent<SpriteRenderer>());
                            break;

                        case (int)RoomTypes.Bottom:
                            _SpriteSorter.BottomRoom_SpriteRendererList.Add(newFurniture.GetComponent<SpriteRenderer>());
                            break;

                        case (int)RoomTypes.Center:
                            _SpriteSorter.CenterRoom_SpriteRendererList.Add(newFurniture.GetComponent<SpriteRenderer>());
                            break;
                    }
                }
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

 //Broken for now
        //Vector2 newPos;
        //Collider2D[] neighbors;
        //int tries = 0;
        //do
        //{
        //    float newPos_x = Random.Range(roomCenterPoint.x - newPos_x_delta, roomCenterPoint.x + newPos_x_delta);
        //    float newPos_y = Random.Range(roomCenterPoint.y - newPos_y_delta1, roomCenterPoint.y + newPos_y_delta2);
        //    newPos = new Vector2(newPos_x, newPos_y);
        //    neighbors = Physics2D.OverlapCircleAll(newPos, minDistance);
        //    tries++;
        //    if(tries >= 4)
        //    {
        //        newPos = new Vector2(50,50);
        //        System.Array.Clear(neighbors,0,neighbors.Length);
        //    }
        //} while (neighbors.Length > 0 || tries < 3);
        return newPos;
    }

    private void spawnSpecItem(int index, Vector2 roomCenterPoint, float specPos_x_delta, float specPos_y_delta1, float specPos_y_delta2)
    {
        float specPos_x = Random.Range(roomCenterPoint.x - specPos_x_delta, roomCenterPoint.x + specPos_x_delta);
        float specPos_y = Random.Range(roomCenterPoint.y - specPos_y_delta1, roomCenterPoint.y + specPos_y_delta2);
        Vector2 specPos = new Vector2(specPos_x, specPos_y);

        GameObject specialItem = (GameObject)Instantiate(currentSpecialItems[index], specPos, Quaternion.identity);
        GetComponent<MissionManager>().AddMissionObjective(specialItem);    

        if (_SpriteSorter != null)
        {
            _SpriteSorter.AddToAllLists(specialItem.GetComponent<SpriteRenderer>());
        }
    }

    private void initializeMissionManager()
    {
        GetComponent<MissionManager>().Initialize();
    }
}
