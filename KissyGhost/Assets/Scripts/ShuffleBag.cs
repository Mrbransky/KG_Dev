using System;
using UnityEngine;
using System.Collections.Generic;

//Reference: http://gamedevelopment.tutsplus.com/tutorials/shuffle-bags-making-random-feel-more-random--gamedev-1249

public class ShuffleBag : MonoBehaviour
{
    public static ShuffleBag shuffle
    {
        get;
        private set;
    }

    void Awake()
    {
        if (shuffle == null)
            shuffle = this;

        else if (shuffle != this)
            Destroy(gameObject);
    }

    void Start()
    {
        if (playerDataSize != 4)
        {
            playerData = new List<int>();

            for (int i = 0; i < 4; i++)
                shuffle.AddToPlayerData(i, 1);
        }

        if (shootingStarDataSize != 8)
        {
            shootingStarData = new List<int>();

            for (int i = 2; i < 8; i++)
                shuffle.AddToStarData(i, 1);
        }

        DontDestroyOnLoad(this);
    }

    private System.Random random = new System.Random();

    public List<int> playerData;
    public List<int> shootingStarData;

    private int currentPlayerDataItem;
    private int currentShootingStarItem;

    private int currentPlayerDataPosition = -1;
    private int currentStarDataPosition = -1;

    public int playerDataSize { get { return playerData.Count; } }
    public int shootingStarDataSize { get { return shootingStarData.Count; } }

    public void AddToPlayerData(int item, int amount)
    {
        for (int i = 0; i < amount; i++)
            playerData.Add(item);

        currentPlayerDataPosition = playerDataSize - 1;
    }

    public void AddToStarData(int item, int amount)
    {
        for (int i = 0; i < amount; i++)
            shootingStarData.Add(item);

        currentStarDataPosition = shootingStarDataSize - 1;
    }

    public int PlayerListNext()
    {
        if(currentPlayerDataPosition < 1)
        {
            currentPlayerDataPosition = playerDataSize - 1;
            currentPlayerDataItem = playerData[0];

            return currentPlayerDataItem;
        }

        int pos = random.Next(currentPlayerDataPosition);

        currentPlayerDataItem = playerData[pos];
        playerData[pos] = playerData[currentPlayerDataPosition];
        playerData[currentPlayerDataPosition] = currentPlayerDataItem;
        currentPlayerDataPosition--;

        return currentPlayerDataItem;
    }

    public int StarListNext()
    {
        if (currentStarDataPosition < 1)
        {
            currentStarDataPosition = shootingStarDataSize - 1;
            currentShootingStarItem = shootingStarData[0];

            return currentShootingStarItem;
        }

        int pos = random.Next(currentStarDataPosition);

        currentShootingStarItem = shootingStarData[pos];
        shootingStarData[pos] = shootingStarData[currentStarDataPosition];
        shootingStarData[currentStarDataPosition] = currentShootingStarItem;
        currentStarDataPosition--;

        return currentShootingStarItem;

    }


}
