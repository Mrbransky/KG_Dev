using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnableLoading : MonoBehaviour {

    public Text loadingText;
    public float timeUntilEnable;
    private bool startTimer = false;

    void Start()
    {
        loadingText.enabled = false;
    }

    void Update()
    {
        if(startTimer)
        {
            if (timeUntilEnable > 0)
                timeUntilEnable -= Time.deltaTime;
            else
            {
                startTimer = false;
                loadingText.enabled = true;
            }
        }
    }


    public void EnableLoadingTextTimer()
    {
        startTimer = true;
    }
}
