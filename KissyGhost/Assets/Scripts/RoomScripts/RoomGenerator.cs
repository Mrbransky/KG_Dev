using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public enum RoomLimits
{
    Min_X = 0,
    Max_X = 1,
    Min_Y = 2,
    Max_Y = 3,
    Total = 4
}

public enum RoomTypes
{
    Center = 0, // "Big"
    Left = 1,   // "Medium Left"
    Right = 2,  // "Medium Right"
    Bottom = 3, // "Small"
    Total = 4
}

public class RoomGenerator : MonoBehaviour 
{
	public GameObject BottomBaseRoomPiece;
	public GameObject LeftBaseRoomPiece;
    public GameObject RightBaseRoomPiece;
	public GameObject MainBaseRoomPiece;
    public SpriteSorter _SpriteSorter;
    private RoomChangeManager _RoomChangeManager;

    //Furniture list
    [Header("Furniture Count")]
	public List<GameObject> currentFurniture;
    public List<GameObject> leftFurniture;
    public List<GameObject> rightFurniture;
    public List<GameObject> bottomFurniture;
	public int numberOfFurnitureForCenter = 15;
    public int numberOfFurnitureForRight = 10;
    public int numberOfFurnitureForLeft = 10;
    public int numberOfFurnitureForBottom = 5;
    [Header("Furniture Options")]
    public GameObject[] furnitureOptions;
    public GameObject[] kitchenFurnitureOptions;
    public GameObject[] bathroomFurnitureOptions;
    public GameObject[] bedroomFurnitureOptions;
    public bool IsRugsEnabledInCenterRoom = true;

    //Special Items
    [Header("Special Items")]
    public List<GameObject> AllSpecialItems;
    public List<GameObject> AiTracker;
    private List<GameObject> currentSpecialItems;

    private float[] CenterRoomLimits;
    private float[] LeftRoomLimits;
    private float[] RightRoomLimits;
    private float[] BottomRoomLimits;

