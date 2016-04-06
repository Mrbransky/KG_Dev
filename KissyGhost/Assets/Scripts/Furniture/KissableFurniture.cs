using UnityEngine;
using System.Collections;

public enum KissedFurnitureBehavior
{
    None = 0,
    FollowPlayer = 1,
    Shoot = 2,
    RhinoCharge = 3
}

public class KissableFurniture : MonoBehaviour
{
    [SerializeField] private KissedFurnitureBehavior kissedBehavior = KissedFurnitureBehavior.None;
    public Sprite UnkissedSprite;
    public Sprite KissedSprite;
    public Color kissedColor = new Color(255.0f / 255.0f, 192.0f / 255.0f, 203.0f / 255.0f);
    private SpriteRenderer spriteRenderer;
    private bool isKissed = false;
    public int amountKissed = 0;
   
    [SerializeField] private float kissedDuration = 3.0f;
    private float timeSinceKiss;
    private GameManager _GameManager;

    private Transform Heart_Fountain;
    private Transform Smaller_Heart_Fountain;

    private Furniture_FollowPlayer followPlayerBehavior;
    private Furniture_RhinoCharge rhinoChargeBehavior;
    private Furniture_Shoot shootBehavior;

    public float kickCooldown = 1.0f;
    private Rigidbody2D myRigidbody;
    private float timeSinceKick = 0.0f;

#if UNITY_EDITOR
    public KeyCode KissKey = KeyCode.Alpha0;
#endif

    void Start()
    {
        _GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        myRigidbody = GetComponent<Rigidbody2D>();


        switch ((int)kissedBehavior)
        {
            case (int)KissedFurnitureBehavior.FollowPlayer:
                followPlayerBehavior = GetComponent<Furniture_FollowPlayer>();
                amountKissed = 2;
                if (followPlayerBehavior == null)
                {
                    kissedBehavior = KissedFurnitureBehavior.None;
                }
                break;
            case (int)KissedFurnitureBehavior.Shoot:
                shootBehavior = GetComponent<Furniture_Shoot>();
                amountKissed = 2;
                if (shootBehavior == null)
                {
                    kissedBehavior = KissedFurnitureBehavior.None;
                }
                break;
            case (int)KissedFurnitureBehavior.RhinoCharge:
                rhinoChargeBehavior = GetComponent<Furniture_RhinoCharge>();
                
                if (rhinoChargeBehavior == null)
                {
                    kissedBehavior = KissedFurnitureBehavior.None;
                }
                break;
        }

        Heart_Fountain = transform.FindChild("Heart_Fountain");
        Heart_Fountain.gameObject.SetActive(false);

        if((int)kissedBehavior == (int)KissedFurnitureBehavior.RhinoCharge)
        {
            Smaller_Heart_Fountain = transform.FindChild("Smaller_Heart_Fountain");
            Smaller_Heart_Fountain.gameObject.SetActive(false);
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

        if (timeSinceKick > 0)
        {
            timeSinceKick -= Time.deltaTime;
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
            myRigidbody.velocity = Vector2.zero;

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

        if (amountKissed >= 2)
        {
            if (KissedSprite != null)
            {
                spriteRenderer.sprite = KissedSprite;
            }
            else
            {
                spriteRenderer.color = kissedColor;
            }

            //Start Playing Furniture sliding sound
            soundManager.SOUND_MAN.playSound("Play_FurnitureMove", gameObject);
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
            case (int)KissedFurnitureBehavior.RhinoCharge:
                if (amountKissed >= 2)
                {
                    rhinoChargeBehavior.enabled = true;
                    rhinoChargeBehavior.Initialize(getClosestPlayerTransform());
                    
                }
                else { amountKissed++; }
                break;
        }

        switch(amountKissed)
        {
            case 1:
            if (Smaller_Heart_Fountain != null)
            {
                Smaller_Heart_Fountain.gameObject.SetActive(true);
            }
            break;
            case 2:
            if (Smaller_Heart_Fountain != null)
            { 
                Smaller_Heart_Fountain.gameObject.SetActive(false);
            }

            if (Heart_Fountain != null)
            {
                Heart_Fountain.gameObject.SetActive(true);
            }
            break;
            
        }
    }

    public void UnkissFurniture()
    {
        if (isKissed)
        {
            isKissed = false;
            OnFurnitureUnkissed();
        }
		AkSoundEngine.PostEvent ("Stop_FurnitureMove", gameObject);

        //soundManager.SOUND_MAN.stopSound("Play_FurnitureMove", gameObject, 1);
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

        if(KissedSprite == null && spriteRenderer.color != Color.white)
            spriteRenderer.color = Color.white;

        switch ((int)kissedBehavior)
        {
            case (int)KissedFurnitureBehavior.FollowPlayer:
                followPlayerBehavior.enabled = false;
                break;
            case (int)KissedFurnitureBehavior.Shoot:
                shootBehavior.enabled = false;
                break;
            case (int)KissedFurnitureBehavior.RhinoCharge:
                rhinoChargeBehavior.enabled = false;
                break;
        }

      Heart_Fountain.gameObject.SetActive(false);
      if ((int)kissedBehavior == (int)KissedFurnitureBehavior.RhinoCharge)
      {
          Smaller_Heart_Fountain = transform.FindChild("Smaller_Heart_Fountain");
          Smaller_Heart_Fountain.gameObject.SetActive(false);
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

    void OnCollisionEnter2D(Collision2D col)
    {
        if(isKissed && col.gameObject.tag == "Ghost Barrier")
        {
            UnkissFurniture();
            GetComponent<Rigidbody2D>().AddForce(followPlayerBehavior.GetFurnitureRigidbodyVelocity() * -1 * 10000);
            //Getco .AddForce(followPlayerBehavior.GetFurnitureRigidbodyVelocity() * -1 * 500);
            
            //Make furniture bounce away from door
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

    public bool CanKick()
    {
        if (!isKissed && timeSinceKick <= 0)
        {
            return true;
        }

        return false;
    }

    public void Kick(Vector2 kickVector)
    {
        myRigidbody.AddForce(kickVector);
        timeSinceKick = kickCooldown;
    }
}
