using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class OffsetScroller : MonoBehaviour
{

    public float scrollSpeed;
    private Vector2 savedOffset;
    public bool goLeft = false;

    void Start()
    {
        savedOffset = new Vector2(GetComponent<RawImage>().uvRect.x, GetComponent<RawImage>().uvRect.y);
    }

    void Update()
    {
        float y = Mathf.Repeat(Time.time * scrollSpeed, 1);

        Vector2 offsetY = new Vector2(savedOffset.x, y);
        Vector2 offsetX = new Vector2(y, savedOffset.y);
        if (goLeft)
        {
            Vector2 combo = offsetX - offsetY;
            Rect rec = GetComponent<RawImage>().uvRect;
            rec.position = combo;
            GetComponent<RawImage>().uvRect = rec;
        }
        else
        {
            Vector2 combo = offsetX + offsetY;
            Rect rec = GetComponent<RawImage>().uvRect;
            rec.position = combo;
            GetComponent<RawImage>().uvRect = rec;
        }
        
    }

    void OnDisable()
    {
        Rect rec = GetComponent<RawImage>().uvRect;
        rec.position = savedOffset;
        GetComponent<RawImage>().uvRect = rec;
    }
}
