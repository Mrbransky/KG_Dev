using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeartComponent : MonoBehaviour
{
    public int heartNum = 0;
    public GameObject HeartOutline;
    
    private float shrinkSpeed = 1.2f;
    private bool isShrinking = false;

    public void Disable()
    {
        HeartOutline.SetActive(false);
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (isShrinking)
        {
            shrink();
        }
    }

    public void StartShrink()
    {
        isShrinking = true;
    }

    private void shrink()
    {
        transform.localScale -= new Vector3(shrinkSpeed, shrinkSpeed, shrinkSpeed) * Time.deltaTime;

        if (transform.localScale.x <= 0)
        {
            GetComponent<Image>().enabled = false;
            isShrinking = false;
        }
    }
}
