using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeartComponent : MonoBehaviour
{
    public int heartNum = 0;
    public GameObject HeartOutline;

    private Animator _Animator;
    private bool isAnimating = false;

    private Image _Image;
    private Color defaultColor = Color.white;
    private Color transparentColor = Color.white;
    private float defaultScale = 1.0f;

    private float shrinkSpeed = 1.2f;
    private bool isShrinking = false;

    public bool IsShrinking
    {
        get { return isShrinking; }
    }

    void Start()
    {
        defaultScale = transform.localScale.x;

        _Animator = GetComponent<Animator>();
        _Image = GetComponent<Image>();
        defaultColor = _Image.color;
        transparentColor = defaultColor;
        transparentColor.a /= 2;
    }

    public void Disable()
    {
        if (HeartOutline != null)
        {
            HeartOutline.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    void Update()
    {
        if (isAnimating && _Animator.GetCurrentAnimatorStateInfo(0).IsName("Heartsplosion_End"))
        {
            _Image.enabled = false;
            _Animator.enabled = false;
            isAnimating = false;
            this.enabled = false;
        }

        if (isShrinking)
        {
            shrink();
        }
    }

    public void StartAnimation()
    {
        _Animator.enabled = true;
        isAnimating = true;
    }

    public void StartShrink()
    {
        isShrinking = true;
        ReEnable();
    }

    private void shrink()
    {
        transform.localScale -= new Vector3(shrinkSpeed, shrinkSpeed, shrinkSpeed) * Time.deltaTime;

        if (transform.localScale.x <= 0)
        {
            _Image.enabled = false;
            isShrinking = false;
        }
    }

    public void Hide()
    {
        _Image.enabled = false;
    }

    public void ReEnable()
    {
        transform.localScale = new Vector3(defaultScale, defaultScale, defaultScale);
        _Image.enabled = true;
        _Image.color = defaultColor;
    }
    
    public void UpdateGrow(float scaleFactor)
    {
        _Image.enabled = true;
        _Image.color = transparentColor;
        transform.localScale = new Vector3(defaultScale * scaleFactor, defaultScale * scaleFactor, defaultScale * scaleFactor);
    }
}
