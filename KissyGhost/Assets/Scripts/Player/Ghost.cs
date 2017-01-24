using UnityEngine;
using System.Collections.Generic;

public class Ghost : Player
{   
    [Header("Timers")]
    public float timeToRechargeKiss = 3.0f;
    public float timeBetweenKisses = 0.2f;
    private float kissRechargeTimer = 0.0f;
    private float stunTimer = 2.0f;
    private float timeSinceKiss;

    [Header("Ghost Misc.")]
    public bool GetAButtonDown = false;
    public bool TouchingFurniture;
    public Color MainColor;
    private bool wasAButtonPressed = false;
    private bool hasGameEnded = false;
    private bool isStunned = false;
    public GameObject kissParts;
    public GameObject DecoyExplosion;
  
    private HeartComponent[] heartComponentsArray;

    private int availableKisses = 3;
    public int maxKisses = 3;

    public float SpeedReducePercent = 50;

    private List<Collider2D> bodyColliderList;
    public bool IsHighlightingFurnitureTouchingBody
    {
        get 
        {
            foreach(Collider2D col in bodyColliderList)
            {
                if (col != null && col.GetComponent<KissableFurniture>().IsShowingOutline)
                    return true;
            }

            return false;
        }
    }

    public override void Awake() 
    {
        FacingRight = false;
        base.Awake();

        bodyColliderList = new List<Collider2D>();

        heartComponentsArray = GetComponentsInChildren<HeartComponent>();
        availableKisses = maxKisses;
    }

    public override void Update()
    {
        GetAButtonDown = false;
        if(isStunned == true)
        {
            stunTimer -= Time.deltaTime;
            if(stunTimer > 0)
            { return; }
            else
            { isStunned = false;  stunTimer = 2.0f; TouchingFurniture = false; }
        }
        if (InputMapper.GrabVal(XBOX360_BUTTONS.A, this.playerNum) && !wasAButtonPressed)
        {
            wasAButtonPressed = true;
            GetAButtonDown = true;
        }
        else if (!InputMapper.GrabVal(XBOX360_BUTTONS.A, this.playerNum) && wasAButtonPressed)
        {
            wasAButtonPressed = false;
        }

        if (timeSinceKiss > 0)
        {
            timeSinceKiss -= Time.deltaTime;
        }
        else if (GetAButtonDown && canKissObject())
        {
            if (_MoveInteractTrigger.interactColliderList.Count > 0)
            { kissObject(_MoveInteractTrigger.interactColliderList); }
            else
            {kissObject(bodyColliderList);}
            MoveAnimation = MoveAnim.Kiss;
            kissParts.SetActive(true);
        }
        #region Keyboard Input Related Code (for Debugging)
#if UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE
        else if (Input.GetKeyDown(KeyCode.M) && canKissObject())
        {
            if (_MoveInteractTrigger.interactColliderList.Count > 0)
            { kissObject(_MoveInteractTrigger.interactColliderList); }
            else
            { kissObject(bodyColliderList); }
            MoveAnimation = MoveAnim.Kiss;
            kissParts.SetActive(true);
        }
#endif
        #endregion
        
        if (availableKisses < maxKisses)
        {
            kissRechargeTimer += Time.deltaTime;
            heartComponentsArray[availableKisses].UpdateGrow(kissRechargeTimer / timeToRechargeKiss);

            if (kissRechargeTimer >= timeToRechargeKiss)
            {
                kissRechargeTimer = 0.0f;
                heartComponentsArray[availableKisses].ReEnable();
                ++availableKisses;
            }
        }
		if(Time.timeScale <= 0.01f && GameObject.Find("GameManager").GetComponent<GameManager>().didHumansWin == true)
		{
			GameObject.Find("Heart_Splosion").GetComponent<ParticleSystem>().Simulate(Time.unscaledDeltaTime, true, false);
        }
        base.Update();

        if (TouchingFurniture && currentSpeed > 1.5f)
            currentSpeed = SlowGhostDown(SpeedReducePercent);
#if UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE
        else if (TouchingFurniture && debugCurrentSpeed > 1.5f)
            debugCurrentSpeed = DebugSlowGhostDown(SpeedReducePercent);
#endif
    }
    
