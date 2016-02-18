using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Human : Player 
{
    [SerializeField] private float invulnerabilityDuration = 1.5f;

    private int hugPoints = 0;
    private int hugLimit = 3;

    private GameObject[] heartObjects;
    private float timeSinceInvulnerable = -1;
    private Color defaultSpriteColor;
    private Color invulnSpriteColor;

    public bool IsCarryingItem;
    public bool CanGrabItem
    {
        get
        {
            if (!IsCarryingItem && timeBetweenItemInteract == 0)
                return true;
            else
                return false;
        }
    }
    public bool IsPullingSwitch;

    string HeldItemName;

    public float timeBetweenItemInteract;
    private SpriteRenderer interactButtonPromptSpriteRenderer;
    private float interactButtonPromptDurationBuffer = 0.1f;
    private float timeSinceButtonPrompt = 0.0f;

#if UNITY_EDITOR
    public KeyCode ItemPickUpKeycode = KeyCode.Z;
    public KeyCode ItemThrowKeycode = KeyCode.X;
#endif

    public override void Awake() 
    {
        FacingRight = false;
        IsPullingSwitch = false;

        defaultSpriteColor = GetComponent<SpriteRenderer>().color;
        invulnSpriteColor = defaultSpriteColor;
        invulnSpriteColor.a /= 2;

        base.Awake();

        GameManager _GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        GameObject characterSelectData = GameObject.FindGameObjectWithTag("CharacterSelectData");
        
        if (_GameManager != null && characterSelectData != null)
        {
            int playerCount = characterSelectData.GetComponent<CharacterSelectData>().PlayerCount;
            switch (playerCount)
            {
                case 2:
                    hugLimit = 5;
                    break;
                case 3:
                    hugLimit = 4;
                    break;
                case 4:
                    hugLimit = 3;
                    break;
            }
        }

        heartObjects = new GameObject[hugLimit];

        foreach (HeartComponent heartComponent in GetComponentsInChildren<HeartComponent>())
        {
            if (heartComponent.heartNum < hugLimit)
            {
                heartObjects[heartComponent.heartNum] = heartComponent.gameObject;
                heartComponent.GetComponent<Image>().enabled = false;
            }
            else
            {
                heartComponent.Disable();
            }
        }

        foreach (SpriteRenderer childSpriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            if (childSpriteRenderer.gameObject.name == "InteractButtonPrompt")
            {
                interactButtonPromptSpriteRenderer = childSpriteRenderer;
            }
        }
	}

    public override void Update() 
    {

        gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int)(-transform.localPosition.y+1);
        
        if (IsCarryingItem && timeBetweenItemInteract == 0)
        {
            if (InputMapper.GrabVal(XBOX360_BUTTONS.A, this.playerNum))
            {
                PutItemDown(HeldItemName);
            }
            else if (InputMapper.GrabVal(XBOX360_BUTTONS.B, this.playerNum))
            {
                ThrowItem(HeldItemName);
            }
#if UNITY_EDITOR
            else if (Input.GetKeyDown(ItemPickUpKeycode))
            {
                PutItemDown(HeldItemName);
            }
            else if (Input.GetKeyDown(ItemThrowKeycode))
            {
                ThrowItem(HeldItemName);
            }
#endif
        }

        if (timeBetweenItemInteract > 0)
            timeBetweenItemInteract -= Time.deltaTime;
        else if (timeBetweenItemInteract < 0)
            timeBetweenItemInteract = 0;

        if (timeSinceInvulnerable > 0)
        {
            timeSinceInvulnerable -= Time.deltaTime;

            if (timeSinceInvulnerable <= 0)
            {
                GetComponent<SpriteRenderer>().color = defaultSpriteColor;
            }
        }

        if (timeSinceButtonPrompt > 0)
        {
            timeSinceButtonPrompt -= Time.deltaTime;
            
            if (timeSinceButtonPrompt <= 0)
            {
                interactButtonPromptSpriteRenderer.enabled = false;
            }
        }

        base.Update();
	}

    public void GrabItem(GameObject obj)
    {
        this.IsCarryingItem = true;
        
        obj.transform.parent = transform;
        obj.transform.localPosition = new Vector3(0, .75f, 0);
        obj.transform.localScale = new Vector3(5, 5, 1);
        obj.GetComponent<SpriteRenderer>().sortingOrder = this.gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
        obj.GetComponent<MissionObjective_Item>().IsItemPlacedDown = false;
        HeldItemName = obj.name;

        timeBetweenItemInteract = 0.1f;
    }
    
    void PutItemDown(string itemName)
    {
        this.IsCarryingItem = false;
        Transform childTransform = transform.FindChild(itemName);
        childTransform.localPosition = new Vector3(interactTrigger.offset.x, interactTrigger.offset.y, 1);
        childTransform.transform.parent = null;
        childTransform.GetComponent<MissionObjective_Item>().IsItemPlacedDown = true;
        HeldItemName = "";

        timeBetweenItemInteract = 0.1f;
    }

    void ThrowItem(string itemName)
    {
        this.IsCarryingItem = false;
        Transform childTransform = transform.FindChild(itemName);
        childTransform.transform.parent = null;
        childTransform.GetComponent<MissionObjective_Item>().IsItemPlacedDown = true;

        ThrowableItem _ThrowableItem = childTransform.GetComponent<ThrowableItem>();
        if (_ThrowableItem != null)
        {
            _ThrowableItem.enabled = true;
            _ThrowableItem.ThrowItem(FacingRight);
        }

        HeldItemName = "";

        timeBetweenItemInteract = 1;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (CanGrabItem && col.tag == "Cat")
        { 
            interactButtonPromptSpriteRenderer.enabled = true;
            timeSinceButtonPrompt = interactButtonPromptDurationBuffer;

            if (InputMapper.GrabVal(XBOX360_BUTTONS.A, this.playerNum))
            {
                GrabItem(col.gameObject);
                interactButtonPromptSpriteRenderer.enabled = false;
            }
#if UNITY_EDITOR
            else if (Input.GetKeyDown(ItemPickUpKeycode))
            {
                GrabItem(col.gameObject);
                interactButtonPromptSpriteRenderer.enabled = false;
            }
#endif
        }

        //else if (col.tag == "Pull" && !IsPullingSwitch)
        //{
        //    if(InputMapper.GrabVal(XBOX360_BUTTONS.A, this.playerNum) || Input.GetKeyDown(ItemPickUpKeycode))
        //    {
        //        AttachToPullSwitch(col.gameObject);
        //    }
        //}
    }

    public void HugHuman()
    {
        if (timeSinceInvulnerable <= 0)
        {
            timeSinceInvulnerable = invulnerabilityDuration;
            gainHug();

            GetComponent<SpriteRenderer>().color = invulnSpriteColor;
        }
    }

    private void gainHug()
    {
        heartObjects[hugPoints].GetComponent<Image>().enabled = true;
        ++hugPoints;

        if (hugPoints >= hugLimit)
        {
            killSelf();
        }
    }

    private void killSelf()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().OnHumanDead(gameObject);
        Camera.main.GetComponent<NewCameraBehavior>().targets.Remove(gameObject);

        if (IsCarryingItem)
        {
            PutItemDown(HeldItemName);
        }

        Destroy(gameObject);
    }

    //public void AttachToPullSwitch(GameObject obj)
    //{
    //    IsPullingSwitch = true;
    //    Vector3 newPos = obj.transform.position;
    //    transform.position.Set(newPos.x, newPos.y, newPos.z);
    //}
}
