using UnityEngine;
using System.Collections;

public class CharacterSelectData : MonoBehaviour
{
    private bool[] isPlayerReadyArray;
    
	void Start ()
    {
        DontDestroyOnLoad(gameObject);
	}

    public void SetIsPlayerReady(bool[] _isPlayerReadyArray)
    {
        isPlayerReadyArray = _isPlayerReadyArray;
    }

    public bool GetIsPlayerReady(int playerNumber)
    {
        // playerNumber represents its equivalent index number + 1
        --playerNumber;

        if (playerNumber >= 0 && playerNumber < isPlayerReadyArray.Length)
        {
            return isPlayerReadyArray[playerNumber];
        }
        else
        {
            Debug.LogError("CharacterSelectData: Array index \"playerNumber\" out of range.");
            return false;
        }
    }
}
