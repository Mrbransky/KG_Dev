using UnityEngine;
using System.Collections;

public class MissionObjective_Item : MonoBehaviour
{
    public int MissionObjectiveListIndex = -1;
    private bool isItemPlacedDown = true;
    private bool hasBeenPickedUpBefore = false;

    public bool isAnimated = false;
    public Sprite HighlightedSprite;
    public Sprite UnhighlightedSprite;

    private bool isHighlighted = true;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private RoomGenerator _RoomGenerator;
    
    public bool IsItemPlacedDown
    {
        get { return isItemPlacedDown; }
    }

    void Start()
    {
        if (isAnimated)
        {
            animator = GetComponent<Animator>();
        }
        else
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

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
                turnHighlightOff();
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
    }

    public void PlaceItemDown()
    {
        isItemPlacedDown = true;
        transform.position = _RoomGenerator.RepositionItemIfOutOfBounds(transform.position);
    }

    private void turnHighlightOn()
    {
        if (isAnimated)
        {
            animator.SetBool("turnHighlightOn", true);
            animator.SetBool("turnHighlightOff", false);
        }
        else
        {
            spriteRenderer.sprite = HighlightedSprite;
        }
    }

    private void turnHighlightOff()
    {
        if (isAnimated)
        {
            animator.SetBool("turnHighlightOff", true);
            animator.SetBool("turnHighlightOn", false);
        }
        else
        {
            spriteRenderer.sprite = UnhighlightedSprite;
        }
    }
}
