using UnityEngine;
using System.Collections;

public class _NewControls : MonoBehaviour {

    //These should maybe be in a character manager or something
    public GameObject Player1, Player2;

    enum PlayerType { Human, Ghost };
    public PlayerType characterSprite = PlayerType.Human;

    public Vector2 moveDir;
    public float moveSpeed;
    public bool IsFacingRight;

	void Awake() 
    {
	
	}
	
	void Update () 
    {
	
	}

}
