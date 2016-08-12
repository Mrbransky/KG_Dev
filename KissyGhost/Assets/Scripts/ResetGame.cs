using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    static ResetGame reset;

    public KeyCode FirstKey, SecondKey, ThirdKey;

    float P1_Reset, P2_Reset, P3_Reset, P4_Reset;

    [Range(2,5)]
    public float ResetTime;

    void Awake()
    {
        if (reset == null)
            reset = this;

        else if (reset != this)
            Destroy(gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (Input.GetKey(FirstKey) && Input.GetKey(SecondKey) && Input.GetKey(ThirdKey))
                SceneManager.LoadScene(0);

            for (int i = 1; i < 5; i++)
                UpdatePlayerResetTimer(i);

            if (CheckForReset())
            {
                for (int i = 1; i < 5; i++)
                    ResetTimer(i);

                SceneManager.LoadScene(0);
            }
        }
    }

    //playerIndex 1 thru 4
    void UpdatePlayerResetTimer(int playerIndex)
    {
        if (Mathf.Clamp(playerIndex, 1, 4) != playerIndex)
            return;

        if (InputMapper.GrabVal(XBOX360_BUTTONS.SELECT, playerIndex))
            AddToTimer(playerIndex);
        else
            ResetTimer(playerIndex);
    }

    void AddToTimer(int playerIndex)
    {
        switch (playerIndex)
        {
            case 1:
                P1_Reset += Time.deltaTime;
                return;

            case 2:
                P2_Reset += Time.deltaTime;
                return;

            case 3:
                P3_Reset += Time.deltaTime;
                return;

            case 4:
                P4_Reset += Time.deltaTime;
                return;

            default:
                return;
        }
    }

    void ResetTimer(int playerIndex)
    {
        switch (playerIndex)
        {
            case 1:
                P1_Reset = 0;
                return;

            case 2:
                P2_Reset = 0;
                return;

            case 3:
                P3_Reset = 0;
                return;

            case 4:
                P4_Reset = 0;
                return;

            default:
                return;
        }
    }

    bool CheckForReset()
    {
        if (P1_Reset >= ResetTime
            || P2_Reset >= ResetTime
            || P3_Reset >= ResetTime
            || P4_Reset >= ResetTime)
            return true;

        return false;
    }
}
