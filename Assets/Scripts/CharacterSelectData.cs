using UnityEngine;
using System.Collections;

public class CharacterSelectData : MonoBehaviour
{
    private bool[] isPlayerReadyArray;
    private int playerCount = 0;
    
    public bool[] IsPlayerReadyArray
    {
        get { return isPlayerReadyArray; }
    }

    public int PlayerCount
    {
        get { return playerCount; }
    }


    void Start ()
    {
        DontDestroyOnLoad(gameObject);
	}

    public void SetIsPlayerReady(bool[] _isPlayerReadyArray, int _playerCount)
    {
        isPlayerReadyArray = _isPlayerReadyArray;
        playerCount = _playerCount;
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
