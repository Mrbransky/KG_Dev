using UnityEngine;
using System.Collections;

public class Human : Player 
{
    [SerializeField] private int hugPoints = 3;
    [SerializeField] private float invulnerabilityDuration = 1.5f;

    private GameObject[] heartObjects;
    private float timeSinceInvulnerable;
    private Color defaultSpriteColor;
    private Color invulnSpriteColor;

    public bool IsCarryingItem;
    string HeldItemName;

    public float timeBetweenItemInteract;

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
	}

    public override void Update() 
    {
        if (IsCarryingItem && InputMapper.GrabVal(XBOX360_BUTTONS.A, this.playerNum))
        {
            if (timeBetweenItemInteract == 0)
                PutItemDown(HeldItemName);
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

        base.Update();
	}

    void GrabItem(GameObject obj)
    {
        this.IsCarryingItem = true;
        
        obj.transform.parent = transform;
        obj.transform.localPosition = new Vector3(0, .75f, 0);
        obj.transform.localScale = new Vector3(1, 1, 1);
        obj.GetComponent<SpriteRenderer>().sortingOrder = this.gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
        HeldItemName = obj.name;

        timeBetweenItemInteract = 1;
    }
    
    void PutItemDown(string itemName)
    {
        this.IsCarryingItem = false;
        transform.FindChild(itemName).localPosition = new Vector3(interactTrigger.offset.x, interactTrigger.offset.y, 1);
        transform.FindChild(itemName).transform.parent = null;
        HeldItemName = "";

        timeBetweenItemInteract = 1;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (this.interactTrigger.IsTouching(col) && InputMapper.GrabVal(XBOX360_BUTTONS.A, this.playerNum))
        {
            if (!IsCarryingItem && col.tag == "Cat")
            {
                if(timeBetweenItemInteract == 0)
                    GrabItem(col.gameObject);
            }
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

            Destroy(gameObject);
        }
    }
}
