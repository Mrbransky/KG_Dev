using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SpriteSorter : MonoBehaviour
{
    public class TransformYComparer : IComparer<SpriteRenderer>
    {
        int IComparer<SpriteRenderer>.Compare(SpriteRenderer first, SpriteRenderer second)
        {
            return ((new CaseInsensitiveComparer()).Compare(
                second.transform.position.y - second.sprite.bounds.extents.y / 2,
                first.transform.position.y - first.sprite.bounds.extents.y / 2));
        }
    }

    public RoomChangeManager _RoomChangeManager;
    
    public List<SpriteRenderer> CenterRoom_SpriteRendererList;
    public List<SpriteRenderer> LeftRoom_SpriteRendererList;
    public List<SpriteRenderer> RightRoom_SpriteRendererList;
    public List<SpriteRenderer> BottomRoom_SpriteRendererList;
    public bool isInitialized = false;

    private IComparer<SpriteRenderer> myComparer;
    private int sortOrderOffset = 10;
    
	void Awake()
    {
        if (CenterRoom_SpriteRendererList == null)
        {
            CenterRoom_SpriteRendererList = new List<SpriteRenderer>();
        }

        if (LeftRoom_SpriteRendererList == null)
        {
            LeftRoom_SpriteRendererList = new List<SpriteRenderer>();
        }

        if (RightRoom_SpriteRendererList == null)
        {
            RightRoom_SpriteRendererList = new List<SpriteRenderer>();
        }

        if (BottomRoom_SpriteRendererList == null)
        {
            BottomRoom_SpriteRendererList = new List<SpriteRenderer>();
        }

        myComparer = new TransformYComparer();
	}
	
	void Update()
    {
	    if (isInitialized)
        {
            updateSortOrder();
        }
	}

    public void AddToAllLists(SpriteRenderer srToAdd)
    {
        CenterRoom_SpriteRendererList.Add(srToAdd);
        LeftRoom_SpriteRendererList.Add(srToAdd);
        RightRoom_SpriteRendererList.Add(srToAdd);
        BottomRoom_SpriteRendererList.Add(srToAdd);
    }

    public void RemoveFromAllLists(SpriteRenderer srToRemove)
    {
        CenterRoom_SpriteRendererList.Remove(srToRemove);
        LeftRoom_SpriteRendererList.Remove(srToRemove);
        RightRoom_SpriteRendererList.Remove(srToRemove);
        BottomRoom_SpriteRendererList.Remove(srToRemove);
    }

    private void updateSortOrder()
    {
        int listCount = 0;

        switch ((int)_RoomChangeManager.CurrentRoomLocation)
        {
            case (int)RoomLocations.Center:
                CenterRoom_SpriteRendererList.Sort(myComparer);
                listCount = CenterRoom_SpriteRendererList.Count;

                for (int i = 0; i < listCount; ++i)
                {
                    CenterRoom_SpriteRendererList[i].sortingOrder = i + sortOrderOffset;
                }
                break;

            case (int)RoomLocations.Left:
                LeftRoom_SpriteRendererList.Sort(myComparer);
                listCount = LeftRoom_SpriteRendererList.Count;

                for (int i = 0; i < listCount; ++i)
                {
                    LeftRoom_SpriteRendererList[i].sortingOrder = i + sortOrderOffset;
                }
                break;

            case (int)RoomLocations.Right:
                RightRoom_SpriteRendererList.Sort(myComparer);
                listCount = RightRoom_SpriteRendererList.Count;

                for (int i = 0; i < listCount; ++i)
                {
                    RightRoom_SpriteRendererList[i].sortingOrder = i + sortOrderOffset;
                }
                break;

            case (int)RoomLocations.Bottom:
                BottomRoom_SpriteRendererList.Sort(myComparer);
                listCount = BottomRoom_SpriteRendererList.Count;

                for (int i = 0; i < listCount; ++i)
                {
                    BottomRoom_SpriteRendererList[i].sortingOrder = i + sortOrderOffset;
                }
                break;
        }
    }
}
