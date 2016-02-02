using UnityEngine;
using System.Collections;

public class KissableFurniture : MonoBehaviour
{
    private bool isKissed = false;
    
    public void KissFurniture()
    {
        isKissed = true;
        OnFurnitureKissed();
    }

    private void OnFurnitureKissed()
    {
        GetComponent<SpriteRenderer>().color = new Color(255.0f / 255.0f, 192.0f / 255.0f, 203.0f / 255.0f);
    }

    public void UnkissFurniture()
    {
        isKissed = false;
        OnFurnitureUnkissed();
    }

    private void OnFurnitureUnkissed()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
