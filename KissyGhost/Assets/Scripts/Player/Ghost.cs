using UnityEngine;
using System.Collections.Generic;

public class Ghost : Player
{
    public float timeBetweenKisses = 1.5f;
    public float SpeedReducePercent = 75;
    private float timeSinceKiss;
    private MoveInteractTrigger _MoveInteractTrigger;
    public AudioClip[] smoochSounds;

    public bool TouchingFurniture;   

    private AudioSource source;

    public override void Awake() 
    {
        FacingRight = false;
        base.Awake();

        _MoveInteractTrigger = this.interactTrigger.GetComponent<MoveInteractTrigger>();
        source = this.GetComponent<AudioSource>();
	}

    public override void Update()
    {
        if (timeSinceKiss > 0)
        {
            timeSinceKiss -= Time.deltaTime;
        }
        else if (InputMapper.GrabVal(XBOX360_BUTTONS.A, this.playerNum) && canKissObject())
        {
            kissObject();            
        }
#region Keyboard Input Related Code (for Debugging)
#if UNITY_EDITOR
        else if (Input.GetKeyDown(KeyCode.M) && canKissObject())
        {
            kissObject();
        }
#endif
#endregion

        base.Update();

        if (TouchingFurniture && currentSpeed > 1.5f)
            currentSpeed = SlowGhostDown(SpeedReducePercent);
#if UNITY_EDITOR
        else if (TouchingFurniture && debugCurrentSpeed > 1.5f)
            debugCurrentSpeed = DebugSlowGhostDown(SpeedReducePercent);
#endif
    }
    
    private bool canKissObject()
    {
        return (_MoveInteractTrigger.colliderList.Count > 0 && timeSinceKiss <= 0);
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
                timeSinceKiss = timeBetweenKisses;
                source.PlayOneShot(PickRandomKissSound());
                StartCoroutine(InputMapper.Vibration(playerNum, .2f, .15f, .5f));
            }
        }

        soundManager.SOUND_MAN.playSound("Play_Kisses", gameObject);
    }

    //float Arguement gets used as a percentage
    private float SlowGhostDown(float SpeedReduction)
    {
        if (SpeedReduction > 100) SpeedReduction = 100;
        else if (SpeedReduction < 0) SpeedReduction = 0;

        SpeedReduction = SpeedReduction/100f;

        return currentSpeed * SpeedReduction;
    }

    private float DebugSlowGhostDown(float SpeedReduction)
    {
        if (SpeedReduction > 100) SpeedReduction = 100;
        else if (SpeedReduction < 0) SpeedReduction = 0;

        SpeedReduction = SpeedReduction / 100f;

        return debugCurrentSpeed * SpeedReduction;
    }
}
