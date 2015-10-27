using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject Player1;
    public GameObject Player2;
	// Update is called once per frame
	void Update () {

        if (Player1.tag == "Ghost" && Player2.GetComponent<PlayerControls>().Health <= 0)
        {
            
            if (Player1.GetComponent<PlayerControls>().IsFacingRight != true)
            {
                Player1.GetComponent<PlayerControls>().IsFacingRight = true;
            }
            Player1.GetComponent<PlayerControls>().SwapCharacters();
        }
        if (Player2.tag == "Ghost" && Player1.GetComponent<PlayerControls>().Health <= 0)
        {
            
            if(Player2.GetComponent<PlayerControls>().IsFacingRight != true)
            {
                Player2.GetComponent<PlayerControls>().IsFacingRight = true;
            }
            Player2.GetComponent<PlayerControls>().SwapCharacters();
        }
	}
}
