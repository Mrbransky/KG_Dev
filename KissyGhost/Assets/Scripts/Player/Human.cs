using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XInputDotNetPure;
using System.Collections.Generic;

public class Human : Player 
{
    [SerializeField] private float invulnerabilityDuration = 1.5f;

    private int hugPoints = 0;
    private int hugLimit = 3;

    private GameObject[] heartObjects;
    private float timeSinceInvulnerable = -1;

    public bool IsFemaleWizard;
    public bool GetAButtonDown = false;
    private bool wasAButtonPressed = false;
    public bool GetBButtonDown = false;
    private bool wasBButtonPressed = false;

    public bool IsCarryingItem;
    public float priority;
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
    public bool CanKickFurniture
    {
        get
        {
            if (!IsCarryingItem && timeBetweenFurnitureKick == 0)
                return true;
            else
                return false;
        }
    }
    public bool IsPullingSwitch;

    private string HeldItemName;
    private SpriteRenderer mySpriteRenderer;
    private SpriteRenderer heldItemSpriteRenderer;
    private HumanSpriteFlasher _HumanSpriteFlasher;

    public float kickForce = 1000.0f;
    public float timeBetweenItemInteract = 0;
    public float timeBetweenFurnitureKick = 0;
    public Color MainColor;
    //private SpriteRenderer interactButtonPromptSpriteRenderer;
    //private SpriteRenderer kickButtonPromptSpriteRenderer;
    //private float interactButtonPromptDurationBuffer = 0.1f;
    //private float timeSinceInteractButtonPrompt = 0.0f;
    //private float timeSinceKickButtonPrompt = 0.0f;

    private List<Collider2D> furnitureToKick;

#if UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE
    public KeyCode ItemPickUpKeycode = KeyCode.Z;
    public KeyCode ItemThrowKeycode = KeyCode.X;
#endif

    public override void Awake() 
    {
        currentAcelRate = NormalAccelRate;
        currentTopSpeed = NormalTopSpeed;

        FacingRight = false;
        IsPullingSwitch = false;

        mySpriteRenderer = GetComponent<SpriteRenderer>();
        _HumanSpriteFlasher = GetComponent<HumanSpriteFlasher>();

        furnitureToKick = new List<Collider2D>();

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

        //foreach (SpriteRenderer childSpriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        //{
        //    if (childSpriteRenderer.gameObject.name == "InteractButtonPrompt")
        //    {
        //        interactButtonPromptSpriteRenderer = childSpriteRenderer;
        //    }
        //    else if (childSpriteRenderer.gameObject.name == "KickButtonPrompt")
        //    {
        //        kickButtonPromptSpriteRenderer = childSpriteRenderer;
        //    }
        //}
	}

