﻿using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class VideoOnDelay : MonoBehaviour
{
    private const int NUMBER_OF_GAMEPAD_BUTTONS = 20;

    public HeartZoomTransition _HeartZoomTransition;
    public bool IsMovieEnabled = true;
    public bool IsMovieLoopable = true;

    private Renderer myRenderer;
    private MovieTexture myMovieTexture;
    private bool noInputAfterTime = false;
    private float MenuTimer = 5;

    public bool IsMoviePlaying
    {
        get { return myMovieTexture.isPlaying; }
    }

    void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        myMovieTexture = (MovieTexture)myRenderer.material.mainTexture;
        myMovieTexture.loop = IsMovieLoopable;
    }

    void Update()
    {
        if (IsMovieEnabled && !_HeartZoomTransition.enabled)
        {
            TimerForVideo();
        }
    }

    void TimerForVideo()
    {
        if (!noInputAfterTime)
        {
            MenuTimer -= Time.deltaTime;
            myRenderer.enabled = false;

            if (MenuTimer <= 0)
            {
                noInputAfterTime = true;
                myRenderer.enabled = true;
                
                MenuTimer = 5;
                myMovieTexture.Play();
            }
        }
        
        if (Input.anyKeyDown || 
            (!IsMovieLoopable && noInputAfterTime && !myMovieTexture.isPlaying) || 
            anyGamepadButtonDown())
        {
            noInputAfterTime = false;
            MenuTimer = 5;
            myMovieTexture.Stop();
            myRenderer.enabled = false;
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
