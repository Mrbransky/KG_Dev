using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class HeartZoomTransition : MonoBehaviour
{
    public int LevelIndexToLoad = -1;

    private Image _Image;
    private Animator _Animator;
    private EnableLoading enableLoad;
    private bool isZoomOut = true;
    private bool isZoomIn = false;
    private bool isZoomInFinish = true;

    private int MultiplayerScene = 3;
    private int SinglePlayerScene = 8;

    void Awake()
    {
        _Image = GetComponent<Image>();
        _Animator = GetComponent<Animator>();
        enableLoad = GetComponent<EnableLoading>();

        if (!_Image.enabled)
        {
            _Image.enabled = true;
        }
    }

    void Update()
    {
        if (isZoomOut)
        {
            float timeSinceLeveLoad = _Animator.GetFloat("timeSinceLevelLoad") + Time.unscaledDeltaTime;
            _Animator.SetFloat("timeSinceLevelLoad", timeSinceLeveLoad);
        }

        if (isZoomOut && _Animator.GetCurrentAnimatorStateInfo(0).IsName("ZoomOutDone"))
        {
            isZoomOut = false;
            _Animator.enabled = false;
            _Image.enabled = false;
            this.enabled = false;
        }
        else if (isZoomIn && _Animator.GetCurrentAnimatorStateInfo(0).IsName("ZoomInDone"))
        {
            isZoomIn = false;

            if (LevelIndexToLoad < 0)
            {
                Application.Quit();
            }
            else
            {
               SceneManager.LoadScene(LevelIndexToLoad);
            }
        }
    }

    public void StartHeartZoomIn(int levelIndex)
    {
        if (isZoomIn)
        {
            return;
        }

        LevelIndexToLoad = levelIndex;

        //if (LevelIndexToLoad == MultiplayerScene || LevelIndexToLoad == SinglePlayerScene)
        //{
        //    if (enableLoad != null)
        //    {
        //        enableLoad.EnableLoadingTextTimer();
        //    }
        //}

        _Image.enabled = true;
        _Animator.enabled = true;
        isZoomIn = true;

        _Animator.SetBool("isZoomIn", true);
    }

    public void StartHeartZoomInHalfway()
    {
        _Image.enabled = true;
        _Animator.enabled = true;
        _Animator.SetBool("isZoomInHalfway", true);
    }

    public bool IsZoomInHalfwayDone()
    {
        return _Animator.GetCurrentAnimatorStateInfo(0).IsName("ZoomInHalfwayDone");
    }

    public void StartHeartZoomInFinish(int levelIndex)
    {
        LevelIndexToLoad = levelIndex;

        isZoomIn = true;

        _Animator.SetBool("isZoomInFinish", true);
    }
}
