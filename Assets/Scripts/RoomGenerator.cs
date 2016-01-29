using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour {


	public GameObject floorPiece;
	public GameObject SmallBaseRoomPiece;
	public GameObject MediumBaseRoomPiece;
	public GameObject LargeBaseRoomPiece;
	List<GameObject> currentFloors;
	public int numberOfFloors = 3;
    public GameObject[] floorOptions;
	
	void Start()
	{
		SmallBaseRoomPiece.SetActive(false);
		MediumBaseRoomPiece.SetActive(false);
		LargeBaseRoomPiece.SetActive(true);
		GenerateInternals(new Vector2(5.0f, 2.5f));
	}
	public void GenerateInternals(Vector2 RoomSize)
	{
		int NoF = numberOfFloors;
		currentFloors = new List<GameObject>();

		for(int i = 0; i<NoF; i++)
		{
			Vector2 newPos = new Vector2(Random.Range (-RoomSize.x,RoomSize.x),Random.Range (-RoomSize.y,RoomSize.y));
			GameObject newFloor = Instantiate(floorOptions[Random.Range (0,floorOptions.Length)],newPos,Quaternion.identity) as GameObject;
			if(newFloor.tag == "Furniture")
			{
			currentFloors.Add(newFloor);
				
			}
		}

	}
	void RemoveFloors()
	{
		foreach  (GameObject floorsToDelete in currentFloors)
		{
			Destroy(floorsToDelete.gameObject);
		}
	}
	public void GenerateNewRoom()
	{
		RemoveFloors ();
		GenerateInternals(new Vector2(6f, 3f));
//		switch (Random.Range (1, 4)) {
//		case 1:
//			RemoveFloors();
//            GenerateInternals(new Vector2(4f, 4f));
//			break;
//		case 2:
//			RemoveFloors();
//            GenerateInternals(new Vector2(4f, 4f));
//			break;
//		case 3:
//			RemoveFloors();
//            GenerateInternals(new Vector2(4f, 4f));
//			break;
//		default:
//			break;
//		
//		}


	}
}
