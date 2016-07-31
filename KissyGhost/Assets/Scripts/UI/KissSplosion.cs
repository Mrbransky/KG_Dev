using UnityEngine;
using System.Collections;

public class KissSplosion : MonoBehaviour {

    float timer = 0.6f;
	
	// Update is called once per frame
	void Update () {

        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            gameObject.SetActive(false);
            timer = 0.6f;
        }
	
	}
}