    private bool canKissObject()
    {
        if (_MoveInteractTrigger.interactColliderList.Count > 0 || bodyColliderList.Count > 0)
            return (timeSinceKiss <= 0 && availableKisses > 0 && !hasGameEnded);

        return false;
          
        //return (_MoveInteractTrigger.interactColliderList.Count > 0 && timeSinceKiss <= 0 && availableKisses > 0 && !hasGameEnded);
    }

    private void kissObject(List<Collider2D> furniture)
    {
        // Don't put kiss on cooldown if the furniture is already kissed
        //foreach (Collider2D col in furniture)
        //{
        //    if (col.GetComponent<KissableFurniture>().KissFurniture())
        //    {
        //        if (availableKisses < maxKisses)
        //        {
        //            heartComponentsArray[availableKisses].Hide();
        //        }

        //        --availableKisses;

        //        if (availableKisses >= 0)
        //        {
        //            heartComponentsArray[availableKisses].Hide();
        //        }

        //        timeSinceKiss = timeBetweenKisses;
        //        StartCoroutine(InputMapper.Vibration(playerNum, .2f, .15f, .5f));

        //        soundManager.SOUND_MAN.playSound("Play_Kisses", gameObject);

        //        return;
        //    }


        //}

        for (int i = furniture.Count - 1; i >= 0; i--)
        {
            if (furniture[i].GetComponent<KissableFurniture>().KissFurniture())
            {
                if (availableKisses < maxKisses)
                {
                    heartComponentsArray[availableKisses].Hide();
                }

                --availableKisses;

                if (availableKisses >= 0)
                {
                    heartComponentsArray[availableKisses].Hide();
                }

                timeSinceKiss = timeBetweenKisses;
                StartCoroutine(InputMapper.Vibration(playerNum, .2f, .15f, .5f));

                soundManager.SOUND_MAN.playSound("Play_Kisses", gameObject);

                return;
            }
        }
    }

    //float Arguement gets used as a percentage
    private float SlowGhostDown(float SpeedReduction)
    {
        if (SpeedReduction > 100) SpeedReduction = 100;
        else if (SpeedReduction < 0) SpeedReduction = 0;

        SpeedReduction = SpeedReduction/100f;

        return currentSpeed * SpeedReduction;
    }

#if UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE
    private float DebugSlowGhostDown(float SpeedReduction)
    {
        if (SpeedReduction > 100) SpeedReduction = 100;
        else if (SpeedReduction < 0) SpeedReduction = 0;

        SpeedReduction = SpeedReduction / 100f;

        return debugCurrentSpeed * SpeedReduction;
    }
#endif

    public void SetTimeSinceKiss(float time)
    {
        this.timeSinceKiss = time;
    }

    public void UpdateGameHasEnded(GameManager gm)
    {
        hasGameEnded = gm.gameEnd;
    }

    public void ShowBodyFurnitureOutline()
    {
        if(bodyColliderList.Count > 0)
        {
            if(bodyColliderList[bodyColliderList.Count -1] != null)
            bodyColliderList[bodyColliderList.Count - 1].GetComponent<KissableFurniture>().ShowOutline(MainColor);
        }
    }

    public void HideBodyFurnitureOutline()
    {
        foreach(Collider2D col in bodyColliderList)
        {
            if (col != null && col.GetComponent<KissableFurniture>().IsShowingOutline)
                col.GetComponent<KissableFurniture>().HideOutline();
        }
    }

    public void AddColliderToList(Collider2D col)
    {
        if (!bodyColliderList.Contains(col))
        {
            bodyColliderList.Add(col);

            if (GetComponentInChildren<MoveInteractTrigger>().interactColliderList.Count == 0)
                col.GetComponent<KissableFurniture>().ShowOutline(MainColor);
        }
    }

    public void RemoveColliderFromList(Collider2D col)
    {
        if (bodyColliderList.Contains(col))
        {
            bodyColliderList.Remove(col);
            col.GetComponent<KissableFurniture>().HideOutline();
        }
    }

    public void stopKissing()
    {
        MoveAnimation = MoveAnim.stopKiss;
    }
    public void kissedDecoy()
    {
        Instantiate(DecoyExplosion, transform.position, Quaternion.identity);
        transform.GetChild(0).GetComponent<MoveInteractTrigger>().RemoveDecoyObject();
        isStunned = true;
    }
}
