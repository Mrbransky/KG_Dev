using UnityEngine;
using System.Collections;

public class GoToCSelect : MonoBehaviour
{
    public HeartZoomTransition _HeartZoomTransition;
    public int CharSelectSceneNum = 3;

    void Awake()
    {
        Time.timeScale = 1;
    }
    public void GoBack() 
    {
        _HeartZoomTransition.enabled = true;
        _HeartZoomTransition.StartHeartZoomIn(1);
        soundManager.SOUND_MAN.playSound("Play_MenuDown", gameObject);

    }
	void Update ()
    {
        if (_HeartZoomTransition.enabled)
        {
            return;
        }
        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Backspace))
        {
            _HeartZoomTransition.enabled = true;
            _HeartZoomTransition.StartHeartZoomIn(1);
            soundManager.SOUND_MAN.playSound("Play_MenuDown", gameObject);
        }
        if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.Space))
        {
            _HeartZoomTransition.enabled = true;
            _HeartZoomTransition.StartHeartZoomIn(CharSelectSceneNum);
            soundManager.SOUND_MAN.playSound("Play_MenuConfirm", gameObject);
        }
#if UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            _HeartZoomTransition.enabled = true;
            _HeartZoomTransition.StartHeartZoomIn(CharSelectSceneNum);
            soundManager.SOUND_MAN.playSound("Play_MenuConfirm", gameObject);
        }
#endif

#if !UNITY_EDITOR && !UNITY_WEBGL && !UNITY_WEBPLAYER
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _HeartZoomTransition.enabled = true;
            _HeartZoomTransition.StartHeartZoomIn(-1);
        }
#endif
	}
}
