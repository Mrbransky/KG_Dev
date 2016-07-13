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
        Scene scene = SceneManager.GetActiveScene();
        if (Input.GetKey("joystick button 1") && scene.buildIndex == 1)
        {
            GoBackToAnimatic(0);
        }
        else if(Input.GetKey("joystick button 1"))
        {
            GoBackToInstructions();
        }

#if !UNITY_EDITOR && !UNITY_WEBGL && !UNITY_WEBPLAYER
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _HeartZoomTransition.enabled = true;
            _HeartZoomTransition.StartHeartZoomIn(-1);
        }
#endif
    }
    public void GoBackToInstructions()
    {
        _HeartZoomTransition.enabled = true;
        _HeartZoomTransition.StartHeartZoomIn(sceneToGoBackToo);
    }
    public void GoBackToAnimatic(int sceneToTransfer)
    {
        _HeartZoomTransition.enabled = true;
        _HeartZoomTransition.StartHeartZoomIn(sceneToTransfer);
    }
}
