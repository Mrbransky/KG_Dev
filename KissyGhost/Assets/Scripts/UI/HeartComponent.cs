using UnityEngine;
using System.Collections;

public class HeartComponent : MonoBehaviour
{
    public int heartNum = 0;
    public GameObject HeartOutline;

    public void Disable()
    {
        HeartOutline.SetActive(false);
        gameObject.SetActive(false);
    }
}
