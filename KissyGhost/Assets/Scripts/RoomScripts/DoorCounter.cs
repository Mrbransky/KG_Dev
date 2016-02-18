using UnityEngine;
using System.Collections;

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
                    roomManager.GetComponent<RoomChangeManager>().playersGoingLeft.Add(col.gameObject);
                    break;
                case doorSwitchState.RIGHT:
                    roomManager.GetComponent<RoomChangeManager>().playersGoingRight.Add(col.gameObject);
                    break;
                case doorSwitchState.BOTTOM:
                    roomManager.GetComponent<RoomChangeManager>().playersGoingBottom.Add(col.gameObject);
                    break;
                case doorSwitchState.BACK:
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
