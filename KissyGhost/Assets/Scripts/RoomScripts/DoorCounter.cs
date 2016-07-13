using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DoorCounter : MonoBehaviour {

    public GameObject roomManager;
    bool isWaiting = false;

    public enum doorSwitchState { LEFT, RIGHT, BOTTOM, BACK };
    public doorSwitchState curState;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            switch(curState)
            {
                case doorSwitchState.LEFT:
                    if(!roomManager.GetComponent<RoomChangeManager>().playersGoingLeft.Contains(col.gameObject))
                        roomManager.GetComponent<RoomChangeManager>().playersGoingLeft.Add(col.gameObject);
                    break;
                case doorSwitchState.RIGHT:
                    if(!roomManager.GetComponent<RoomChangeManager>().playersGoingRight.Contains(col.gameObject))
                    roomManager.GetComponent<RoomChangeManager>().playersGoingRight.Add(col.gameObject);
                    break;
                case doorSwitchState.BOTTOM:
                    if (!roomManager.GetComponent<RoomChangeManager>().playersGoingBottom.Contains(col.gameObject))
                    roomManager.GetComponent<RoomChangeManager>().playersGoingBottom.Add(col.gameObject);
                    break;
                case doorSwitchState.BACK:
                    if (!roomManager.GetComponent<RoomChangeManager>().playersGoingBack.Contains(col.gameObject))
                    roomManager.GetComponent<RoomChangeManager>().playersGoingBack.Add(col.gameObject);
                    break;
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            switch (curState)
            {
                case doorSwitchState.LEFT:
                    roomManager.GetComponent<RoomChangeManager>().playersGoingLeft.Remove(col.gameObject);
                    break;
                case doorSwitchState.RIGHT:
                    roomManager.GetComponent<RoomChangeManager>().playersGoingRight.Remove(col.gameObject);
                    break;
                case doorSwitchState.BOTTOM:
                    roomManager.GetComponent<RoomChangeManager>().playersGoingBottom.Remove(col.gameObject);
                    break;
                case doorSwitchState.BACK:
                    roomManager.GetComponent<RoomChangeManager>().playersGoingBack.Remove(col.gameObject);
                    break;
            }
        }  
    }
}
