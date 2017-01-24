using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GoBack : MonoBehaviour {

    public HeartZoomTransition _HeartZoomTransition;

    public int sceneToGoBackToo;
	void Update ()
    {
	    if (_HeartZoomTransition.enabled)
        {
            return;
        }

        //TODO: controller support
        if (Input.GetKey("joystick button 1"))
        {
            GoBackToScene(sceneToGoBackToo);
        }
        //else if(Input.GetKey("joystick button 1"))
        //{
            //GoBackToInstructions();
        //}

#if !UNITY_EDITOR && !UNITY_WEBGL && !UNITY_WEBPLAYER
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _HeartZoomTransition.enabled = true;
            _HeartZoomTransition.StartHeartZoomIn(-1);
        }
#endif
    }

    public void GoBackToScene(int sceneToGoBackToo_)
    {
        _HeartZoomTransition.enabled = true;
        _HeartZoomTransition.StartHeartZoomIn(sceneToGoBackToo_);
    }
}
