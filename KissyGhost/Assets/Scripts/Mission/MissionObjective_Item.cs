using UnityEngine;
using System.Collections;

public class MissionObjective_Item : MonoBehaviour
{
    public int MissionObjectiveListIndex = -1;
    public bool IsItemPlacedDown = false;

    public bool isAnimated = false;
    public Sprite HighlightedSprite;
    public Sprite UnhighlightedSprite;

    private bool isHighlighted = true;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    
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
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (IsItemPlacedDown)
        {
            MissionObjective_ItemNode itemNodeScript = col.GetComponent<MissionObjective_ItemNode>();

            if (itemNodeScript != null && !itemNodeScript.HasItem)
            {
                itemNodeScript.HasItem = true;
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionManager>().OnMissionObjectiveCompleted(MissionObjectiveListIndex);
                transform.position = itemNodeScript.ItemTargetPosition;
                turnHighlightOff();

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
