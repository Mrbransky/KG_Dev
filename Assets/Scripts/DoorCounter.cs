using UnityEngine;
using System.Collections;

public class DoorCounter : MonoBehaviour {

    public GameObject roomManager;
    void OnTriggerEnter2D(Collider2D col)
    {
        roomManager.GetComponent<RoomChangeManager>().playersGoingBottom.Add(col.gameObject);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        roomManager.GetComponent<RoomChangeManager>().playersGoingBottom.Remove(col.gameObject);
    }
}
