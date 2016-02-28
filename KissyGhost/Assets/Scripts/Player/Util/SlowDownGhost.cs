using UnityEngine;
using System.Collections;

public class SlowDownGhost : MonoBehaviour 
{
    Ghost parent;

    void Awake()
    {
        parent = GetComponentInParent<Ghost>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Furniture")
            parent.TouchingFurniture = true;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Furniture" && !parent.TouchingFurniture)
            OnTriggerEnter2D(col);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Furniture")
            parent.TouchingFurniture = false;
    }
}
