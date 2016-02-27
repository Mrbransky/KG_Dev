using UnityEngine;
using System.Collections;

public class VideoOnDelay : MonoBehaviour {

    
    bool noInputAfterTime = false;
    float MenuTimer = 5;

	void Update () {

        TimerForVideo();

	}

    void TimerForVideo()
    {
        Renderer r = GetComponent<Renderer>();
        MovieTexture movie = (MovieTexture)r.material.mainTexture;

        if (!noInputAfterTime)
        {
            MenuTimer -= Time.deltaTime;
            r.enabled = false;

            if (MenuTimer <= 0)
            {
                noInputAfterTime = true;
                r.enabled = true;
                
                MenuTimer = 5;

            }
        }
        else
        {
            movie.Play();
        }


        if (Input.anyKey)
        {
            noInputAfterTime = false;
            MenuTimer = 5;
            movie.Stop();
            r.enabled = false;
        }
    }
}
