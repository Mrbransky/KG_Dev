using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIFlasher : MonoBehaviour
{
    public UIFlasherReference _UIFlasherReference;
    public float targetAlpha = 0.5f;

    private Text _Text;
    private Image _Image;
    private bool isText = true;

    private Color defaultColor;
    private Color transparentColor;

	void Start ()
    {
        _Text = GetComponent<Text>();

        if (_Text == null)
        {
            isText = false;
            _Image = GetComponent<Image>();

            if (_Image == null)
            {
                Debug.LogError(gameObject.name + ": Could not find Text or Image component for UIFlasher.cs");
                this.enabled = false;
            }
            else
            {
                defaultColor = _Image.color;
            }
        }
        else
        {
            defaultColor = _Text.color;
        }

        transparentColor = defaultColor;
        transparentColor.a = targetAlpha;
    }
	
	void Update ()
    {
        if (isText)
        {
            if (!_UIFlasherReference.IsTransparent)
            {
                _Text.color = Color.Lerp(defaultColor, transparentColor, _UIFlasherReference.TimeDelta);
            }
            else
            {
                _Text.color = Color.Lerp(transparentColor, defaultColor, _UIFlasherReference.TimeDelta);
            }
        }
        else
        {
            if (!_UIFlasherReference.IsTransparent)
            {
                _Image.color = Color.Lerp(defaultColor, transparentColor, _UIFlasherReference.TimeDelta);
            }
            else
            {
                _Image.color = Color.Lerp(transparentColor, defaultColor, _UIFlasherReference.TimeDelta);
            }
        }
	}

    public void SetDefaultColor(Color newCol)
    {
        defaultColor = newCol;
        transparentColor = newCol;
        transparentColor.a = targetAlpha;
    }
}
