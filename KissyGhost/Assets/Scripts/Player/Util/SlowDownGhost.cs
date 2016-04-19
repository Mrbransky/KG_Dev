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
        if (col.tag == "Furniture")
        {
            parent.TouchingFurniture = true;
            parent.AddColliderToList(col);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Furniture" && !parent.TouchingFurniture)
            parent.TouchingFurniture = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Furniture")
        {
            parent.TouchingFurniture = false;
            parent.RemoveColliderFromList(col);
        }
    }
}
