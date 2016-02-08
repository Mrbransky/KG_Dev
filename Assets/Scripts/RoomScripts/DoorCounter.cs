using UnityEngine;
using System.Collections;

public class DoorCounter : MonoBehaviour {

    public GameObject roomManager;
    bool isWaiting = false;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            roomManager.GetComponent<RoomChangeManager>().playersGoingBottom.Add(col.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            roomManager.GetComponent<RoomChangeManager>().playersGoingBottom.Remove(col.gameObject);
        }  
    }
}
