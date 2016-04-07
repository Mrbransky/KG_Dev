using UnityEngine;
using System.Collections.Generic;

public class Ghost : Player
{
    public float timeBetweenKisses = 0.2f;
    public float SpeedReducePercent = 75;
    private float timeSinceKiss;
    public AudioClip[] smoochSounds;

    public bool GetAButtonDown = false;
    private bool wasAButtonPressed = false;

    public bool TouchingFurniture;

    public int maxKisses = 3;
    public float timeToRechargeKiss = 3.0f;
    private HeartComponent[] heartComponentsArray;
    private int availableKisses = 3;
    private float kissRechargeTimer = 0.0f;

    private AudioSource source;

    public override void Awake() 
    {
        FacingRight = false;
        base.Awake();

        source = this.GetComponent<AudioSource>();
        heartComponentsArray = GetComponentsInChildren<HeartComponent>();
        availableKisses = maxKisses;
    }

    public override void Update()
    {
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

        if (timeSinceKiss > 0)
        {
            timeSinceKiss -= Time.deltaTime;
        }
        else if (GetAButtonDown && canKissObject())
        {
            kissObject();            
        }
        #region Keyboard Input Related Code (for Debugging)
#if UNITY_EDITOR || UNITY_WEBGL //|| UNITY_STANDALONE
        else if (Input.GetKeyDown(KeyCode.M) && canKissObject())
        {
            kissObject();
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

        base.Update();

        if (TouchingFurniture && currentSpeed > 1.5f)
            currentSpeed = SlowGhostDown(SpeedReducePercent);
#if UNITY_EDITOR || UNITY_WEBGL //|| UNITY_STANDALONE
        else if (TouchingFurniture && debugCurrentSpeed > 1.5f)
            debugCurrentSpeed = DebugSlowGhostDown(SpeedReducePercent);
#endif
    }
    
    private bool canKissObject()
    {
        return (_MoveInteractTrigger.colliderList.Count > 0 && timeSinceKiss <= 0 && availableKisses > 0);
    }

    private AudioClip PickRandomKissSound()
    {
        
        return smoochSounds[Random.Range(0, smoochSounds.Length - 1)];
    }

    private void kissObject()
    {
        // Don't put kiss on cooldown if the furniture is already kissed
        foreach (Collider2D col in _MoveInteractTrigger.colliderList)
        {
            if (col.GetComponent<KissableFurniture>().KissFurniture())
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
                source.PlayOneShot(PickRandomKissSound());
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

#if UNITY_EDITOR || UNITY_WEBGL //|| UNITY_STANDALONE
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
}
