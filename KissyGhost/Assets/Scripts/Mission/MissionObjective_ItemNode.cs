using UnityEngine;
using System.Collections;

public class MissionObjective_ItemNode : MonoBehaviour
{
    public Color HasItemColor = Color.white;
    public bool HasItem = false;

    public Vector3 ItemTargetPositionOffset = Vector3.zero;
    private Vector3 itemTargetPosition;
    private SpriteRenderer spriteRenderer;

    public Vector3 ItemTargetPosition
    {
        get { return itemTargetPosition; }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemTargetPosition = transform.position + ItemTargetPositionOffset;
    }

    void Update()
    {
        if (HasItem)
        {
            GetComponent<Animator>().SetBool("HasItem", true);
            spriteRenderer.color = HasItemColor;
        }
    }
}
