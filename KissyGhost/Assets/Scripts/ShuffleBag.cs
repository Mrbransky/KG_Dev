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
        if (Size != 4)
        {
            data = new List<int>();

            for (int i = 0; i < 4; i++)
                shuffle.Add(i, 1);
        }

        DontDestroyOnLoad(this);
    }

    private System.Random random = new System.Random();
    public List<int> data;

    private int currentItem;
    private int currentPosition = -1;

    private int Capacity { get { return data.Capacity; } }
    public int Size { get { return data.Count; } }

    public void Add(int item, int amount)
    {
        for (int i = 0; i < amount; i++)
            data.Add(item);

        currentPosition = Size - 1;
    }

    public int Next()
    {
        if(currentPosition < 1)
        {
            currentPosition = Size - 1;
            currentItem = data[0];

            return currentItem;
        }

        int pos = random.Next(currentPosition);

        currentItem = data[pos];
        data[pos] = data[currentPosition];
        data[currentPosition] = currentItem;
        currentPosition--;

        return currentItem;
    }

	
}
