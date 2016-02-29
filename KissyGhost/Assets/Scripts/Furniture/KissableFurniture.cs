using UnityEngine;
using System.Collections;

public enum KissedFurnitureBehavior
{
    None = 0,
    FollowPlayer = 1,
    Shoot = 2
}

public class KissableFurniture : MonoBehaviour
{
    [SerializeField] private KissedFurnitureBehavior kissedBehavior = KissedFurnitureBehavior.None;
    public Sprite UnkissedSprite;
    public Sprite KissedSprite;
    public Color kissedColor = new Color(255.0f / 255.0f, 192.0f / 255.0f, 203.0f / 255.0f);
    private SpriteRenderer spriteRenderer;
    private bool isKissed = false;
   
    [SerializeField] private float kissedDuration = 3.0f;
    private float timeSinceKiss;
    private GameManager _GameManager;

    private Furniture_FollowPlayer followPlayerBehavior;
    private Furniture_Shoot shootBehavior;

#if UNITY_EDITOR
    public KeyCode KissKey = KeyCode.Alpha0;
#endif

    void Start()
    {
        _GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        switch ((int)kissedBehavior)
        {
            case (int)KissedFurnitureBehavior.FollowPlayer:
                followPlayerBehavior = GetComponent<Furniture_FollowPlayer>();

                if (followPlayerBehavior == null)
                {
                    kissedBehavior = KissedFurnitureBehavior.None;
                }
                break;
            case (int)KissedFurnitureBehavior.Shoot:
                shootBehavior = GetComponent<Furniture_Shoot>();

                if (shootBehavior == null)
                {
                    kissedBehavior = KissedFurnitureBehavior.None;
                }
                break;
        }
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KissKey))
        {
            isKissed = !isKissed;

            if (isKissed)
            {
                KissFurniture();
            }
            else
            {
                UnkissFurniture();
            }
        }
#endif

        if (isKissed)
        {
            timeSinceKiss -= Time.deltaTime;

            if (timeSinceKiss <= 0)
            {
                UnkissFurniture();
            }
        }
    }

    // Returns false if the furniture is already kissed
    public bool KissFurniture()
    {
        if (isKissed)
        {
            return false;
        }
        else
        {
            isKissed = true;
            timeSinceKiss = kissedDuration;

            OnFurnitureKissed();

            return true;
        }
    }

    private void OnFurnitureKissed()
    {
        if (_GameManager.currentPlayers.Count < 2)
        {
            UnkissFurniture();
            return;
        }

        if (KissedSprite != null)
        {
            spriteRenderer.sprite = KissedSprite;
        }
        else
        {
            spriteRenderer.color = kissedColor;
        }

        switch ((int)kissedBehavior)
        {
            case (int)KissedFurnitureBehavior.FollowPlayer:
                followPlayerBehavior.enabled = true;
                followPlayerBehavior.Initialize(getClosestPlayerTransform());
                break;
            case (int)KissedFurnitureBehavior.Shoot:
                shootBehavior.enabled = true;
                break;
        }

        //Start Playing Furniture sliding sound
        soundManager.SOUND_MAN.playSound("Play_FurnitureMove", gameObject);
    }

    public void UnkissFurniture()
    {
        if (isKissed)
        {
            isKissed = false;
            OnFurnitureUnkissed();
        }

        soundManager.SOUND_MAN.stopSound("Play_FurnitureMove", gameObject, 1);
    }

    private void OnFurnitureUnkissed()
    {
        if (UnkissedSprite != null)
        {
            spriteRenderer.sprite = UnkissedSprite;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }

        switch ((int)kissedBehavior)
        {
            case (int)KissedFurnitureBehavior.FollowPlayer:
                followPlayerBehavior.enabled = false;
                break;
            case (int)KissedFurnitureBehavior.Shoot:
                shootBehavior.enabled = false;
                break;
        }

        //Stop Furniture sliding sound
        
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (isKissed)
        {
            Human humanScript = col.gameObject.GetComponent<Human>();
            
            if (humanScript != null)
            {
                humanScript.HugHuman();                
                UnkissFurniture();
            }
        }
    }

    private Transform getClosestPlayerTransform()
    {
        GameObject closestPlayer = null;
        float closestPlayerDist = float.MaxValue;

        for (int i = 0; i < _GameManager.currentPlayers.Count - 1; ++i)
        {
            float playerDist = Vector3.Distance(_GameManager.currentPlayers[i].transform.position, transform.position);

            if (playerDist < closestPlayerDist)
            {
                closestPlayer = _GameManager.currentPlayers[i];
                closestPlayerDist = playerDist;
            }
        }

        return closestPlayer.transform;
    }
}
