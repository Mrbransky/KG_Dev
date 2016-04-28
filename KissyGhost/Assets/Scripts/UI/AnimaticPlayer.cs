using UnityEngine;
using System.Collections;

public class AnimaticPlayer : MonoBehaviour {

    private const int NUMBER_OF_GAMEPAD_BUTTONS = 20;

    //public HeartZoomTransition _HeartZoomTransition;
    public bool IsMovieEnabled = true;
    public bool IsMovieLoopable = true;

    private Renderer myRenderer;
    private MovieTexture myMovieTexture;
    private bool noInputAfterTime = false;
    private float MenuTimer = 20;

    public bool IsMoviePlaying
    {
        get { return myMovieTexture.isPlaying; }
    }

    void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        myMovieTexture = (MovieTexture)myRenderer.material.mainTexture;
        myMovieTexture.loop = IsMovieLoopable;
        myMovieTexture.Play();
    }

    void Update()
    {

        if(anyGamepadButtonDown() == true || Input.anyKey)
        {
            Application.LoadLevel(1);
        }
        if (myMovieTexture.isPlaying == false)
        {
            Application.LoadLevel(1);
        }
    }

    private bool anyGamepadButtonDown()
    {
        for (int i = 0; i < NUMBER_OF_GAMEPAD_BUTTONS; ++i)
        {
            if (Input.GetKeyDown("joystick button " + i))
            {
                return true;
            }
        }

        return false;
    }
}
