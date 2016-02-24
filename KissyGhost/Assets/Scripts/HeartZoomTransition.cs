using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeartZoomTransition : MonoBehaviour
{
    public int LevelIndexToLoad = -1;

    private Image _Image;
    private Animator _Animator;
    private bool isZoomOut = true;
    private bool isZoomIn = false;
    private bool isZooming = true;

    public bool IsZooming
    {
        get { return isZooming; }
    }

    void Start()
    {
        _Image = GetComponent<Image>();
        _Animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isZoomOut && _Animator.GetCurrentAnimatorStateInfo(0).IsName("ZoomOutDone"))
        {
            isZoomOut = false;
            isZooming = false;
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
                Application.LoadLevel(LevelIndexToLoad);
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

        _Image.enabled = true;
        _Animator.enabled = true;
        isZoomIn = true;
        isZooming = true;

        _Animator.SetBool("isZoomIn", true);
    }
}
