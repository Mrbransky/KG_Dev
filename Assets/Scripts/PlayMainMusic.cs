using UnityEngine;
using System.Collections;

public class PlayMainMusic : MonoBehaviour {

    public AudioSource audio;

	// Use this for initialization
	void Awake () 
    {
        audio.loop = true;
        audio.Play();
	}
	

}
