using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ScreenFader : MonoBehaviour 
{
    public float MaxFadeInDuration = 1.5f;
    public float MaxFadeOutDuration = 1.5f;
    public int LevelIndexToLoad = -1;

    private Image screenFadeImage;
    private Color fullFadeColor;
    private Color transparentColor;
    private float timeSinceFade = 0;
    private bool isFadingIn = true;
    private bool isFadingOut = false;
    private bool hasFadeIn = false;

    public bool HasFadeIn
    {
        get { return hasFadeIn; }
    }

    void Start()
    {
        screenFadeImage = GetComponent<Image>();
        fullFadeColor = screenFadeImage.color;
        transparentColor = fullFadeColor;
        transparentColor.a = 0;
    }

    void Update()
    {
        if (isFadingIn)
        {
            fadeIn();
        }
        else if (isFadingOut)
        {
            fadeOut();
        }
    }

    private void fadeIn()
    {
        timeSinceFade += Time.deltaTime;
        screenFadeImage.color = Color.Lerp(fullFadeColor, transparentColor, timeSinceFade / MaxFadeInDuration);

        if (timeSinceFade > MaxFadeInDuration)
        {
            isFadingIn = false;
            this.enabled = false;
        }
    }

    private void fadeOut()
    {
        timeSinceFade += Time.deltaTime;
        screenFadeImage.color = Color.Lerp(transparentColor, fullFadeColor, timeSinceFade / MaxFadeOutDuration);

        if (timeSinceFade > MaxFadeOutDuration)
        {
            isFadingOut = false;
            SceneManager.LoadScene(LevelIndexToLoad);
        }
    }

    public void StartFadeOut(int levelIndex)
    {
        LevelIndexToLoad = levelIndex;
        timeSinceFade = 0;
        isFadingOut = true;
    }
}
