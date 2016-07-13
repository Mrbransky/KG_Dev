using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorArrowTracker : MonoBehaviour
{
    public Transform TargetDoorTransform;
    public Transform TextTransform;
    public Transform IconTransform;
    public GameObject ChildObject;
    public Text TimerText;
    private Image _Image;
    private Text arrowTimerText;
    private Image arrowIconImage;

    private Image[] imageList;
    private Text[] textList;
    private float fadeOutValue = 0.02f;
    private bool isFading = false;

    private float distanceThreshold = 0.1f;
    private float textureMidWidth = 0;
    private float textureMidHeight = 0;

    private bool isTargetOnScreen = false;

    private bool IsTargetOnScreen
    {
        set
        {
            if (isTargetOnScreen != value)
            {
                isTargetOnScreen = value;

                if (isTargetOnScreen && !isFading)
                {
                    disableUI();
                }
                else
                {
                    enableUI();
                }
            }
        }
    }

    void Start()
    {
        _Image = GetComponent<Image>();
        arrowTimerText = TextTransform.GetComponent<Text>();
        arrowIconImage = IconTransform.GetComponent<Image>();

        RectTransform rectTransform = GetComponent<RectTransform>();
        textureMidWidth = rectTransform.rect.width / 2;
        textureMidHeight = rectTransform.rect.height / 2;

        imageList = GetComponentsInChildren<Image>();
        textList = GetComponentsInChildren<Text>();
    }

    void Update()
    {
        if (!isFading)
        {
            //IsTargetOnScreen = updateIsTargetOnScreen();

            if (!isTargetOnScreen)
            {
                updatePosition();
                updateRotation();
                updateUI();
            }
        }
        else
        {
            Color color = Color.white;

            foreach (Image img in imageList)
            {
                color = img.color;
                color.a -= fadeOutValue;
                img.color = color;

                if (color.a <= 0)
                {
                    gameObject.SetActive(false);
                }
            }

            foreach (Text txt in textList)
            {
                color = txt.color;
                color.a -= fadeOutValue;
                txt.color = color;

                if (color.a <= 0)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    private void enableUI()
    {
        ChildObject.SetActive(true);
        _Image.enabled = true;
    }

    private void disableUI()
    {
        ChildObject.SetActive(false);
        _Image.enabled = false;
    }

    private bool updateIsTargetOnScreen()
    {
        Vector3 targetScreenPosition = Camera.main.WorldToScreenPoint(TargetDoorTransform.position);
        return (targetScreenPosition.x > 0 &&
                targetScreenPosition.x < Screen.width &&
                targetScreenPosition.y > 0 &&
                targetScreenPosition.y < Screen.height &&
                TimerText.text == "0");
    }

    private void updatePosition()
    {
        Vector3 newPosition = Camera.main.WorldToScreenPoint(TargetDoorTransform.position);
        newPosition.x = Mathf.Clamp(newPosition.x, textureMidWidth, Screen.width - textureMidWidth);
        newPosition.y = Mathf.Clamp(newPosition.y, textureMidHeight, Screen.height - textureMidHeight);
        transform.position = newPosition;
    }

    private void updateRotation()
    {
        Vector3 direction = Camera.main.ScreenToWorldPoint(transform.position) - TargetDoorTransform.position;

        if (direction != Vector3.zero && direction.magnitude > distanceThreshold)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            TextTransform.rotation = Quaternion.identity;
        }
    }

    private void updateUI()
    {
        if (TimerText.text == "0")
        {
            arrowTimerText.enabled = false;
            arrowIconImage.enabled = true;
        }
        else
        {
            arrowTimerText.text = TimerText.text;
        }
    }

    public void StartFading()
    {
        isFading = true;
    }
}
