using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeartComponentManager : MonoBehaviour {

    //public Color heartShaderColor;
    public Material OccludeMat;

    //public void SetHeartOccluderColors()
    //{
    //    Image[] occludeMats = GetComponentsInChildren<Image>();

    //    foreach (Image image in occludeMats)
    //    {
    //        image.material.SetColor("_OColor", heartShaderColor);
    //    }
    //}

    public void SetHeartMaterial()
    {
        Image[] occludeMats = GetComponentsInChildren<Image>();

        foreach (Image image in occludeMats)
        {
            image.material = OccludeMat;
            image.color = OccludeMat.GetColor("_OColor");
        }
    }
	

}
