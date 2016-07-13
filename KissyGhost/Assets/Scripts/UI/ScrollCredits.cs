using UnityEngine;
using System.Collections;

public class ScrollCredits : MonoBehaviour {

    public float scrollSpeed;
    public float HeartActivation;

    float startingYposition;
    float Yposition;

    bool ActivatedHearts;

    public ParticleSystem[] heartParticles;

    public float ResetPosition;

    void Awake()
    {
        startingYposition = GetComponent<RectTransform>().localPosition.y;
    }
	
	void Update () 
    {
        Yposition += scrollSpeed * Time.smoothDeltaTime;
        this.GetComponent<RectTransform>().localPosition = new Vector3(0, startingYposition + Yposition, 0);
        //Vector3 velocity = new Vector3(0, scrollSpeed, 0);
        //this.GetComponent<RectTransform>().localPosition = Vector3.SmoothDamp(GetComponent<RectTransform>().localPosition, new Vector3(0, Yposition, 0), ref velocity, 1);

        if (GetComponent<RectTransform>().localPosition.y >= ResetPosition)
            ResetCredits();

        if (GetComponent<RectTransform>().localPosition.y > HeartActivation && !ActivatedHearts)
            ActivateHeartParticles();
	}

    void ActivateHeartParticles()
    {
        foreach (ParticleSystem hearts in heartParticles)
        {
            ParticleSystem.EmissionModule em = hearts.emission;
            em.enabled = true;
        }
        ActivatedHearts = true;
    }

    void ResetCredits()
    {
        Yposition = 0;
        GetComponent<RectTransform>().localPosition = new Vector3(0, startingYposition, 0);

        foreach (ParticleSystem hearts in heartParticles)
        {
            ParticleSystem.EmissionModule em = hearts.emission;
            em.enabled = false;
        }

        ActivatedHearts = false;
    }
}