    public override void Update() 
    {
        // Handling sort order in SpriteSorter.cs
        // gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int)(-transform.localPosition.y+1);

        //if (furnitureToKick.Count > 0 && CanKickFurniture)
        //    furnitureToKick[furnitureToKick.Count - 1].GetComponent<KissableFurniture>().ShowOutline(MainColor);

        priority = Mathf.Clamp(priority, 0f, 1f);

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

        GetBButtonDown = false;

        if (InputMapper.GrabVal(XBOX360_BUTTONS.B, this.playerNum) && !wasBButtonPressed)
        {
            wasBButtonPressed = true;
            GetBButtonDown = true;
        }
        else if (!InputMapper.GrabVal(XBOX360_BUTTONS.B, this.playerNum) && wasBButtonPressed)
        {
            wasBButtonPressed = false;
        }

        if (heldItemSpriteRenderer != null)
        {
            heldItemSpriteRenderer.sortingOrder = mySpriteRenderer.sortingOrder + 1;
        }

        if (timeBetweenItemInteract == 0)
        {
            if (IsCarryingItem)
            {
                if (GetAButtonDown)
                {
                    PutItemDown(HeldItemName);
                }
                //else if (InputMapper.GrabVal(XBOX360_BUTTONS.B, this.playerNum))
                //{
                //    ThrowItem(HeldItemName);
                //}
#if UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE
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
        }

        if (timeBetweenItemInteract > 0)
            timeBetweenItemInteract -= Time.deltaTime;
        else if (timeBetweenItemInteract < 0)
            timeBetweenItemInteract = 0;

        if (timeBetweenFurnitureKick > 0)
            timeBetweenFurnitureKick -= Time.deltaTime;
        else if (timeBetweenFurnitureKick < 0)
            timeBetweenFurnitureKick = 0;

        if (timeSinceInvulnerable > 0)
        {
            timeSinceInvulnerable -= Time.deltaTime;

            if (timeSinceInvulnerable <= 0)
            {
                _HumanSpriteFlasher.StopFlashing();
            }
        }

        //if (timeSinceInteractButtonPrompt > 0)
        //{
        //    timeSinceInteractButtonPrompt -= Time.deltaTime;
            
        //    if (timeSinceInteractButtonPrompt <= 0)
        //    {
        //        interactButtonPromptSpriteRenderer.enabled = false;
        //    }
        //}

        //if (timeSinceKickButtonPrompt > 0)
        //{
        //    timeSinceKickButtonPrompt -= Time.deltaTime;

        //    if (timeSinceKickButtonPrompt <= 0)
        //    {
        //        kickButtonPromptSpriteRenderer.enabled = false;
        //    }
        //}

        if (furnitureToKick.Count > 0 && timeBetweenFurnitureKick <= 0)
        {
            if (GetBButtonDown)
            {
                MoveAnimation = MoveAnim.Kicking;
                KickFurniture(furnitureToKick[furnitureToKick.Count - 1].GetComponent<KissableFurniture>());
                furnitureToKick.Remove(furnitureToKick[furnitureToKick.Count - 1]);
                //kickButtonPromptSpriteRenderer.enabled = false;
            }

#if UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE
            else if (Input.GetKeyDown(ItemThrowKeycode))
            {
                MoveAnimation = MoveAnim.Kicking;
                KickFurniture(furnitureToKick[furnitureToKick.Count - 1].GetComponent<KissableFurniture>());
                //kickButtonPromptSpriteRenderer.enabled = false;
            }
#endif
        }

        foreach(Collider2D collider in furnitureToKick)
        {
            if (!collider.GetComponent<KissableFurniture>().CanKick())
                collider.GetComponent<KissableFurniture>().HideOutline();
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

        currentTopSpeed = TopSpeedWhileHoldingItem;
        currentAcelRate = AccelWhileHoldingItem;

        //Item Pick up Sound
        soundManager.SOUND_MAN.playSound("Play_Item_Pick_Up", gameObject);
    }

    public void KickFurniture(KissableFurniture _KissableFurniture)
    {
        Vector2 forceDirection;

        if(FacingRight)
            forceDirection = new Vector3(Mathf.Abs(interactTrigger.offset.x), interactTrigger.offset.y, 0);

        else
            forceDirection = new Vector3(interactTrigger.offset.x, interactTrigger.offset.y, 0);

        forceDirection += ((Vector2)_KissableFurniture.transform.position - (Vector2)transform.position);
        _KissableFurniture.Kick(forceDirection.normalized * kickForce);

        timeBetweenFurnitureKick = 0.5f;

        //Furniture Kick Sound
        //soundManager.SOUND_MAN.playSound("Play_Item_Pick_Up", gameObject);
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

        currentTopSpeed = NormalTopSpeed;
        currentAcelRate = AccelWhileHoldingItem;

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

#if UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE
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
        if (CanGrabItem)
        {
            if (col.tag == "Cat" && 
                col.GetComponent<MissionObjective_Item>() != null && 
                col.GetComponent<MissionObjective_Item>().IsItemPlacedDown)
            {
                //interactButtonPromptSpriteRenderer.enabled = true;
                //timeSinceInteractButtonPrompt = interactButtonPromptDurationBuffer;

                if (GetAButtonDown)
                {
                    GrabItem(col.gameObject);
                    //interactButtonPromptSpriteRenderer.enabled = false;
                    StartCoroutine(InputMapper.Vibration(playerNum, .2f, .15f, .5f));
                }
#if UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE
                else if (Input.GetKeyDown(ItemPickUpKeycode))
                {
                    GrabItem(col.gameObject);
                    //interactButtonPromptSpriteRenderer.enabled = false;

                }
#endif
            }
        }

        if (CanKickFurniture && col.tag == "Furniture" && col.GetComponent<KissableFurniture>() != null && col.GetComponent<KissableFurniture>().CanKick())
        {
            if (!furnitureToKick.Contains(col))
            {
                foreach (Collider2D collider in furnitureToKick)
                {
                    collider.GetComponent<KissableFurniture>().HideOutline();
                }
                    furnitureToKick.Add(col);
                    col.GetComponent<KissableFurniture>().ShowOutline(MainColor);
            }

            //kickButtonPromptSpriteRenderer.enabled = true;
            //timeSinceKickButtonPrompt = interactButtonPromptDurationBuffer;

        }

        //else if (col.tag == "Pull" && !IsPullingSwitch)
        //{
        //    if(InputMapper.GrabVal(XBOX360_BUTTONS.A, this.playerNum) || Input.GetKeyDown(ItemPickUpKeycode))
        //    {
        //        AttachToPullSwitch(col.gameObject);
        //    }
        //}
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Furniture")
        {
            furnitureToKick.Remove(col);
            col.GetComponent<KissableFurniture>().HideOutline();
        }
    }

    public void HugHuman()
    {
        if (timeSinceInvulnerable <= 0)
        {
            timeSinceInvulnerable = invulnerabilityDuration;
            _HumanSpriteFlasher.StartFlashing();

            gainHug();

            if(hugPoints > 0)
                StartCoroutine(InputMapper.Vibration(playerNum, 1, .55f, .7f));

            if (IsCarryingItem)
            {
                PutItemDown(HeldItemName);
            }
        }
    }

    private void gainHug()
    {
        --hugPoints;
        heartObjects[hugPoints].GetComponent<HeartComponent>().StartAnimation();
        //heartObjects[hugPoints].GetComponent<Image>().enabled = false;
        Camera.main.GetComponent<ScreenShake>().shake = 0.5f;

        //GET HURT, SCRUB (Play sound effect here please, tom. Okay, nice, have a good day. 8=====D~~~~~~

        if (hugPoints <= 0)
        {
            killSelf();
        }
    }

    private void killSelf()
    {
        GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if (gm.gameEnd == false)
        {
            gm.OnHumanDead(gameObject, gameObject.GetComponent<SpriteRenderer>());
            Camera.main.GetComponent<NewCameraBehavior>().targets.Remove(gameObject);

            if (IsCarryingItem)
                PutItemDown(HeldItemName);

            _HumanSpriteFlasher.StopFlashing();
            Destroy(GetComponent<Rigidbody>());
            GetComponent<Collider2D>().enabled = false;
            GetComponent<HumanDeath>().enabled = true;
            this.enabled = false;
        }
    }

    public void endKick()
    {

        MoveAnimation = MoveAnim.notKicking;

    }
    //public void AttachToPullSwitch(GameObject obj)
    //{
    //    IsPullingSwitch = true;
    //    Vector3 newPos = obj.transform.position;
    //    transform.position.Set(newPos.x, newPos.y, newPos.z);
    //}
}
