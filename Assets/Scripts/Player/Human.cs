﻿using UnityEngine;
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
    string HeldItemName;

    public float timeBetweenItemInteract;
    private SpriteRenderer interactButtonPromptSpriteRenderer;

#if UNITY_EDITOR
    public KeyCode ItemPickUpKeycode = KeyCode.Z;
    public KeyCode ItemThrowKeycode = KeyCode.X;
#endif

    public override void Awake() 
    {
        FacingRight = false;

        defaultSpriteColor = GetComponent<SpriteRenderer>().color;
        invulnSpriteColor = defaultSpriteColor;
        invulnSpriteColor.a /= 2;

        heartObjects = new GameObject[hugLimit];

        base.Awake();

        foreach (HeartComponent heartComponent in GetComponentsInChildren<HeartComponent>())
        {
            heartObjects[heartComponent.heartNum] = heartComponent.gameObject;
            heartComponent.gameObject.SetActive(false);
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

        reduceVelocity();

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
        if (col.tag == "Cat")
        {
            if (timeBetweenItemInteract == 0 && !IsCarryingItem && col.tag == "Cat" && this.interactTrigger.IsTouching(col))
            {
                interactButtonPromptSpriteRenderer.enabled = true;

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
                    Debug.Log("PEW");
                }
#endif
            }
            else
            {
                interactButtonPromptSpriteRenderer.enabled = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Cat" && interactButtonPromptSpriteRenderer.enabled)
        {
            interactButtonPromptSpriteRenderer.enabled = false;
        }
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
        heartObjects[hugPoints].SetActive(true);
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

    private void reduceVelocity()
    {
        Vector2 Vel = this.GetComponent<Rigidbody2D>().velocity;

        if (Vel.x > 0)
            rigidBody.velocity.Set(Vel.x -= .2f, Vel.y);
        else if (Vel.x < 0)
            rigidBody.velocity.Set(Vel.x += .2f, Vel.y);
    }
}