    //public float minDistance = 10;
    void Awake()
    {
        _RoomChangeManager = GetComponent<RoomChangeManager>();

        currentSpecialItems = new List<GameObject>();
        currentFurniture = new List<GameObject>();

        for (int i = 0; i < 4; i++)
        {
            int rand = Random.Range(0, AllSpecialItems.Count);
            currentSpecialItems.Add(AllSpecialItems[rand]);
            AllSpecialItems.Remove(AllSpecialItems[rand]);

            //DON'T YOU DARE TRY AND USE THIS
            //pls its so funny
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
        calculateRoomLimits(RoomTypes.Center);
        calculateRoomLimits(RoomTypes.Left);
        calculateRoomLimits(RoomTypes.Right);
        calculateRoomLimits(RoomTypes.Bottom);

        GenerateInternals(RoomTypes.Center);
        GenerateInternals(RoomTypes.Left);
        GenerateInternals(RoomTypes.Right);
        GenerateInternals(RoomTypes.Bottom);
        
        initializeMissionManager();

        if (_SpriteSorter != null)
        {
            _SpriteSorter.isInitialized = true;
        }
    }
    void Update()
    {
        currentFurniture = GameObject.FindGameObjectsWithTag("Furniture").ToList();
    }
    private void calculateRoomLimits(RoomTypes roomType)
    {
        Vector2 roomSize = Vector2.zero;
        Vector2 roomCenterPoint = Vector2.zero;

        switch (roomType)
        {
            case RoomTypes.Center:
                CenterRoomLimits = new float[(int)RoomLimits.Total];
                roomSize = MainBaseRoomPiece.GetComponent<SpriteRenderer>().sprite.bounds.size;
                roomCenterPoint = MainBaseRoomPiece.transform.position;

                CenterRoomLimits[(int)RoomLimits.Min_X] = roomCenterPoint.x - (roomSize.x - 2);
                CenterRoomLimits[(int)RoomLimits.Max_X] = roomCenterPoint.x + (roomSize.x - 2);
                CenterRoomLimits[(int)RoomLimits.Min_Y] = roomCenterPoint.y - (roomSize.y - 2);
                CenterRoomLimits[(int)RoomLimits.Max_Y] = roomCenterPoint.y + (roomSize.y - 6);
                break;
                
            case RoomTypes.Left:
                LeftRoomLimits = new float[(int)RoomLimits.Total];
                roomSize = LeftBaseRoomPiece.GetComponent<SpriteRenderer>().sprite.bounds.size;
                roomCenterPoint = LeftBaseRoomPiece.transform.position;

                LeftRoomLimits[(int)RoomLimits.Min_X] = roomCenterPoint.x - (roomSize.x - 5);
                LeftRoomLimits[(int)RoomLimits.Max_X] = roomCenterPoint.x + (roomSize.x - 5);
                LeftRoomLimits[(int)RoomLimits.Min_Y] = roomCenterPoint.y - (roomSize.y - 5);
                LeftRoomLimits[(int)RoomLimits.Max_Y] = roomCenterPoint.y + (roomSize.y - 7);
                break;

            case RoomTypes.Right:
                RightRoomLimits = new float[(int)RoomLimits.Total];
                roomSize = RightBaseRoomPiece.GetComponent<SpriteRenderer>().sprite.bounds.size;
                roomCenterPoint = RightBaseRoomPiece.transform.position;

                RightRoomLimits[(int)RoomLimits.Min_X] = roomCenterPoint.x - (roomSize.x - 5);
                RightRoomLimits[(int)RoomLimits.Max_X] = roomCenterPoint.x + (roomSize.x - 5);
                RightRoomLimits[(int)RoomLimits.Min_Y] = roomCenterPoint.y - (roomSize.y - 4);
                RightRoomLimits[(int)RoomLimits.Max_Y] = roomCenterPoint.y + (roomSize.y - 9);
                break;

            case RoomTypes.Bottom:
                BottomRoomLimits = new float[(int)RoomLimits.Total];
                roomSize = BottomBaseRoomPiece.GetComponent<SpriteRenderer>().sprite.bounds.size;
                roomCenterPoint = BottomBaseRoomPiece.transform.position;

                BottomRoomLimits[(int)RoomLimits.Min_X] = roomCenterPoint.x - (roomSize.x - 8);
                BottomRoomLimits[(int)RoomLimits.Max_X] = roomCenterPoint.x + (roomSize.x - 8);
                BottomRoomLimits[(int)RoomLimits.Min_Y] = roomCenterPoint.y - (roomSize.y - 8);
                BottomRoomLimits[(int)RoomLimits.Max_Y] = roomCenterPoint.y + (roomSize.y - 8);
                break;
        }
    }

    public void GenerateInternals(RoomTypes roomType)
	{
        switch ((int)roomType)
        {
            case (int)RoomTypes.Left:
                spawnSpecItem(0, LeftRoomLimits[(int)RoomLimits.Min_X], LeftRoomLimits[(int)RoomLimits.Max_X], LeftRoomLimits[(int)RoomLimits.Min_Y], LeftRoomLimits[(int)RoomLimits.Max_Y]);
                spawnFurniture(LeftBaseRoomPiece, LeftRoomLimits[(int)RoomLimits.Min_X], LeftRoomLimits[(int)RoomLimits.Max_X], LeftRoomLimits[(int)RoomLimits.Min_Y], LeftRoomLimits[(int)RoomLimits.Max_Y], roomType);
                break;

            case (int)RoomTypes.Right:
                spawnSpecItem(1, RightRoomLimits[(int)RoomLimits.Min_X], RightRoomLimits[(int)RoomLimits.Max_X], RightRoomLimits[(int)RoomLimits.Min_Y], RightRoomLimits[(int)RoomLimits.Max_Y]);
                spawnFurniture(RightBaseRoomPiece, RightRoomLimits[(int)RoomLimits.Min_X], RightRoomLimits[(int)RoomLimits.Max_X], RightRoomLimits[(int)RoomLimits.Min_Y], RightRoomLimits[(int)RoomLimits.Max_Y], roomType);
                break;

            case (int)RoomTypes.Bottom:
                spawnSpecItem(2, BottomRoomLimits[(int)RoomLimits.Min_X], BottomRoomLimits[(int)RoomLimits.Max_X], BottomRoomLimits[(int)RoomLimits.Min_Y], BottomRoomLimits[(int)RoomLimits.Max_Y]);
                spawnFurniture(BottomBaseRoomPiece, BottomRoomLimits[(int)RoomLimits.Min_X], BottomRoomLimits[(int)RoomLimits.Max_X], BottomRoomLimits[(int)RoomLimits.Min_Y], BottomRoomLimits[(int)RoomLimits.Max_Y], roomType);
                break;

            case (int)RoomTypes.Center:
                spawnSpecItem(3, CenterRoomLimits[(int)RoomLimits.Min_X], CenterRoomLimits[(int)RoomLimits.Max_X], CenterRoomLimits[(int)RoomLimits.Min_Y], CenterRoomLimits[(int)RoomLimits.Max_Y]);
                spawnFurniture(MainBaseRoomPiece, CenterRoomLimits[(int)RoomLimits.Min_X], CenterRoomLimits[(int)RoomLimits.Max_X], CenterRoomLimits[(int)RoomLimits.Min_Y], CenterRoomLimits[(int)RoomLimits.Max_Y], roomType);
                break;
        }
	}
    
    private void spawnFurniture(GameObject roomObject, float min_x, float max_x, float min_y, float max_y, RoomTypes roomType)
    {
        Vector2 newPos = Vector2.zero;
        int numberOfFurniture = 15;
        switch((int)roomType)
        {
            case (int)RoomTypes.Center:
                numberOfFurniture = numberOfFurnitureForCenter;
                break;
            case (int)RoomTypes.Left:
                numberOfFurniture = numberOfFurnitureForLeft;
                break;

            case (int)RoomTypes.Right:
                numberOfFurniture = numberOfFurnitureForRight;
                break;

            case (int)RoomTypes.Bottom:
                numberOfFurniture = numberOfFurnitureForBottom;
                break;
        }
        for (int i = 0; i < numberOfFurniture; i++)
        {
            GameObject FurnitureToSpawn = furnitureOptions[Random.Range(0, furnitureOptions.Length)];
            newPos = getFurniturePos(min_x, max_x, min_y, max_y);


            switch ((int)roomType)
            {
                case (int)RoomTypes.Left:
                    FurnitureToSpawn = bedroomFurnitureOptions[Random.Range(0, bedroomFurnitureOptions.Length)];
                    break;

                case (int)RoomTypes.Right:
                    FurnitureToSpawn = kitchenFurnitureOptions[Random.Range(0, kitchenFurnitureOptions.Length)];
                    break;

                case (int)RoomTypes.Bottom:
                    FurnitureToSpawn = bathroomFurnitureOptions[Random.Range(0, bathroomFurnitureOptions.Length)];
                    break;
            }

            GameObject newFurniture = (GameObject)Instantiate(FurnitureToSpawn, newPos, Quaternion.identity);

            switch ((int)roomType)
            {
                case (int)RoomTypes.Left:
                    leftFurniture.Add(newFurniture);
                    break;

                case (int)RoomTypes.Right:
                    rightFurniture.Add(newFurniture);
                    break;

                case (int)RoomTypes.Bottom:
                    bottomFurniture.Add(newFurniture);
                    break;
            }
            #region
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
            else if (IsRugsEnabledInCenterRoom || roomType != RoomTypes.Center)
            {
                newFurniture.GetComponent<SpriteRenderer>().sortingOrder = -20;
            }
            else
            {
                foreach(Transform child in newFurniture.transform)
                {
                    child.GetComponent<SpriteRenderer>().sortingOrder = (int)(-child.transform.localPosition.y);

                    if (_SpriteSorter != null)
                    {
                        switch ((int)roomType)
                        {
                            case (int)RoomTypes.Left:
                                _SpriteSorter.LeftRoom_SpriteRendererList.Add(child.GetComponent<SpriteRenderer>());
                                break;

                            case (int)RoomTypes.Right:
                                _SpriteSorter.RightRoom_SpriteRendererList.Add(child.GetComponent<SpriteRenderer>());
                                break;

                            case (int)RoomTypes.Bottom:
                                _SpriteSorter.BottomRoom_SpriteRendererList.Add(child.GetComponent<SpriteRenderer>());
                                break;

                            case (int)RoomTypes.Center:
                                _SpriteSorter.CenterRoom_SpriteRendererList.Add(child.GetComponent<SpriteRenderer>());
                                break;
                        }
                    }
                }
            }
            #endregion
            //currentFurniture.Add(newFurniture);
            newFurniture.transform.SetParent(roomObject.transform);
        }
    }

    private Vector2 getFurniturePos(float min_x, float max_x, float min_y, float max_y)
    {
        float newPos_x = Random.Range(min_x, max_x);
        float newPos_y = Random.Range(min_y, max_y);
        Vector2 newPos = new Vector2(newPos_x, newPos_y);

        //Broken for now
        //Vector2 newPos;
        //Collider2D[] neighbors;
        //int tries = 0;
        //do
        //{
        //    float newPos_x = Random.Range(min_x, max_x);
        //    float newPos_y = Random.Range(min_y, max_y);
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

    private void spawnSpecItem(int index, float min_x, float max_x, float min_y, float max_y)
    {
        float specPos_x = Random.Range(min_x, max_x);
        float specPos_y = Random.Range(min_y, max_y);
        Vector2 specPos = new Vector2(specPos_x, specPos_y);

        GameObject specialItem = (GameObject)Instantiate(currentSpecialItems[index], specPos, Quaternion.identity);
        GetComponent<MissionManager>().AddMissionObjective(specialItem);
        AiTracker.Add(specialItem);
        if (_SpriteSorter != null)
        {
            _SpriteSorter.AddToAllLists(specialItem.GetComponent<SpriteRenderer>());
        }
    }

    private void initializeMissionManager()
    {
        GetComponent<MissionManager>().Initialize();
    }

    public Vector2 RepositionItemIfOutOfBounds(Vector2 itemPosition)
    {
        Vector2 newPosition = itemPosition;
        float min_x = 0;
        float max_x = 0;
        float min_y = 0;
        float max_y = 0;

        switch (_RoomChangeManager.CurrentRoomLocation)
        {
            case RoomLocations.Center:
                min_x = CenterRoomLimits[(int)RoomLimits.Min_X] - 1.5f;
                max_x = CenterRoomLimits[(int)RoomLimits.Max_X] + 1.5f;
                min_y = CenterRoomLimits[(int)RoomLimits.Min_Y] - 1.5f;
                max_y = CenterRoomLimits[(int)RoomLimits.Max_Y] + 2.7f;
                break;

            case RoomLocations.Left:
                min_x = LeftRoomLimits[(int)RoomLimits.Min_X] - 1.3f;
                max_x = LeftRoomLimits[(int)RoomLimits.Max_X] + 1.3f;
                min_y = LeftRoomLimits[(int)RoomLimits.Min_Y] - 2.0f;
                max_y = LeftRoomLimits[(int)RoomLimits.Max_Y] + 0.0f;
                break;

            case RoomLocations.Right:
                min_x = RightRoomLimits[(int)RoomLimits.Min_X] - 1.1f;
                max_x = RightRoomLimits[(int)RoomLimits.Max_X] + 1.1f;
                min_y = RightRoomLimits[(int)RoomLimits.Min_Y] - 2.0f;
                max_y = RightRoomLimits[(int)RoomLimits.Max_Y] + 0.1f;
                break;

            case RoomLocations.Bottom:
                min_x = BottomRoomLimits[(int)RoomLimits.Min_X] - 0.8f;
                max_x = BottomRoomLimits[(int)RoomLimits.Max_X] + 0.8f;
                min_y = BottomRoomLimits[(int)RoomLimits.Min_Y] - 2.8f;
                max_y = BottomRoomLimits[(int)RoomLimits.Max_Y] + 1.0f;
                break;
        }

        if (newPosition.x < min_x)
        {
            newPosition.x = min_x;
        }
        else if (newPosition.x > max_x)
        {
            newPosition.x = max_x;
        }

        if (newPosition.y < min_y)
        {
            newPosition.y = min_y;
        }
        else if (newPosition.y > max_y)
        {
            newPosition.y = max_y;
        }

        return newPosition;
    }
}
