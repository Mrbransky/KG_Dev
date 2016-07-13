using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class AnimaticPlayer : MonoBehaviour {

    private const int NUMBER_OF_GAMEPAD_BUTTONS = 20;
	
    public bool IsMovieEnabled = true;
    public bool IsMovieLoopable = true;

    private Renderer myRenderer;
    private MovieTexture myMovieTexture;
    private bool noInputAfterTime = false;
    private float MenuTimer = 20;

	private bool HasBeenSkipped;

    public bool IsMoviePlaying
    {
        get { return myMovieTexture.isPlaying; }
    }

    void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        myMovieTexture = (MovieTexture)myRenderer.material.mainTexture;
        myMovieTexture.loop = IsMovieLoopable;
		myMovieTexture.Play ();
    }

    void Update()
    {
		if (!HasBeenSkipped) 
		{
			if (anyGamepadButtonDown () == true || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.KeypadEnter)) 
			{
				HasBeenSkipped = true;
				myMovieTexture.Stop();
                SceneManager.LoadScene(1);
			}
			if (myMovieTexture.isPlaying == false) 
			{
				HasBeenSkipped = true;
                SceneManager.LoadScene(1);
			}
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
