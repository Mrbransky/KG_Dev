using UnityEngine;
using System.Collections;

public class PlayMainMusic : MonoBehaviour {

    public AudioSource audio;
    public AudioClip clip;

	// Use this for initialization
	void Awake () 
    {
        audio.clip = this.clip;
        audio.loop = true;
        audio.Play();
	}	

}
