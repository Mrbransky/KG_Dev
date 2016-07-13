using UnityEngine;
using System.Collections;

public class CharacterSelectData : MonoBehaviour
{
    [SerializeField] private bool[] isPlayerReadyArray;
    [SerializeField] private int playerCount = 0;
    [SerializeField] private int ghostPlayerIndex = -1;
    [SerializeField] private bool isDebugData = false;
    [SerializeField] private ColorPalette[] playerPaletteArray;
    [SerializeField] private Sprite[] startingSprites;
    [SerializeField] private bool[] isFemaleCharacter;
    
    public bool[] IsPlayerReadyArray
    {
        get { return isPlayerReadyArray; }
    }

    public int PlayerCount
    {
        get { return playerCount; }
    }

    public int GhostPlayerIndex
    {
        get { return ghostPlayerIndex; }
    }

    public ColorPalette[] PlayerPaletteArray
    {
        get { return playerPaletteArray; }
    }

    public Sprite[] StartingSprites
    {
        get { return startingSprites; }
    }

    public bool[] IsFemaleCharacter
    {
        get { return isFemaleCharacter; }
    }

    void Start ()
    {
        if (isDebugData && GameObject.FindGameObjectsWithTag("CharacterSelectData").Length > 1)
        {
            Destroy(gameObject);
        }

        playerPaletteArray = new ColorPalette[4];
        startingSprites = new Sprite[4];
        isFemaleCharacter = new bool[4];

        DontDestroyOnLoad(gameObject);
	}

    public void SetIsPlayerReady(bool[] _isPlayerReadyArray, int _playerCount, int _ghostPlayerIndex)
    {
        isPlayerReadyArray = _isPlayerReadyArray;
        playerCount = _playerCount;
        ghostPlayerIndex = _ghostPlayerIndex;
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

    public void LoadPaletteArray(PaletteSwapper[] palettes)
    {
        for (int i = 0; i < palettes.Length; i++)
        {
            playerPaletteArray[i] = palettes[i].currentPalette;
            Debug.Log(palettes[i].currentPalette + " loaded");
        }
    }

    public void LoadStartSpritesArray(Sprite[] sprites)
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            startingSprites[i] = sprites[i];
        }
    }

    public void LoadIsFemaleBoolArray(Sprite Oldie, Sprite Woman)
    {
        for (int i = 0; i < startingSprites.Length; i++)
        {
            if (startingSprites[i] == Oldie)
                isFemaleCharacter[i] = false;
            else if (startingSprites[i] == Woman)
                isFemaleCharacter[i] = true;
        }   
    }
}
