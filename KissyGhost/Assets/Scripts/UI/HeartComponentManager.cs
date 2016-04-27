using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeartComponentManager : MonoBehaviour {

    public Material OccludeMat;

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
