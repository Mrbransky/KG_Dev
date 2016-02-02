using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour {


	public GameObject BottomBaseRoomPiece;
	public GameObject LeftBaseRoomPiece;
    public GameObject RightBaseRoomPiece;
	public GameObject MainBaseRoomPiece;
	List<GameObject> currentFurniture;
	public int numberOfFurniture = 3;
    public GameObject[] furnitureOptions;

    void Start()
    {
        GenerateInternals(MainBaseRoomPiece.transform.localScale);
    }
	public void GenerateInternals(Vector2 RoomSize)
	{
        int NoF = numberOfFurniture;
		currentFurniture = new List<GameObject>();

		for(int i = 0; i<NoF; i++)
		{
			Vector2 newPos = new Vector2(Random.Range (-RoomSize.x,RoomSize.x),Random.Range (-RoomSize.y,RoomSize.y));
            GameObject newFurniture = Instantiate(furnitureOptions[Random.Range(0, furnitureOptions.Length)], newPos, Quaternion.identity) as GameObject;
            if (newFurniture.tag == "Furniture")
			{
                currentFurniture.Add(newFurniture);
				
			}
		}

	}
}
