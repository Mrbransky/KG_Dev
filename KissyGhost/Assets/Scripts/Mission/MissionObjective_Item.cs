using UnityEngine;
using System.Collections;

public class MissionObjective_Item : MonoBehaviour
{
    public int MissionObjectiveListIndex = -1;
    private bool isItemPlacedDown = true;
    private bool hasBeenPickedUpBefore = false;

    public SpriteRenderer Item_Outline;

    private bool isHighlighted = true;
    private Animator animator;
    private RoomGenerator _RoomGenerator;
    
    public bool IsItemPlacedDown
    {
        get { return isItemPlacedDown; }
    }

    void Start()
    {
        _RoomGenerator = GameObject.FindGameObjectWithTag("GameManager").GetComponent<RoomGenerator>();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (isItemPlacedDown && hasBeenPickedUpBefore)
        {
            MissionObjective_ItemNode itemNodeScript = col.GetComponent<MissionObjective_ItemNode>();

            if (itemNodeScript != null && !itemNodeScript.HasItem)
            {
                itemNodeScript.HasItem = true;
                Item_Outline.gameObject.SetActive(false);
                transform.position = itemNodeScript.ItemTargetPosition;
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionManager>().OnMissionObjectiveCompleted(MissionObjectiveListIndex);

                //Item in Heartagram Sound
                soundManager.SOUND_MAN.playSound("Play_Item_In_Heartagram", gameObject);

                if (GetComponent<ThrowableItem>() != null)
                {
                    Destroy(GetComponent<ThrowableItem>());
                }

                Destroy(GetComponent<Collider2D>());
            }
        }
    }

    public void PickItemUp()
    {
        isItemPlacedDown = false;
        hasBeenPickedUpBefore = true;
        turnHighlightOff();
    }

    public void PlaceItemDown()
    {
        isItemPlacedDown = true;
        transform.position = _RoomGenerator.RepositionItemIfOutOfBounds(transform.position);
        ResetColor();
        turnHighlightOn();
    }

    private void turnHighlightOn()
    {
        Item_Outline.color = Color.white;
    }

    private void turnHighlightOff()
    {
        Color transparent = Item_Outline.color;
        transparent.a = 0;
        Item_Outline.color = transparent;
    }

    public void SetColor(Color playerColor)
    {
        if (Item_Outline.color == Color.white)
        {
            Item_Outline.color = playerColor;
        }
    }

    public void ResetColor()
    {
        Color newColor = Color.white;
        newColor.a = Item_Outline.color.a;
        Item_Outline.color = newColor;
    }
}
