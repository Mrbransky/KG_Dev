using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Human : Player 
{
    [SerializeField] private int hugPoints = 3;
    [SerializeField] private float invulnerabilityDuration = 1.5f;

    private GameObject[] heartObjects;
    private float timeSinceInvulnerable = -1;
    private Color defaultSpriteColor;
    private Color invulnSpriteColor;

    public bool IsCarryingItem;
    string HeldItemName;

    public float timeBetweenItemInteract;
    private SpriteRenderer interactButtonPromptSpriteRenderer;

#if UNITY_EDITOR
    public KeyCode ItemPickUpKeycode = KeyCode.Z;
#endif

    public override void Awake() 
    {
        FacingRight = false;

        defaultSpriteColor = GetComponent<SpriteRenderer>().color;
        invulnSpriteColor = defaultSpriteColor;
        invulnSpriteColor.a /= 2;

        heartObjects = new GameObject[hugPoints];

        base.Awake();

        foreach (HeartComponent heartComponent in GetComponentsInChildren<HeartComponent>())
        {
            heartObjects[heartComponent.heartNum] = heartComponent.gameObject;
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
        if (IsCarryingItem && InputMapper.GrabVal(XBOX360_BUTTONS.A, this.playerNum))
        {
            if (timeBetweenItemInteract == 0)
                PutItemDown(HeldItemName);
        }
#if UNITY_EDITOR
        else if (IsCarryingItem && Input.GetKeyDown(ItemPickUpKeycode))
        {
            if (timeBetweenItemInteract == 0)
                PutItemDown(HeldItemName);
        }
#endif

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

        base.Update();
	}

    void GrabItem(GameObject obj)
    {
        this.IsCarryingItem = true;
        
        obj.transform.parent = transform;
        obj.transform.localPosition = new Vector3(0, .75f, 0);
        obj.transform.localScale = new Vector3(1, 1, 1);
        obj.GetComponent<SpriteRenderer>().sortingOrder = this.gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
        obj.GetComponent<MissionObjective_Item>().IsItemPlacedDown = false;
        HeldItemName = obj.name;

        timeBetweenItemInteract = 1;
    }
    
    void PutItemDown(string itemName)
    {
        this.IsCarryingItem = false;
        Transform childTransform = transform.FindChild(itemName);
        childTransform.localPosition = new Vector3(interactTrigger.offset.x, interactTrigger.offset.y, 1);
        childTransform.transform.parent = null;
        childTransform.GetComponent<MissionObjective_Item>().IsItemPlacedDown = true;
        HeldItemName = "";

        timeBetweenItemInteract = 1;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (this.interactTrigger.IsTouching(col) && !IsCarryingItem && col.tag == "Cat" && timeBetweenItemInteract == 0)
        {
            interactButtonPromptSpriteRenderer.enabled = true;

            if (InputMapper.GrabVal(XBOX360_BUTTONS.A, this.playerNum))
            {
                GrabItem(col.gameObject);
            }
#if UNITY_EDITOR
            else if (Input.GetKeyDown(ItemPickUpKeycode))
            {
                GrabItem(col.gameObject);
            }
#endif
        }
        else
        {
            interactButtonPromptSpriteRenderer.enabled = false;
        }
}

public void HugHuman()
    {
        if (timeSinceInvulnerable <= 0)
        {
            timeSinceInvulnerable = invulnerabilityDuration;
            loseHealth();

            GetComponent<SpriteRenderer>().color = invulnSpriteColor;
        }
    }

    private void loseHealth()
    {
        heartObjects[hugPoints - 1].SetActive(false);
        --hugPoints;

        checkHealth();
    }

    private void checkHealth()
    {
        if (hugPoints <= 0)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().OnHumanDead(gameObject);
            Camera.main.GetComponent<NewCameraBehavior>().targets.Remove(gameObject);
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().playerCount--;
            Destroy(gameObject);
        }
    }
}
