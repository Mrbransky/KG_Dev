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
	//public float minDistance = 1f;
    public Sprite[] floorOptions;
	
	void Start()
	{
		SmallBaseRoomPiece.SetActive(false);
		MediumBaseRoomPiece.SetActive(false);
		LargeBaseRoomPiece.SetActive(true);
        GenerateInternals(new Vector2(4f, 4f));
	}
	public void GenerateInternals(Vector2 RoomSize)
	{
		int NoF = numberOfFloors;
		currentFloors = new List<GameObject>();
		//Vector3 InternalDimension = Vector3(RoomSize.x - 10f, RoomSize.y - 10f);

		for(int i = 0; i<NoF; i++)
		{
            floorPiece.GetComponent<SpriteRenderer>().sprite = floorOptions[Random.Range(0, floorOptions.Length)];
			Vector2 newPos = new Vector2(Random.Range (-RoomSize.x -2,RoomSize.x-2),Random.Range (-RoomSize.y-2,RoomSize.y-2));
			//Vector2 randPos = new Vector2(Random.Range (-RoomSize.x,RoomSize.x),Random.Range (-RoomSize.y,RoomSize.y));
			GameObject newFloor = Instantiate(floorPiece,newPos,Quaternion.identity) as GameObject;
			currentFloors.Add(newFloor);
		}

	}
	/*Vector3 FindNewPos(Vector2 roomSize)
	{
		Collider[] neighbors;
		Vector3 newPos;
		do{
			newPos = new Vector2(Random.Range (-roomSize.x,roomSize.x),Random.Range (-roomSize.y,roomSize.y));

			neighbors = Physics.OverlapSphere(newPos, minDistance);
		}
		while(neighbors.Length > 0);
		return newPos;

	}*/
	void RemoveFloors()
	{
		foreach  (GameObject floorsToDelete in currentFloors)
		{
			Destroy(floorsToDelete.gameObject);
		}
	}
	public void GenerateNewRoom()
	{
		switch (Random.Range (1, 4)) {
		case 1:
			RemoveFloors();
            GenerateInternals(new Vector2(4f, 4f));
			break;
		case 2:
			RemoveFloors();
            GenerateInternals(new Vector2(4f, 4f));
			break;
		case 3:
			RemoveFloors();
            GenerateInternals(new Vector2(4f, 4f));
			break;
		default:
			break;
		
		}


	}
}
