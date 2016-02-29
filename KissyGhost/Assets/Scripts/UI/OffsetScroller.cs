using UnityEngine;
using System.Collections;

public class OffsetScroller : MonoBehaviour
{

    public float scrollSpeed;
    private Vector2 savedOffset;
    public bool goLeft = false;

    void Start()
    {
        savedOffset = GetComponent<Renderer>().sharedMaterial.GetTextureOffset("_MainTex");
    }

    void Update()
    {
        float y = Mathf.Repeat(Time.time * scrollSpeed, 1);

        Vector2 offsetY = new Vector2(savedOffset.x, y);
        Vector2 offsetX = new Vector2(y, savedOffset.y);
        if (goLeft)
        {
            Vector2 combo = offsetX - offsetY;
            GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", combo);
        }
        else
        {
            Vector2 combo = offsetX + offsetY;
            GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", combo);
        }
        
    }

    void OnDisable()
    {
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }
}
