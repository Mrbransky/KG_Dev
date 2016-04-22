using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeartComponentManager : MonoBehaviour {

    public Color heartShaderColor;

    public void SetHeartOccluderColors()
    {
        Image[] occludeMats = GetComponentsInChildren<Image>();

        foreach (Image image in occludeMats)
        {
            image.material.SetColor("_OColor", heartShaderColor);
        }
    }
	

}
