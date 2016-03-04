using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XInputDotNetPure;

public class Human : Player 
{
    [SerializeField] private float invulnerabilityDuration = 1.5f;

    private int hugPoints = 0;
    private int hugLimit = 3;

    private GameObject[] heartObjects;
    private float timeSinceInvulnerable = -1;
    private Color defaultSpriteColor;
    private Color invulnSpriteColor;

    public bool GetAButtonDown = false;
    private bool wasAButtonPressed = false;

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

    private string HeldItemName;
    private SpriteRenderer mySpriteRenderer;
    private SpriteRenderer heldItemSpriteRenderer;

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

        mySpriteRenderer = GetComponent<SpriteRenderer>();
        defaultSpriteColor = mySpriteRenderer.color;
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
        hugPoints = hugLimit;

        foreach (HeartComponent heartComponent in GetComponentsInChildren<HeartComponent>())
        {
            if (heartComponent.heartNum < hugLimit)
            {
                heartObjects[heartComponent.heartNum] = heartComponent.gameObject;
                //heartComponent.GetComponent<Image>().enabled = false;
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
        // Handling sort order in SpriteSorter.cs
        // gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int)(-transform.localPosition.y+1);

        GetAButtonDown = false;

        if (InputMapper.GrabVal(XBOX360_BUTTONS.A, this.playerNum) && !wasAButtonPressed)
        {
            wasAButtonPressed = true;
            GetAButtonDown = true;
        }
        else if (!InputMapper.GrabVal(XBOX360_BUTTONS.A, this.playerNum) && wasAButtonPressed)
        {
            wasAButtonPressed = false;
        }

        if (heldItemSpriteRenderer != null)
        {
            heldItemSpriteRenderer.sortingOrder = mySpriteRenderer.sortingOrder + 1;
        }
        
        if (IsCarryingItem && timeBetweenItemInteract == 0)
        {
            if (GetAButtonDown)
            {
                PutItemDown(HeldItemName);
            }
            //else if (InputMapper.GrabVal(XBOX360_BUTTONS.B, this.playerNum))
            //{
            //    ThrowItem(HeldItemName);
            //}
#if UNITY_EDITOR
            else if (Input.GetKeyDown(ItemPickUpKeycode))
            {
                PutItemDown(HeldItemName);
            }
            //else if (Input.GetKeyDown(ItemThrowKeycode))
            //{
            //    ThrowItem(HeldItemName);
            //}
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
        obj.transform.localScale = new Vector3(1, 1, 1);
        heldItemSpriteRenderer = obj.GetComponent<SpriteRenderer>();
        heldItemSpriteRenderer.sortingOrder = mySpriteRenderer.sortingOrder + 1;
        obj.GetComponent<MissionObjective_Item>().PickItemUp();
        obj.GetComponent<Rigidbody2D>().isKinematic = true;
        HeldItemName = obj.name;

        timeBetweenItemInteract = 0.25f;

        //Item Pick up Sound
        soundManager.SOUND_MAN.playSound("Play_Item_Pick_Up", gameObject);
    }
    
    void PutItemDown(string itemName)
    {
        this.IsCarryingItem = false;
        Transform childTransform = heldItemSpriteRenderer.transform;
        childTransform.localPosition = new Vector3(interactTrigger.offset.x, interactTrigger.offset.y, 1);
        childTransform.parent = null;
        childTransform.GetComponent<MissionObjective_Item>().PlaceItemDown();
        HeldItemName = "";
        heldItemSpriteRenderer = null;

        timeBetweenItemInteract = 0.25f;

        //Put Item Down Sound
        if(!interactTrigger.GetComponent<MoveInteractTrigger>().IsOnItemNode)
            soundManager.SOUND_MAN.playSound("Play_Item_Down", gameObject);
    }

    void ThrowItem(string itemName)
    {
        ThrowableItem _ThrowableItem = gameObject.GetComponentInChildren<ThrowableItem>();
        if (_ThrowableItem != null)
        {
            this.IsCarryingItem = false;
            Transform childTransform = _ThrowableItem.transform;
            _ThrowableItem.lastParentVector = moveDir;

#if UNITY_EDITOR
            _ThrowableItem.lastParentVector = debugMoveDir;
#endif

            childTransform.parent = null;
            childTransform.GetComponent<MissionObjective_Item>().PlaceItemDown();

            _ThrowableItem.enabled = true;
            _ThrowableItem.ThrowItem(FacingRight);

            HeldItemName = "";

            timeBetweenItemInteract = 1;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (CanGrabItem && col.tag == "Cat")
        { 
            interactButtonPromptSpriteRenderer.enabled = true;
            timeSinceButtonPrompt = interactButtonPromptDurationBuffer;

            if (GetAButtonDown)
            {
                GrabItem(col.gameObject);
                interactButtonPromptSpriteRenderer.enabled = false;
                StartCoroutine(InputMapper.Vibration(playerNum, .2f, .15f, .5f));
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

            if(hugPoints > 0)
                StartCoroutine(InputMapper.Vibration(playerNum, 1, .55f, .7f));

            GetComponent<SpriteRenderer>().color = invulnSpriteColor;

            if (IsCarryingItem)
            {
                PutItemDown(HeldItemName);
            }
        }
    }

    private void gainHug()
    {
        --hugPoints;
        heartObjects[hugPoints].GetComponent<Image>().enabled = false;

        if (hugPoints <= 0)
        {
            killSelf();
        }
    }

    private void killSelf()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().OnHumanDead(gameObject, gameObject.GetComponent<SpriteRenderer>());
        Camera.main.GetComponent<NewCameraBehavior>().targets.Remove(gameObject);

        if (IsCarryingItem)
            PutItemDown(HeldItemName);

        Destroy(gameObject);
    }

    //public void AttachToPullSwitch(GameObject obj)
    //{
    //    IsPullingSwitch = true;
    //    Vector3 newPos = obj.transform.position;
    //    transform.position.Set(newPos.x, newPos.y, newPos.z);
    //}
}
