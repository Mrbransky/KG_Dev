﻿using UnityEngine;
using System.Collections;

public class DoorCounter : MonoBehaviour {

    public GameObject roomManager;
    bool isWaiting = false;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (isWaiting == false && col.gameObject.tag == "Player")
        {
            roomManager.GetComponent<RoomChangeManager>().playersGoingBottom.Add(col.gameObject);
            isWaiting = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (isWaiting == true && col.gameObject.tag == "Player")
        {
            roomManager.GetComponent<RoomChangeManager>().playersGoingBottom.Remove(col.gameObject);
            isWaiting = false;
        }  
    }
}
